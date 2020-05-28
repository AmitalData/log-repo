using CHAMP17;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Logitude.XSD;
using Logitude.XSD.FSR;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
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
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for FSRWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class FSRWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public FSRResultClass SendRequest(string entityId, string objectTableName, string myRecipient, int tenant)
        {
            FSRResultClass myResultClass = new FSRResultClass()
            {
                Id = tenant,
                IsValid = true,
                IsUpgradingChamp = false,
            };

            try
            {
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                IShipmentsContext shipmentContext = null;
                IBookingContext bookingContext = null; 
                Booking myBooking = null;
                ShipmentPM myShipment = null;

                string myTTY = null;
                string myCCSMessageType = null;
                bool IsDemoTenant = false;
                bool IsEAWBOnlyDemo = false;
                string loggedContactId = null;
                string email = SecurityUtility.GetAuthenticatedUser();
                string entityReference = "";
                string myPrefix = "";
                string myMaster = "";

                #region GetGlobalVariables

                using (TransactionScope scope2 = TransactionFactory.GetNewTransaction())
                {
                    if (tenant != 290)
                    {
                        SettingRepository settingRepository = new SettingRepository();
                        Setting setting = settingRepository.GetSingleSetting("1");
                        if (setting != null)
                        {
                            if (setting.IsUpgradingChamp)
                            {
                                myResultClass.IsValid = false;
                                myResultClass.IsUpgradingChamp = setting.IsUpgradingChamp;
                            }
                        }
                    }

                    if (myResultClass.IsValid)
                    {
                        TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                        TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
                        if (tenantManagement != null)
                        {
                            myTTY = tenantManagement.TTY;
                            myCCSMessageType = tenantManagement.AWBMessagesCCSTypeCode;
                            IsEAWBOnlyDemo = tenantManagement.IsEAWBOnlyDemo;
                        }

                        if (tenant == 65 || IsEAWBOnlyDemo)
                        {
                            IsDemoTenant = true;
                            myResultClass.IsDemoTenant = true;
                        }
                    }

                    scope2.Complete();
                }

                #endregion

                #region Logged Contact

                if (!string.IsNullOrEmpty(email))
                {
                    ContactRepository contactRepository = new ContactRepository(commonContext);
                    Simplog.Data.CommonDataModel.EntityPOCOs.Contact contact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);
                    if (contact != null)
                    {
                        loggedContactId = contact.Id;
                    }
                }

                #endregion

                #region Get Entity Object
                if (objectTableName == "Booking")
                {
                    bookingContext = BookingContext.GetContext(tenant);
                    BookingRepository bookingRepository = new BookingRepository(bookingContext);
                    myBooking = bookingRepository.GetSingle(entityId, tenant);

                    if (myBooking != null)
                    {
                        entityReference = myBooking.BookingNumber;
                        myPrefix = myBooking.AirlinePrefix;
                        myMaster = myBooking.Master;
                    }
                }

                else
                {
                    shipmentContext = ShipmentsContext.GetContext(tenant);
                    ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentContext);
                    ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                    myShipment = shipmentQuery.GetSinglePMWithoutComposition(entityId, tenant);

                    if (myShipment != null)
                    {
                        entityReference = myShipment.ShipmentNumber;
                        myPrefix = myShipment.AirlinePrefix;
                        myMaster = myShipment.Master;
                    }
                }

                #endregion

                if (myResultClass.IsValid)
                {
                    if (!string.IsNullOrEmpty(myPrefix) && !string.IsNullOrEmpty(myMaster))
                    {
                        CHAMP17.StatusRequest myStatusRequest = new CHAMP17.StatusRequest();

                        myPrefix = string.Format("{0:d3}", myPrefix);

                        #region [0] MessageIdentification
                        myStatusRequest.StandardMessageIdentification = new CHAMP17.StandardMessageIdentification()
                        {
                            StandardMessageIdentifier = "FSR",
                        };
                        #endregion

                        #region [1] ConsignmentDetails
                        myStatusRequest.MasterAWBConsignmentDetail = new CHAMP17.FSR_AWBConsignmentDetails()
                        {
                            AWBIdentification = new CHAMP17.AWBIdentification()
                            {
                                AirlinePrefix = myPrefix,
                                AWBSerialNumber = myMaster,
                            },
                        };
                        #endregion

                        #region Build Envelop

                        Envelope envelop = new Envelope()
                        {
                            Sender = myTTY,
                            Recipient = myRecipient,
                            Item = myStatusRequest,
                        };

                        MemoryStream memstream = new MemoryStream();
                        XmlSerializer ser = new XmlSerializer(typeof(Envelope));
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

                        ns.Add("", "http://www.champ.aero/GCCS/CargoXML");

                        XmlWriterSettings settings = new XmlWriterSettings()
                        {
                            Indent = true,
                            IndentChars = "",
                            OmitXmlDeclaration = true,
                            NewLineChars = "",
                            NewLineHandling = NewLineHandling.Replace,
                        };

                        XmlWriter writer = XmlTextWriter.Create(memstream, settings);

                        writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");

                        ser.Serialize(writer, envelop, ns);
                        memstream.Seek(0, SeekOrigin.Begin);
                        var reader = new StreamReader(memstream);
                        string content = reader.ReadToEnd();

                        content = content.Replace(" />", "/>");

                        byte[] bytearray = Encoding.ASCII.GetBytes(content);

                        #endregion

                        #region Document

                        DocumentRepository documentrepository = new DocumentRepository(commonContext);

                        Document document = new Document()
                        {
                            CreateDate = DateTime.Now,
                            Extension = "xml",
                            FileSize = bytearray.Length,
                            Tenant = Convert.ToInt32(tenant),
                            Id = IdCounter.GetNumber("Document", tenant),
                            HasFile = true,
                            Folder = "champ",
                        };

                        documentrepository.Add(document);
                        documentrepository.SubmitChanges();

                        #endregion

                        #region CommunicationLog

                        string myObjectTableId = null;
                        ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(tenant);
                        ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
                        if (objectTable != null)
                        {
                            myObjectTableId = objectTable.Id;
                        }

                        CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);

                        CommunicationLog commLog = new CommunicationLog()
                        {
                            Id = IdCounter.GetNumber("CommunicationLog", tenant),
                            LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                            LastStatusDateUTC = DateTime.UtcNow,
                            To = "Champ",
                            InOut = "O",
                            EntityId = entityId,
                            ObjectTableId = myObjectTableId,
                            Subject = "FSR",
                            Tenant = tenant,
                            CommunicationLogTypeCode = "T",
                            CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                            CommunicationStatusTypeCode = "W",
                            CreatedByUserId = loggedContactId,
                            DocumentId = document.Id,
                            EntityReference = entityReference,
                            CreateDateUTC = DateTime.UtcNow,
                            AWBNumber = myPrefix + "-" + myMaster,
                        };

                        if (IsDemoTenant)
                        {
                            commLog.CommunicationStatusTypeCode = "D";
                            commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            commLog.DoneDateUTC = DateTime.UtcNow;
                        }

                        communicationLogRepository.Add(commLog);
                        communicationLogRepository.SubmitChanges();

                        //myResultClass.CreateDate = commLog.CreateDate;

                        #endregion

                        #region Add to Storage

                        Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                        {
                            FileName = document.Id,
                            FolderName = document.Folder,
                            Extension = document.Extension,
                            Tenant = document.Tenant,
                            FileSize = memstream.Length,
                        };

                        Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                        storageservice.Write(memstream.ToArray(), fileInfo);

                        #endregion

                        #region Update Entity

                        if (objectTableName == "Booking")
                        {
                            myBooking.LastSentByUserId = loggedContactId;
                            myBooking.LastFSRStatusRequestDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            bookingContext.SaveChanges();
                        }

                        else
                        {
                            myShipment.IsFSRSent = true;
                            myShipment.LastSentByUserId = loggedContactId;
                            myShipment.LastFSRStatusRequestDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                            ShipmentService service = new ShipmentService(shipmentContext, myShipment, email);
                            service.Update();

                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = tenant,
                                EventTypeCode = "CRLG",
                                UserId = loggedContactId,
                                EntityId = commLog.Id,
                                ObjectTableName = "CommunicationLog",
                                Notes = commLog.Subject,
                            });

                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = tenant,
                                EventTypeCode = "FSRS",
                                UserId = loggedContactId,
                                EntityId = commLog.EntityId,
                                ObjectTableName = "Shipment",
                                Notes = commLog.Subject,
                            });
                        }

                        #endregion

                        #region Send

                        if (!IsDemoTenant)
                        {
                            try
                            {
                                //using (TransactionScope serializableScope = TransactionFactory.GetNewSerializableTransaction())
                                //{
                                //    BrokeredMessage message = new BrokeredMessage();

                                //    message.Properties["CommunicationLogId"] = commLog.Id;
                                //    message.Properties["Tenant"] = tenant;
                                //    string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment("champmessageoutqueue");

                                //    QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName);

                                //    client.Send(message);

                                //    serializableScope.Complete();
                                //}

                                DbQueueService queueservice = new DbQueueService("champmessageoutqueue", tenant);
                                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", commLog.Id }, { "Tenant", tenant.ToString() } });
                            }

                            catch (Exception ex)
                            {
                                string ip = "";

                                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                                {
                                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                                    if (string.IsNullOrEmpty(currentIP))
                                    {
                                        currentIP = HttpContext.Current.Request.UserHostAddress;
                                    }
                                    ip = currentIP;
                                }

                                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "champ request answer web service", null, null);
                            }
                        }
                        #endregion

                        this.BuildTransmissionLog(tenant, myBooking, myShipment);                        
                    }
                }
            }

            catch (Exception ex)
            {
                return null;

                string ip = "";

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "FSR web service (aka: simulator response web service)", null, ip);
            }

            return myResultClass;
        }

        private void BuildTransmissionLog(int tenant, Booking myBooking, ShipmentPM myShipment)
        {
            MessagesTransmissionHelper TransmissionHelper = new MessagesTransmissionHelper(tenant, "FSR");
            if (myBooking != null)
            {
                TransmissionHelper.Build(myBooking);
            }

            else if (myShipment != null)
            {
                TransmissionHelper.Build(myShipment);
            }
        }
    }
}
