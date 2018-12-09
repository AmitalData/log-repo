using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
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

namespace Logitude.XSD.INTTRA.BL
{
    public class INTTRAHelper
    {
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string LoggedContactId { get; set; }
        public Simplog.Data.CommonDataModel.EntityPOCOs.Contact LoggedContact { get; set; }
        public INTTRAResult Result { get; set; }
        public INTTRADataContext DataContext { get; set; }
        private ICommonDataContext CommonContext;
        public INTTRAHelper(string myShipmentId, int myTenant)
        {
            this.Tenant = myTenant;
            this.ShipmentId = myShipmentId;
            this.CommonContext = CommonDataContext.GetContext(Tenant);

            ContactRepository contactRepository = new ContactRepository(this.CommonContext);
            this.LoggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), Tenant);
            this.LoggedContactId = LoggedContact.Id;

            this.DataContext = new INTTRADataContext(this.Tenant, this.ShipmentId, this.LoggedContact, this.CommonContext);

            this.Result = new INTTRAResult()
            {
                Id = Tenant,
                IsValid = this.DataContext.IsValid,
                IsLimited = this.DataContext.IsLimited,
                Errors = this.DataContext.Errors,
                IsCarrierRegisteredToINTTRA = this.DataContext.IsCarrierRegisteredToINTTRA,
                IsCarrierRegisteredToBranch = this.DataContext.IsCarrierRegisteredToBranch,
            };
        }

        public void Run()
        {
            if (this.DataContext.IsValid)
            {
                if (!this.DataContext.IsLimited)
                {
                    this.DataContext.Build();

                    INTTRADataBuilder dataBuilder = new INTTRADataBuilder(this.DataContext);

                    INTTRA_Out.Message message = new INTTRA_Out.Message()
                    {
                        Header = dataBuilder.GetHeader(),

                        MessageBody = new INTTRA_Out.MessageBody()
                        {
                            MessageDetails = dataBuilder.GetMessageDetails(),

                            MessageProperties = dataBuilder.GetMessageProperties(),
                        }
                    };

                    this.SendXMLFile(message);
                    this.SaveChanges();
                }
            }
        }

        #region Send XML
        public void SendXMLFile(object myRequest)
        {
            this.GetObjectTableData();
            this.UpdateShipmentStatus();

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

            CommunicationLog commLog = this.BuildCommunicationLog(myByteArray.Length);

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

            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(commLog.QueueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", commLog.Id }, { "Tenant", Tenant.ToString() } });

                //using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())
                //{
                //    BrokeredMessage message = new BrokeredMessage();
                //    message.ScheduledEnqueueTimeUtc = System.DateTime.UtcNow.Add(new TimeSpan(0, 0, 10));

                //    message.Properties["CommunicationLogId"] = myCommunicationLogId;
                //    message.Properties["Tenant"] = Tenant;

                //    string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment("sendtointraqueue");
                //    QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName);
                //    client.Send(message);

                //    scope.Complete();
                //}
            }

            catch (Exception ex)
            {
                string ip = "";

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }

                ExceptionHandler.HandleException(ex, System.DateTime.Now, 0, null, "INTTRA controller", null, ip);
            }
        }

        private void UpdateShipmentStatus()
        {
            this.DataContext.Shipment.INTTRASIStatusCode = "SENT";
            this.DataContext.Shipment.INTTRASIStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
            this.DataContext.shipmentRepository.Update(this.DataContext.Shipment);
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

        private string myDocumentId;
        private string myDocumentFolder;
        private string myDocumentExtension;
        private string myCommunicationLogId;
        private CommunicationLog BuildCommunicationLog(int? fileSize)
        {
            DocumentRepository documentRepository = new DocumentRepository(this.CommonContext);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(this.CommonContext);

            string xmlTarget = "INTTRA";
            string xmlSubject = "Shipping Instructions";

            Document document = new Document()
            {
                CreateDate = System.DateTime.Now,
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
                Id = this.DataContext.CommunicationLogIdCounter,
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                LastStatusDateUTC = System.DateTime.UtcNow,
                To = xmlTarget,
                InOut = "O",
                EntityId = ShipmentId,
                ObjectTableId = myObjectTableId,
                Subject = xmlSubject,
                Tenant = Tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = this.LoggedContactId,
                DocumentId = document.Id,
                EntityReference = this.DataContext.ShipmentNumber,
                SearchFields = this.DataContext.ShipmentNumber + "," + xmlTarget + "," + "O" + "," + xmlSubject,
                CreateDateUTC = System.DateTime.UtcNow,
                LogSettings = this.GetLogSettings(xmlSubject),
                QueueName = "FTPCommunicationLogQueue",
            };

            if (this.DataContext.MasterData != null)
            {
                commLog.AWBNumber = this.DataContext.MasterData.Master;
            }

            communicationLogRepository.Add(commLog);

            this.myDocumentId = document.Id;
            this.myDocumentFolder = document.Folder;
            this.myDocumentExtension = document.Extension;
            this.myCommunicationLogId = commLog.Id;
            return commLog;
        }

        private string GetLogSettings(string xmlSubject)
        {
            string myResult = null;

            if (this.DataContext.INTTRA_OutSettingsId != null)
            {
                FTPDetail fTPDetail = (from d in this.CommonContext.FTPDetails where d.Id == this.DataContext.INTTRA_OutSettingsId select d).FirstOrDefault();

                if (fTPDetail != null)
                {
                    string filename = Tenant + "_" + DataContext.ShipmentNumber + "_SI_" + this.DataContext.XMLCreateDate + "_" + this.DataContext.CommunicationLogIdCounter;

                    CommunicationLogSettings settings = new CommunicationLogSettings()
                    {
                        Host = fTPDetail.Host,
                        Folder = fTPDetail.Folder,
                        Username = fTPDetail.UserName,
                        Password = fTPDetail.Password,
                        Filename = filename,
                    };

                    myResult = JsonConvert.SerializeObject(settings);
                }
            }

            return myResult;
        }
        #endregion

        public void SaveChanges()
        {
            this.DataContext.shipmentRepository.SubmitChanges();
            this.CommonContext.SaveChanges();
        }
    }
    public class INTTRAResult
    {
        [Key]
        public int Id { get; set; }
        public bool IsValid { get; set; }
        public bool IsLimited { get; set; }
        public List<string> Errors { get; set; }
        public bool IsCarrierRegisteredToINTTRA { get; set; }
        public bool IsCarrierRegisteredToBranch { get; set; }
        public INTTRAResult()
        {
            this.IsValid = true;
        }
    }
    public class CommunicationLogSettings
    {
        public string Host { get; set; }
        public string Folder { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Filename { get; set; }
    }
}
