using CWXSD;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
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
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;

namespace Logitude.XSD.CW_API.ABM
{
    public class ABMHelper
    {
        public ABMResult Result { get; set; }
        public string ShipmentId { get; set; }
        public Shipment Shipment { get; set; }
        public ShipmentMasterData MasterData { get; set; }
        public int Tenant { get; set; }
        public bool IsValid { get; set; }
        public string LoggedContactId { get; set; }

        private IShipmentsContext shipmentContext;
        private ShipmentRepository shipmentRepository;
        private ShipmentMasterDataRepository shipmentMasterDataRepository;
        private ICommonDataContext commonContext;
        private DocumentRepository documentRepository;
        private CommunicationLogRepository communicationLogRepository;

        public ABMHelper(string myShipmentId, int myTenant)
        {
            this.Tenant = myTenant;
            this.ShipmentId = myShipmentId;
            this.IsValid = true;

            this.Result = new ABMResult()
            {
                IsValid = true,
            };

            if (this.IsValid)
            {
                this.GetShipmentObjects();
                //this.Validate();
            }

            if (this.IsValid)
            {
                commonContext = CommonDataContext.GetContext(Tenant);
                documentRepository = new DocumentRepository(commonContext);
                communicationLogRepository = new CommunicationLogRepository(commonContext);

                ContactRepository contactRepository = new ContactRepository(commonContext);
                Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), Tenant);
                LoggedContactId = loggedContact.Id;
            }
        }

        private void GetShipmentObjects()
        {
            if (shipmentContext == null)
            {
                shipmentContext = ShipmentsContext.GetContext(Tenant);
            }

            this.shipmentRepository = new ShipmentRepository(shipmentContext);
            this.shipmentMasterDataRepository = new ShipmentMasterDataRepository(shipmentContext);

            this.Shipment = shipmentRepository.GetSingleShipment(ShipmentId, Tenant);

            if (Shipment.MasterShipmentDataId != null)
            {
                this.MasterData = shipmentMasterDataRepository.GetSingleMasterData(Shipment.MasterShipmentDataId);
            }

            GetLoggedContact();
        }


        public void Run()
        {
            if (this.IsValid)
            {
                ABMDataContext dataContext = new ABMDataContext(this.ShipmentId, this.Tenant, this.commonContext);
                ABMDataBuilder dataBuilder = new ABMDataBuilder(dataContext);

                CustomsForceServiceRequest request = new CustomsForceServiceRequest()
                {
                    MessageBody = dataBuilder.GetABMBody(),
                    MessageHeader = dataBuilder.GetABMHeader(),
                };

                this.SendXMLFile(request, "sendtocustomsqueue");
                this.SaveChanges();
            }
        }

        #region Send XML
        public void SendXMLFile(object myRequest, string queueName)
        {
            this.GetObjectTableData();
            this.UpdateShipment();

            Type myType = myRequest.GetType();
            MemoryStream myMemoryStream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(myType);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace,
            };

            XmlWriter writer = XmlTextWriter.Create(myMemoryStream, settings);
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");

            ser.Serialize(writer, myRequest, ns);
            myMemoryStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(myMemoryStream);

            string xmlString = reader.ReadToEnd();
            xmlString = xmlString.Replace(" />", "/>");

            byte[] myByteArray = Encoding.ASCII.GetBytes(xmlString);

            this.BuildCommunicationLog(myByteArray.Length);

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = myDocumentId,
                FolderName = myDocumentFolder,
                Extension = myDocumentExtension,
                Tenant = Tenant,
                FileSize = myByteArray.Length,
            };

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(myByteArray, fileInfo);

            //string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment(queueName);
            DbQueueService queueservice = new DbQueueService(queueName, Tenant);
            queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", myCommunicationLogId }, { "Tenant", Tenant.ToString() } });

            //try
            //{
            //    using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())
            //    {
            //        BrokeredMessage message = new BrokeredMessage();
            //        message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.Add(new TimeSpan(0, 0, 10));

            //        message.Properties["CommunicationLogId"] = myCommunicationLogId;
            //        message.Properties["Tenant"] = Tenant;

            //        string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment(queueName);
            //        QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName);
            //        client.Send(message);

            //        scope.Complete();
            //    }
            //}

            //catch (Exception ex)
            //{
            //    string ip = "";

            //    if (HttpContext.Current != null && HttpContext.Current.Request != null)
            //    {
            //        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
            //        if (string.IsNullOrEmpty(currentIP))
            //        {
            //            currentIP = HttpContext.Current.Request.UserHostAddress;
            //        }
            //        ip = currentIP;
            //    }

            //    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "customs controller", null, ip);
            //}
        }

        private string myObjectTableId;
        private void GetObjectTableData()
        {
            ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(Tenant);
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName("Shipment", 0, true);
            if (objectTable != null)
            {
                myObjectTableId = objectTable.Id;
            }
        }
        private void UpdateShipment()
        {
            Shipment.LocalCustomsTransmissionsStatusCode = "SENT";
            Shipment.LocalCustomsTransmissionsStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant);

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = Tenant,
                EventTypeCode = "CUST",
                UserId = LoggedContactId,
                EntityId = ShipmentId,
                ObjectTableName = "Shipment",
                Notes = "via ABM",
            });
        }

        private string myDocumentId;
        private string myDocumentFolder;
        private string myDocumentExtension;
        private string myCommunicationLogId;
        private void BuildCommunicationLog(int? fileSize)
        {
            string xmlTarget = "ABM";
            string xmlSubject = "Customs Request";
            
            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = fileSize,
                Tenant = Convert.ToInt32(Tenant),
                Id = IdCounter.GetNumber("Document", Tenant),
                HasFile = true,
                Folder = xmlTarget.ToLower(),
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();
            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = xmlTarget,
                InOut = "O",
                EntityId = ShipmentId,
                ObjectTableId = myObjectTableId,
                Subject = xmlSubject,
                Tenant = Tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = LoggedContactId,
                DocumentId = document.Id,
                EntityReference = Shipment.ShipmentNumber,
                SearchFields = Shipment.ShipmentNumber + "," + xmlTarget + "," + "O" + "," + xmlSubject,
                CreateDateUTC = DateTime.UtcNow,
            };

            if (this.MasterData != null)
            {
                commLog.AWBNumber = MasterData.Master;
            }

            communicationLogRepository.Add(commLog);

            this.myDocumentId = document.Id;
            this.myDocumentFolder = document.Folder;
            this.myDocumentExtension = document.Extension;
            this.myCommunicationLogId = commLog.Id;
        }
        #endregion

        #region SaveChanges
        public void SaveChanges()
        {
            shipmentRepository.Update(Shipment);
            shipmentContext.SaveChanges();
            commonContext.SaveChanges();
        }
        #endregion        

        private void GetLoggedContact()
        {
            commonContext = CommonDataContext.GetContext(Tenant);
            ContactRepository contactRepository = new ContactRepository(commonContext);
            Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), Tenant);
            this.Shipment.LocalCustomsSentByUserId = loggedContact.Id;
        }
    }

    public class ABMResult
    {
        [Key]
        public int Id { get; set; }
        public bool IsValid { get; set; }
        public ABMResult()
        {
            this.IsValid = true;
        }
    }
}
