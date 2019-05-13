using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.XSD
{
    public class XmlSender
    {
        public int Tenant { get; set; }
        public string TTY { get; set; }
        public string PIMA { get; set; }
        public string CCSTypeCode { get; set; }
        public bool IsDemoTenant { get; set; }
        public bool IsEAWBOnlyDemo { get; set; }
        public string LoggedContactId { get; set; }
        public string XmlTypeCode { get; set; }
        public XmlSender(int tenant, string xmlTypeCode)
        {
            this.Tenant = tenant;
            this.XmlTypeCode = xmlTypeCode;
            this.GetGlobalVariables();
            this.CheckDemoTenantData();
            this.GetLoggedContactData();
        }

        private void GetGlobalVariables()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(Tenant);

                if (tenantManagement != null)
                {
                    TTY = tenantManagement.TTY;
                    PIMA = tenantManagement.PIMA;
                    CCSTypeCode = tenantManagement.AWBMessagesCCSTypeCode;
                    IsEAWBOnlyDemo = tenantManagement.IsEAWBOnlyDemo;
                }
            }
        }
        private void CheckDemoTenantData()
        {
            if (Tenant == 65)
            {
                this.IsDemoTenant = true;
            }

            else if (XmlTypeCode == "FWB" || XmlTypeCode == "FHL")
            {
                if (IsEAWBOnlyDemo)
                {
                    this.IsDemoTenant = true;
                }
            }
        }
        private void GetLoggedContactData()
        {
            ContactRepository myContactRepository = new ContactRepository(Tenant);
            Simplog.Data.CommonDataModel.EntityPOCOs.Contact myContact = myContactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), Tenant);
            if (myContact != null)
            {
                this.LoggedContactId = myContact.Id;
            }
        }

        #region Settings
        private string myTo;
        private string myFolder;
        private string mySubject;
        private string myEntityId;
        private string myEntityReference;
        private string myObjectTableId;
        private bool isSendingToChamp;
        private string myExceptionHandlerMessage;
        public void SetSettings(string entityId, string entityReference, string objectTableName, bool sendingToChamp)
        {
            this.myEntityId = entityId;
            this.myEntityReference = entityReference;
            this.isSendingToChamp = sendingToChamp;

            if (!string.IsNullOrEmpty(objectTableName))
            {
                ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(Tenant);
                ObjectTable myObjectTable = myObjectTabelRepository.GetObjectTableByName(objectTableName, Tenant, true);
                if (myObjectTable != null)
                {
                    this.myObjectTableId = myObjectTable.Id;
                }
            }

            if (isSendingToChamp)
            {
                this.myTo = "Champ";
                this.myFolder = myTo.ToLower();
                this.mySubject = XmlTypeCode;

                switch (XmlTypeCode)
                {
                    default:
                        {
                            myExceptionHandlerMessage = "Champ web service";
                            break;
                        }
                }
            }

            else
            {
                this.myTo = "GLSHK";
                this.myFolder = myTo.ToLower();
                this.mySubject = XmlTypeCode;

                switch (XmlTypeCode)
                {
                    case "CUSEXP":
                        {
                            myExceptionHandlerMessage = "GLSHK to Customs web service";
                            break;
                        }

                    default:
                        {
                            myExceptionHandlerMessage = "GLSHK web service";
                            break;
                        }
                }
            }
        }
        #endregion

        #region SendXML
        public void Send(object myXmlObject, string queueName)
        {
            if (myXmlObject != null)
            {                 
                ICommonDataContext myCommonContext = CommonDataContext.GetContext(Tenant);
                CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(myCommonContext);
                DocumentRepository documentrepository = new DocumentRepository(myCommonContext);

                Type myXmlObjectType = myXmlObject.GetType();
                XmlSerializer xmlSerializer = new XmlSerializer(myXmlObjectType);
                
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
              
                if (isSendingToChamp)
                {
                    xmlSerializerNamespaces.Add("", "http://www.champ.aero/GCCS/CargoXML");
                }

                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "",
                    OmitXmlDeclaration = true,
                    NewLineChars = "",
                    NewLineHandling = NewLineHandling.Replace,
                };

                XmlWriter writer = XmlTextWriter.Create(memoryStream, xmlWriterSettings);
                writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");

                xmlSerializer.Serialize(writer, myXmlObject, xmlSerializerNamespaces);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var reader = new StreamReader(memoryStream);
                string xmlContent = reader.ReadToEnd();
                xmlContent = xmlContent.Replace(" />", "/>");

                if (xmlContent.Contains("<"))
                {
                    int index = xmlContent.IndexOf('<');
                    if (index > 0)
                    {
                        xmlContent = xmlContent.Substring(index);
                    }
                }

                byte[] bytearray = Encoding.ASCII.GetBytes(xmlContent);

                Document document = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = "xml",
                    FileSize = bytearray.Length,
                    Tenant = Tenant,
                    Id = IdCounter.GetNumber("Document", Tenant),
                    HasFile = true,
                    Folder = myFolder,
                };

                documentrepository.Add(document);

                CommunicationLog commLog = new CommunicationLog()
                {
                    Id = IdCounter.GetNumber("CommunicationLog", Tenant),
                    LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                    LastStatusDateUTC = DateTime.UtcNow,
                    To = myTo,
                    InOut = "O",
                    Subject = mySubject,
                    Tenant = Tenant,
                    CommunicationLogTypeCode = "T",
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                    CommunicationStatusTypeCode = "W",
                    CreatedByUserId = LoggedContactId,
                    DocumentId = document.Id,
                    CreateDateUTC = DateTime.UtcNow,
                    EntityId = myEntityId,
                    EntityReference = myEntityReference,
                    ObjectTableId = myObjectTableId,
                    SearchFields = myEntityReference + "," + myTo + ",O," + mySubject,
                };

                if (IsDemoTenant)
                {
                    commLog.CommunicationStatusTypeCode = "D";
                    commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                    commLog.DoneDateUTC = DateTime.UtcNow;
                }

                communicationLogRepository.Add(commLog);

                myCommonContext.SaveChanges();

                Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = document.Tenant,
                    FileSize = document.FileSize,
                };

                Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                storageservice.Write(bytearray, fileInfo);


                  

                //string filename = document.Id + "." + document.Extension;
                //var blobContainer = StorageAcountDetails.GetCurrentContainer(Tenant);
                //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));

                //using (Stream blobstream = blobfile.OpenWrite())
                //{
                //    blobstream.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
                //}

                if (!IsDemoTenant)
                {
                    try
                    {
                        //using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())//TransactionFactory.GetNewTransaction())
                        //{
                        //    BrokeredMessage message = new BrokeredMessage();
                        //    message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.Add(new TimeSpan(0, 0, 3));

                        //    message.Properties["CommunicationLogId"] = commLog.Id;
                        //    message.Properties["Tenant"] = Tenant;

                        //    string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment(queueName);
                        //    QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName);
                        //    client.Send(message);

                        //    scope.Complete();
                        //}

                        DbQueueService queueservice = new DbQueueService(queueName, Tenant);
                        queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", commLog.Id }, { "Tenant", Tenant.ToString() } });
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

                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, myExceptionHandlerMessage, null, ip);
                    }
                }
            }
        }
        #endregion
    }
}
