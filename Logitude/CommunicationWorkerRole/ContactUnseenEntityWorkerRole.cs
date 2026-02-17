using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using WebFreight.Web.Helpers;

namespace CommunicationWorkerRole
{
    public class ContactUnseenEntityWorkerRole : WorkerEntryPoint
    {
        QueueDescription queueDescription;
        QueueClient client;
        private bool _OnStartDone;
        string contactunseenQueueName;
        string statusCode = string.Empty;
        ContactRepository contactRepository;
        ContactsUnseenEntitieRepository contactsUnseenEntitieRepository;
        MobileNotificationLogRepository mobileNotificationLogRepository;
        IQueryable<ContactMobileDevice> ContactMobileDevicesList = null;
        TraceEventRepository traceEventRepository;
        ShipmentRepository shipmentRepository;
        string tenantName = "";    
        List<string> NotificationIds = new List<string>();

        public override void Run()
        {
            int? tenant = null;
            while (IsRunning)
            {
                try
                {
                    var message = client.Receive(new TimeSpan(0, 0, 10));
                    LastActivity = DateTime.UtcNow;
                    string traceEventId = null;
                    string statusName = "";
                    DateTime? sourceEventDate = null;
                    bool IsException = false;


                    if (message != null)
                    {
                        LastActivity = DateTime.UtcNow;
                        try
                        {
                            if (message.Properties.Keys.Contains("TraceEventId")) traceEventId = message.Properties["TraceEventId"].ToString();

                            if (message.Properties.Keys.Contains("Tenant")) tenant = (int)message.Properties["Tenant"];
                            if (message.Properties.Keys.Contains("TenantName")) tenantName = message.Properties["TenantName"].ToString();
                            if (message.Properties.Keys.Contains("SourceEventDate")) sourceEventDate = (DateTime?)message.Properties["SourceEventDate"];

                            if (traceEventId == null || tenant == null)
                            {
                                message.Complete();
                                continue;
                            }


                            traceEventRepository = new TraceEventRepository((int)tenant);
                            TraceEvent traceEvent = traceEventRepository.GetSingleTraceEvent(traceEventId);

                            if (traceEvent == null)
                            {
                                DelayQueueMessage(message);
                                continue;
                            }

                            EventTypeRepository eventTypeRepository = new EventTypeRepository((int)tenant);
                            EventType eventType = eventTypeRepository.GetSingleEventType(traceEvent.EventTypeId, (int)tenant);


                            if (!eventType.IsCustomerView)
                            {
                                message.Complete();
                                continue;
                            }

                            if (eventType.EntityStatus == null && eventType.Code != "EXCE")
                            {
                                message.Complete();
                                continue;
                            }

                            shipmentRepository = new ShipmentRepository((int)tenant);
                            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext((int)tenant);


                            statusCode = eventType.EntityStatus != null ? eventType.EntityStatus.Code : "";
                            statusName = eventType.EntityStatus != null ? eventType.EntityStatus.Name : "";


                            CustomShipmentList shipment = GetShipment(tenant, traceEvent, shipmentsContext, statusCode, eventType.Code);


                            if (shipment == null)
                            {
                                message.Complete();
                                continue;
                            }

                            if (string.IsNullOrEmpty(shipment.CustomerId))
                            {
                                message.Complete();
                                continue;
                            }


                            ICommonDataContext commoncontext = CommonDataContext.GetContext((int)tenant);

                            CardContactRepository cardContactRep = new CardContactRepository(commoncontext);

                            contactsUnseenEntitieRepository = new ContactsUnseenEntitieRepository((int)tenant);

                            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                            {
                                mobileNotificationLogRepository = new MobileNotificationLogRepository();
                                scope.Complete();
                            }

                            contactRepository = new ContactRepository((int)tenant);


                            List<CardContact> customerContacts = cardContactRep.GetCardContactsByCardId(shipment.CustomerId).Where(c => c.InternetAccess).ToList();

                            List<ContactPassword> contactPasswords = null;
                            List<string> emails = null;

                            if ((!string.IsNullOrEmpty(statusCode) && statusCode != "SHOR") || eventType.Code == "EXCE")
                            {
                                emails = (from a in customerContacts  select a.Contact.Email).ToList();
                                contactPasswords = new List<ContactPassword>();

                                using (TransactionScope scope = TransactionFactory.GetTransaction())
                                {
                                    ContactPasswordRepository contactPasswordRepository = new ContactPasswordRepository();
                                    contactPasswords = contactPasswordRepository.context.ContactPasswords.Where(c => emails.Contains(c.Email)).ToList();

                                    if (contactPasswords != null)
                                    {
                                        contactPasswords = contactPasswords.Where(d => d.Email != null).ToList();
                                    }

                                    scope.Complete();
                                }


                                ContactMobileDeviceRepository contactMobileDeviceRepository = new ContactMobileDeviceRepository();
                                ContactMobileDevicesList = contactMobileDeviceRepository.GetAllContactMobileDevicesByEmails(emails);

                            }

                            #region  ShipmentLevelCode A

                            IQueryable<CustomShipmentList> ShipmentLists = null;
                            if (shipment.ShipmentLevelCode == "A" && shipment.CustomConnectToShipment && !string.IsNullOrEmpty(statusCode) && statusCode != "SHOR")
                            {
                                ShipmentLists = (from a in shipmentsContext.Shipments.Include("Ports")//.Include("ShipperCard").Include("ConsigneeCard")
                                                 join sm in shipmentsContext.ShipmentMasterDatas.Include("Port")
                                                 on a.MasterShipmentDataId equals sm.Id into shipmentJoin
                                                 from m in shipmentJoin.DefaultIfEmpty()
                                                 where a.CustomFileId == shipment.Id && a.Tenant == shipment.Tenant && a.CustomConnectToShipment == false && a.ShipmentLevelCode != "A"
                                                 select new CustomShipmentList()
                                                 {
                                                     Id = a.Id,
                                                     ShipperName = a.ShipperName,//a.ShipperCard != null ? a.ShipperCard.EnglishName : "",
                                                     ConsigneeName = a.ConsigneeName, //a.ConsigneeCard != null ? a.ConsigneeCard.EnglishName : "",
                                                     DirectionId = a.DirectionId,
                                                     ShipmentLevelCode = a.ShipmentLevelCode,
                                                     ShipperReference1 = a.ShipperReference1,
                                                     ShipperReference2 = a.ShipperReference2,
                                                     AgentReference1 = a.AgentReference1,
                                                     AgentReference2 = a.AgentReference2,
                                                     ConsigneeReference1 = a.ConsigneeReference1,
                                                     ConsigneeReference2 = a.ConsigneeReference2,
                                                     FromPortName = !string.IsNullOrEmpty(m.MainCarriageFromPort.Code) ? m.MainCarriageFromPort.Code : a.FromPort.Code,
                                                     ToPortName = !string.IsNullOrEmpty(m.MainCarriageToPort.Code) ? m.MainCarriageToPort.Code : a.ToPort.Code,
                                                     ForeignPartnerCountryCode = a.ForeignPartnerCountryCode,           
                                                     TransportModeId = a.TransportModeId,
                                                     CustomFileId = a.CustomFileId,

                                                 });
                            }


                            #endregion

                            #region Create ContactUnseenEntity And CreateMobileNotificationLog
                            NotificationIds = new List<string>();
                            foreach (CardContact cardcontact in customerContacts)
                            {
                                if (eventType.Code == "EXCE") IsException = true;


                                    ContactsUnseenEntitie contactsUnseenEntitieentity = new ContactsUnseenEntitie() { CreateDate = TenantServerConfigration.GetCurrentDateTime(traceEvent.Tenant),Id = Guid.NewGuid().ToString(), Tenant = traceEvent.Tenant, EntityId = traceEvent.EntityId, ObjectTableId = traceEvent.ObjectTableId, ContactId = cardcontact.ContactId };
                                    contactsUnseenEntitieRepository.Add(contactsUnseenEntitieentity);
                             


                                //NotificationRecord
                                if ((!string.IsNullOrEmpty(statusCode) && statusCode != "SHOR") || eventType.Code == "EXCE")
                                {
                                    if (cardcontact.Contact != null && !string.IsNullOrEmpty(cardcontact.Contact.Email))
                                    {

                                        ContactPassword contactPassword = contactPasswords.Where(d => d.Email.ToLower() == cardcontact.Contact.Email.ToLower()).FirstOrDefault();

                                        if (contactPassword != null)
                                        {
                                            if (shipment.ShipmentLevelCode == "A" && shipment.CustomConnectToShipment)
                                            {
                                                if (ShipmentLists != null && ShipmentLists.Count() > 0)
                                                {
                                                    foreach (CustomShipmentList item in ShipmentLists)
                                                    {
                                                        CreateMobileNotificationLog(contactPassword, traceEvent.Tenant, traceEvent.ObjectTableId, item.Id, statusName, IsException, traceEvent.Notes, item, sourceEventDate);
                                                    }
                                                }
                                            }

                                            else
                                            {
                                                CreateMobileNotificationLog(contactPassword, traceEvent.Tenant, traceEvent.ObjectTableId, traceEvent.EntityId, statusName, IsException, traceEvent.Notes, shipment, sourceEventDate);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion


                            contactsUnseenEntitieRepository.SubmitChanges();
                            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                            {
                                mobileNotificationLogRepository.SubmitChanges();
                                scope.Complete();
                            }


                            if (NotificationIds.Count > 0)
                            {
                                foreach (string notificationId in NotificationIds)
                                {
                                    using (TransactionScope scope = TransactionFactory.GetNewTransactionWithDefaultIsolationLevel())
                                    {

                                        QueueClient clientnotificationsLog = ServiceBusQueueHelper.CreateMobileNotificationsLogQueue(traceEvent.Tenant);
                                        BrokeredMessage messageotifications = new BrokeredMessage();

                                        messageotifications.Properties["Tenant"] = traceEvent.Tenant;
                                        messageotifications.Properties["NotificationId"] = notificationId;
                                        messageotifications.Properties["TenantName"] = tenantName;
                                        
                                        clientnotificationsLog.Send(messageotifications);
                                        scope.Complete();
                                    }
                                }
                            }

                            
                            message.Complete();
                            LogDoneItemInMemory();
                        }

                        catch (Exception ex)
                        {
                            ExceptionHandler.HandleException(ex, DateTime.Now, (int)tenant, "", "ContactUnseenWorkerRole", "", null);
                            DelayQueueMessage(message);
                      
                        }
                     
                    }
                    
                }
                catch (Exception ex)
                {

                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "unseen worker role start", null, null);

                    client = StorageAcountDetails.CreateServiceBusQueueClient(contactunseenQueueName);

                    Thread.Sleep(10000);
                }
            }
        }

        private static CustomShipmentList GetShipment(int? tenant, TraceEvent traceEvent, IShipmentsContext shipmentsContext, string statuscode, string eventtypecode)
        {

            CustomShipmentList shipment = null;
            if ((!string.IsNullOrEmpty(statuscode) && statuscode != "SHOR") || eventtypecode == "EXCE")
            {
                shipment = (from a in shipmentsContext.Shipments.Include("Ports")
                            join sm in shipmentsContext.ShipmentMasterDatas.Include("Port")
                            on a.MasterShipmentDataId equals sm.Id into shipmentJoin
                            from m in shipmentJoin.DefaultIfEmpty()
                            where a.Tenant == tenant && a.Id == traceEvent.EntityId
                            select new CustomShipmentList
                            {
                                Id = a.Id,
                                Tenant = a.Tenant,
                                CustomerId = a.CustomerId,
                                DirectionId = a.DirectionId,
                                ShipmentLevelCode = a.ShipmentLevelCode,
                                ShipperReference1 = a.ShipperReference1,
                                ShipperReference2 = a.ShipperReference2,
                                AgentReference1 = a.AgentReference1,
                                AgentReference2 = a.AgentReference2,
                                ConsigneeReference1 = a.ConsigneeReference1,
                                ConsigneeReference2 = a.ConsigneeReference2,
                                FromPortName = !string.IsNullOrEmpty(m.MainCarriageFromPort.Code) ? m.MainCarriageFromPort.Code : a.FromPort.Code,
                                ToPortName = !string.IsNullOrEmpty(m.MainCarriageToPort.Code) ? m.MainCarriageToPort.Code : a.ToPort.Code,
                                ShipperName = a.ShipperName,//a.ShipperCard != null ? a.ShipperCard.EnglishName : "",
                                ConsigneeName = a.ConsigneeName, //a.ConsigneeCard != null ? a.ConsigneeCard.EnglishName : "",
                                CustomConnectToShipment = a.CustomConnectToShipment,
                                ForeignPartnerCountryCode = a.ForeignPartnerCountryCode,
                                TransportModeId = a.TransportModeId,
                                CustomFileId = a.CustomFileId,

                            }).FirstOrDefault();
            }
            else
            {
                shipment = (from a in shipmentsContext.Shipments
                            where a.Tenant == tenant && a.Id == traceEvent.EntityId
                            select new CustomShipmentList
                            {
                                Id = a.Id,
                                Tenant = a.Tenant,
                                CustomerId = a.CustomerId,
                                ForeignPartnerCountryCode = a.ForeignPartnerCountryCode,
                                DirectionId = a.DirectionId,
                               TransportModeId = a.TransportModeId,
                                CustomFileId = a.CustomFileId,
                            }).FirstOrDefault();



            }
            return shipment;
        }


        private static void DelayQueueMessage(BrokeredMessage message)
        {
            if (message.DeliveryCount < 11)
            {
                if (message.DeliveryCount >= 3 && message.DeliveryCount <= 5)
                {
                    //    Thread.Sleep(new TimeSpan(0, 0, 10));
                    message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.AddSeconds(5);
                }

                if (message.DeliveryCount > 5 && message.DeliveryCount <= 10)
                {
                    //Thread.Sleep(new TimeSpan(0, 0, 30));
                    message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.AddSeconds(10);
                }
                if (message.DeliveryCount == 11)
                {
                    message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.AddSeconds(60);
                }
                message.Abandon();
            }
            else
            {
                // add error log
                message.Complete();
            }
        }

        private void CreateContactUnseenEntity(string contactId, int tenant, string objectTableId, string entityId, bool isException, string statusName, string shipmentnumber, string notes)
        {

            ContactsUnseenEntitie contactsUnseenEntitieentity = new ContactsUnseenEntitie() { CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant), Id = Guid.NewGuid().ToString(), Tenant = tenant, EntityId = entityId, ObjectTableId = objectTableId, ContactId = contactId };
            contactsUnseenEntitieRepository.Add(contactsUnseenEntitieentity);
            contactsUnseenEntitieRepository.SubmitChanges();

        }


        private void CreateMobileNotificationLog(ContactPassword contactPassword, int tenant, string objectTableId, string entityId, string statusName, bool isException, string exceptionNote, CustomShipmentList shipment, DateTime? sourceEventDate)
        {

            if (!contactPassword.IsSendNotificationForMobile) return;
            if (contactPassword.SharedMobileAppAlertonExceptions && !isException) return;
            if (contactPassword.SharedMobileAppAlertsforFollowedShipment)
            {
                SharedFollowedShipmentRepository sharedFollowedShipmentRepository = new SharedFollowedShipmentRepository(tenant);
                SharedFollowedShipment sharedFollowedShipment = sharedFollowedShipmentRepository.GetSingleSharedFollowedShipmentByShipmentId(entityId);
                if (sharedFollowedShipment == null) return;

            }

            #region BuildMessageNotification

            string message = string.Empty;
            string messageIOS = string.Empty;
            string messageAndroid = string.Empty;
            string routing = string.Empty;
            string routingios = string.Empty;
            string refernce = string.Empty;
            string supplier = string.Empty;
            double appversion = 0;
            bool isnewDesign = false;

            if (shipment != null)
            {
                routing = shipment.FromPortName + " > " + shipment.ToPortName;
                routingios = shipment.FromPortName + ">" + shipment.ToPortName;
                refernce = GetMobileReference(shipment,null);
                if (!string.IsNullOrEmpty(shipment.CustomFileId))
                {
                    if (string.IsNullOrEmpty(refernce))
                    {
                        ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                        ShipmentList shipmentList = shipmentQuery.GetShipmentListBycustomFileIdForMobile(shipment.CustomFileId, tenant);
                        if (shipmentList != null)
                        {
                            refernce = GetMobileReference(null, shipmentList);
                        }
                    }
                }



                supplier = GetSupplier(shipment);
            }

     
            if (ContactMobileDevicesList != null)
            {
                string version = ContactMobileDevicesList.Where(d => d.Email.ToLower() == contactPassword.Email.ToLower()).OrderByDescending(d => d.UpdateDate).Select(d => d.AppVersion).FirstOrDefault();
                if (!string.IsNullOrEmpty(version)) double.TryParse(version, out appversion);
                else appversion = 1.01;

            }


            if (appversion > 1.01 || LogitudeSettings.WorkEnvironment != "cloud")
            {
                if (isException)
                {
                    if (exceptionNote.Length > 200)  message = messageIOS = messageAndroid = exceptionNote.Substring(0,195);
                    else message = messageIOS = messageAndroid = exceptionNote;
                }

                else message = messageIOS = messageAndroid = "Status : " + statusName;

                if (!string.IsNullOrEmpty(supplier)) messageIOS = messageAndroid = message += '\n' + supplier; 
          
                if (!string.IsNullOrEmpty(routing)) messageAndroid = message += '\n' + "Route : " + routing;
         
                if (!string.IsNullOrEmpty(refernce))
                {
                   messageAndroid= message += '\n' + "Your Ref : " + refernce;
                   messageIOS += '\n' + "Your Ref : " + refernce;
                }
                messageIOS = tenantName + "  -  " + routingios + '\n' + messageIOS;
                isnewDesign = true;
            }
            else
            {
                 if (isException)
                    {
                        if (exceptionNote.Length > 200) message = messageIOS = messageAndroid = exceptionNote.Substring(0, 195);
                        else message = messageIOS = messageAndroid = exceptionNote;
                    }
             
                else message = messageIOS = messageAndroid = "Status changed to " + statusName;
                isnewDesign = false;
            }


            #endregion
           
            MobileNotificationLog mobileNotificationLog = new MobileNotificationLog() { Id = Guid.NewGuid().ToString(), NotificationMessage = message, EntityId = entityId, ObjectTableId = objectTableId, AndroidStatus = "W", IOSStatus = "W", Email = contactPassword.Email, Tenant = tenant, CreateDate = DateTime.UtcNow, IsException = isException, SourceEventDate = sourceEventDate, NotificationMessageAndroid = messageAndroid, NotificationMessageIOS = messageIOS };
            NotificationDetails notificationDetails = new NotificationDetails()
            {
                Route = routing,
                Refernce = refernce,
                Supplier = supplier,
                StatusName = statusName,
                Message = message,
                Tenant = tenant,
                Forwarder = tenantName,
                ForeignPartnerCountryCode = shipment.ForeignPartnerCountryCode,
                TransportModeId = shipment.TransportModeId,
                DirectionId = shipment.DirectionId,
                IsNewDesignNotification = isnewDesign,
                EntitiyId = shipment.Id,
                IsRead = false,
                ExceptionNote = exceptionNote,
                IsException = isException,
                NotificationId = mobileNotificationLog.Id
                
            };
   
         mobileNotificationLog.XML = LogitudeXmlSerializer.SerializeObjectToXmlString(notificationDetails);

            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                mobileNotificationLogRepository.Add(mobileNotificationLog);
                NotificationIds.Add(mobileNotificationLog.Id);
                scope.Complete();
            }


        }

        public string GetSupplier(CustomShipmentList shipment)
        {
            string customer = string.Empty;

            if (shipment.DirectionId == "I" || shipment.DirectionId == "C")
            {
                if (!string.IsNullOrEmpty(shipment.ShipperName)) customer = "Shipper : " + shipment.ShipperName;
            }

            else
            {
                if (!string.IsNullOrEmpty(shipment.ConsigneeName)) customer = "Consignee : " + shipment.ConsigneeName;

            }

            return customer;
        }

  
        private string GetMobileReference(CustomShipmentList customShipmentList, ShipmentList shipmentlist)
        {
            string _myRef = "";

            string shipmentLevelCode = customShipmentList != null ? customShipmentList.ShipmentLevelCode : shipmentlist.ShipmentLevelCode;
            string directionId = customShipmentList != null ? customShipmentList.DirectionId : shipmentlist.DirectionId;


            string agentReference1 = customShipmentList != null ? customShipmentList.AgentReference1 : shipmentlist.AgentReference1;
            string agentReference2 = customShipmentList != null ? customShipmentList.AgentReference2 : shipmentlist.AgentReference2;

            string shipperReference1 = customShipmentList != null ? customShipmentList.ShipperReference1 : shipmentlist.ShipperReference1;
            string shipperReference2 = customShipmentList != null ? customShipmentList.ShipperReference2 : shipmentlist.ShipperReference2;

            string consigneeReference2 = customShipmentList != null ? customShipmentList.ConsigneeReference2 : shipmentlist.ConsigneeReference2;
            string consigneeReference1 = customShipmentList != null ? customShipmentList.ConsigneeReference1 : shipmentlist.ConsigneeReference1;

            if (shipmentLevelCode == "C")
            {
                _myRef = agentReference1;
                if (!string.IsNullOrEmpty(agentReference2))
                {
                    _myRef = _myRef == "" ? agentReference2 : _myRef + ", " + agentReference2;
                }
            }
            else
            {
                if (directionId == "E" || directionId == "R")
                {
                    _myRef = shipperReference1;
                    if (!string.IsNullOrEmpty(shipperReference2))
                    {
                        _myRef = _myRef == "" ? shipperReference2 : _myRef + ", " + shipperReference2;
                    }
                }

                else
                {
                    _myRef = consigneeReference1;
                    if (!string.IsNullOrEmpty(consigneeReference2))
                    {
                        _myRef = _myRef == "" ? consigneeReference2 : _myRef + ", " + consigneeReference2;
                    }
                }
            }

            if (_myRef == ", ") _myRef = "";

            return _myRef;
        }




        public override bool OnStart()
        {
            try
            {
                contactunseenQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment("contactunseenentityqueue");
                if (!StorageAcountDetails.NameSpaceManager.QueueExists(contactunseenQueueName))
                {
                    queueDescription = new QueueDescription(contactunseenQueueName);
                    queueDescription.MaxSizeInMegabytes = 5120;
                    queueDescription.EnableDeadLetteringOnMessageExpiration = false;
                    StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
                }

                client = StorageAcountDetails.CreateServiceBusQueueClient(contactunseenQueueName);
            }
            catch (Exception ex)
            {
                //  ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
            }

            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;


            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ContactUnseenEntity";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {

            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {

                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

    } 

    public class CustomShipmentList
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public int Tenant { get; set; }
        public string DirectionId { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }
        public string AgentReference1 { get; set; }
        public string AgentReference2 { get; set; }
        public string ConsigneeReference1 { get; set; }
        public string ConsigneeReference2 { get; set; }
        public string FromPortName { get; set; }
        public string ToPortName { get; set; }
        public string ConsigneeName { get; set; }
        public string ShipperName { get; set; }
        public string ShipmentLevelCode { get; set; }
        public bool CustomConnectToShipment { get; set; }
        public string ForeignPartnerCountryCode { get; set; }
        public string CustomFileId { get; set; }
        public string TransportModeId { get; set; }
 
        
    }


}