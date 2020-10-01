using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;

namespace CommunicationWorkerRole
{
    public class SendToCustomsWorkerRole : WorkerEntryPoint
    {
        IQueueService queueservice;
        private string queueName = "sendtocustomsqueue";
        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        int tenant = 0;
                        queueservice = new DbQueueService(queueName, 0);
                        var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
                        LastActivity = DateTime.UtcNow;

                        if (response.MessageId != null)
                        {
                            string communicationLogId = response.MessageValues["CommunicationLogId"].ToString();
                            int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);

                            ICommonDataContext context = CommonDataContext.GetContext(tenant);
                            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
                            CommunicationLog log = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);

                            if (log != null)
                            {
                                DocumentRepository documentRepository = new DocumentRepository(context);
                                Document document = documentRepository.GetSingleDocument(tenant, log.DocumentId);

                                if (document != null)
                                {
                                    try
                                    {
                                        byte[] fileXml = this.DownloadFile(document, tenant);
                                        string fileBody = UTF8Encoding.UTF8.GetString(fileXml, 0, fileXml.Length);

                                        using (APIWebClient client = new APIWebClient())
                                        {
                                            client.Url = "http://cargowise.customsforce.com/customsforcewebservice.asmx";

                                            XmlDocument doc = new XmlDocument();
                                            doc.LoadXml(fileBody);
                                            System.Xml.XmlDocument xmlDocument = client.ExecAPI(doc);

                                            string StatusCode = null;
                                            XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("StatusCode");
                                            foreach (XmlNode node in xmlNodeList)
                                            {
                                                if (node.Name == "StatusCode")
                                                {
                                                    StatusCode = node.InnerText;
                                                    break;
                                                }
                                            }

                                            if (StatusCode == "-1")
                                            {
                                                string errorText = null;
                                                string errorIdentifier = null;
                                                XmlNodeList errorItems = xmlDocument.GetElementsByTagName("ErrorItem");
                                                foreach (XmlNode errorItem in errorItems)
                                                {
                                                    if (errorText == null && errorIdentifier == null)
                                                    {
                                                        foreach (XmlNode node in errorItem)
                                                        {
                                                            switch (node.Name)
                                                            {
                                                                case "ErrorText":
                                                                    {
                                                                        errorText = node.InnerText;
                                                                        break;
                                                                    }

                                                                case "ErrorIdentifier":
                                                                    {
                                                                        errorIdentifier = node.InnerText;
                                                                        break;
                                                                    }                                                                                                                           
                                                            }
                                                        }
                                                    }

                                                    else
                                                    {
                                                        break;
                                                    }
                                                }

                                                string error = errorText + " [ErrorIdentifier: " + errorIdentifier + "]";
                                                this.UpdateShipment(log, error);
                                                this.UpdateCommunicationLog(log, error);
                                                communicationLogRep.Update(log);
                                                context.SaveChanges();
                                            }

                                            else
                                            {
                                                this.UpdateShipment(log);
                                                this.UpdateCommunicationLog(log);
                                                communicationLogRep.Update(log);
                                                context.SaveChanges();
                                            }

                                            queueservice.Complete();
                                        }
                                    }

                                    catch (Exception ex)
                                    {
                                        //queueservice.CompleteAsFailed();

                                        string error = ex.Message + (ex.InnerException != null ? Environment.NewLine + "InnerException: " + ex.InnerException.Message : "");
                                        this.UpdateShipment(log, error);
                                        this.UpdateCommunicationLog(log, error);
                                        communicationLogRep.Update(log);
                                        context.SaveChanges();                                        
                                                                              
                                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Send to Customs worker role", null, null);
                                        Thread.Sleep(10000);
                                    }
                                }
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Send to Customs worker role", null, null);
                        Thread.Sleep(10000);
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private void UpdateCommunicationLog(CommunicationLog log, string exceptionMessage = null)
        {
            if (exceptionMessage == null)
            {
                log.CommunicationStatusTypeCode = "D";
                log.DoneDate = TenantServerConfigration.GetCurrentDateTime(log.Tenant);
                log.DoneDateUTC = DateTime.UtcNow;
            }

            else
            {
                log.CommunicationStatusTypeCode = "F";
            }

            log.ExceptionMessage = exceptionMessage;
            log.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(log.Tenant);
            log.LastStatusDateUTC = DateTime.UtcNow;
        }

        private void UpdateShipment(CommunicationLog log, string exceptionMessage = null)
        {
            ShipmentRepository rep = new ShipmentRepository(log.Tenant);
            Shipment myEntity = rep.GetSingleShipment(log.EntityId, log.Tenant);
            if (myEntity != null)
            {
                if (exceptionMessage == null)
                {
                    myEntity.LocalCustomsTransmissionsStatusCode = "SENT";
                }

                else
                {
                    myEntity.LocalCustomsTransmissionsStatusCode = "EROR";
                }

                myEntity.LocalCustomsTransmissionsStatusError = exceptionMessage;
                myEntity.LocalCustomsTransmissionsStatusDate = TenantServerConfigration.GetCurrentDateTime(log.Tenant);
                rep.Update(myEntity);
                rep.SubmitChanges();
            }
        }

        private byte[] DownloadFile(Document document, int tenant)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
            };

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            byte[] datainByte = storageservice.Read(fileInfo);
            return datainByte;
        }


    }

    /// <summary>
    ///  This is CustomsWare Web Service wrapper class, implementing web client
    ///  Only ExecAPI function is wrapped
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "CustomsForceWebServiceSoap", Namespace = "http://www.customsware.com/service/CustomsForceWebService")]
    public class APIWebClient : System.Web.Services.Protocols.SoapHttpClientProtocol
    {
        /// <summary>
        ///  Initialises new instance of APIWebClient class
        /// </summary>
        public APIWebClient()
        {
        }

        /// <summary>
        ///  Wrapper for Web Service ExecAPI call
        /// </summary>
        /// <param name="request">XML request - see xsd details for ExecAPI</param>
        /// <returns>XML response from the API server</returns>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.customsware.com/service/CustomsForceWebService/ExecAPI", RequestNamespace = "http://www.customsware.com/service/CustomsForceWebService", ResponseNamespace = "http://www.customsware.com/service/CustomsForceWebService", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlDocument ExecAPI(System.Xml.XmlNode request)
        {
            object[] results = this.Invoke("ExecAPI", new object[] { request });
            return ((System.Xml.XmlDocument)(results[0]));
        }

    }
}
