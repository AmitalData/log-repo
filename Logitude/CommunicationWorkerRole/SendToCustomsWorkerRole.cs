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
                        queueservice = QueueServiceManager.GetQueueService(queueName, 0);
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
                                    byte[] fileXml = this.DownloadFile(document, tenant);
                                    string fileBody = UTF8Encoding.UTF8.GetString(fileXml, 0, fileXml.Length);
                                    
                                    try
                                    {
                                        //string innerXml = fileBody;
                                        //string myElement = "<RequestItem>";
                                        //if (innerXml.Contains(myElement))
                                        //{
                                        //    int indexOfElement = innerXml.IndexOf(myElement);
                                        //    int indexOfStart = indexOfElement + myElement.Length;
                                        //    innerXml = innerXml.Insert(indexOfStart, "<ClientReference>ExecAPI</ClientReference>");
                                        //}
                                            
                                        using (APIWebClient client = new APIWebClient())
                                        {
                                            client.Url = "http://www.CustomsForce.com/customsforcewebservice.asmx";

                                            XmlDocument doc = new XmlDocument();                                            
                                            doc.LoadXml(fileBody);
                                            XmlNode resp = client.ExecAPI(doc);

                                            log.CommunicationStatusTypeCode = "D";
                                            log.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                            log.DoneDateUTC = DateTime.UtcNow;
                                            log.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                            log.LastStatusDateUTC = DateTime.UtcNow;
                                            communicationLogRep.Update(log);
                                            context.SaveChanges();

                                            queueservice.Complete();
                                        }
                                    }

                                    catch (Exception ex)
                                    {
                                        log.CommunicationStatusTypeCode = "F";
                                        log.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                        log.LastStatusDateUTC = DateTime.UtcNow;
                                        log.ExceptionMessage = ex.Message + (ex.InnerException != null ? Environment.NewLine + "InnerException: " + ex.InnerException.Message : "");

                                        ShipmentRepository rep = new ShipmentRepository(tenant);
                                        Shipment myEntity = rep.GetSingleShipment(log.EntityId, tenant);
                                        if(myEntity != null)
                                        {
                                            myEntity.LocalCustomsTransmissionsStatusError = log.ExceptionMessage;
                                            myEntity.LocalCustomsTransmissionsStatusCode = "EROR";
                                            myEntity.LocalCustomsTransmissionsStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                                            rep.Update(myEntity);
                                            rep.SubmitChanges();
                                        }

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

        private byte[] DownloadFile(Document document, int tenant)
        {
            try
            {
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                };
                
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                byte[]  datainByte = storageservice.Read(fileInfo);
                return datainByte;               
            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Download File", null, null);
                Thread.Sleep(10000);
                return null;
            }
        }

        private void SetDocNode(XmlDocument root, string nodePath, string value)
        {
            XmlNode current = root.DocumentElement;
            XmlNode next;

            foreach (string nodeName in nodePath.Split(new char[] { '/' }))
            {
                next = current.SelectSingleNode(nodeName);
                if (next != null)
                {
                    current = next;
                    continue;
                }
                current = current.AppendChild(root.CreateNode(XmlNodeType.Element, nodeName, root.NamespaceURI));
            }

            if (!string.IsNullOrEmpty(value) && current != null)
            {
                current.InnerXml = value;
            }
        }

        //private string GetDocNode(XmlNode root, string xpath)
        //{
        //    XmlNode node = root.SelectSingleNode(xpath);
        //    if (node == null)
        //    {
        //        return string.Empty;
        //    }
        //    return node.InnerXml;
        //}

        //private string Print(XmlNode node)
        //{
        //    using (StringWriter sw = new StringWriter())
        //    {
        //        node.WriteTo(new XmlTextWriter(sw) { Indentation = 1, Formatting = Formatting.Indented });
        //        return sw.ToString();
        //    }

        //}
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
        public System.Xml.XmlNode ExecAPI(System.Xml.XmlNode request)
        {
            object[] results = this.Invoke("ExecAPI", new object[] {
                        request});
            return ((System.Xml.XmlNode)(results[0]));
        }

    }
}
