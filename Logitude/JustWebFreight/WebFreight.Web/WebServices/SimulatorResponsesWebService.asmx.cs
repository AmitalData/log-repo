using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
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
using WebFreight.Web.Security;
using CHAMP17;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Server.Infrastructure.Azure;
using System.Threading;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Microsoft.ServiceBus.Messaging;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Helpers;
using Logitude.XSD.Simulators;
using Logitude.XSD.Analyzers.CHAMPAnalyzer;
using Logitude.BL.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.QueueService;

using Microsoft.Practices.Unity;
using Logitude.XSD.FSR;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace WebFreight.Web.WebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class SimulatorResponsesWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public FSRResultClass SendRequest(int tenant, string shipmentId, string objectTableId, string myRecipient)
        {
            FSRManager fSRManager = new FSRManager(tenant, SecurityUtility.GetAuthenticatedUser());
            FSRResultClass myResultClass = fSRManager.SendFSR(shipmentId, objectTableId, myRecipient);
            return myResultClass;
        }

        [WebMethod]
        public byte[] GetResponse(int tenant, string entityId, DateTime lastRequestDate)
        {
            byte[] datainByte;
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository commlogRepository = new CommunicationLogRepository(commonContext);


            DocumentRepository documentrepository = new DocumentRepository(commonContext);
            while (true)
            {
                CommunicationLog log = commlogRepository.GetSpecificCommunicationLogForEntity(entityId, tenant, lastRequestDate);
                if (log != null)
                {
                    Document document = documentrepository.GetSingleDocument(tenant, log.DocumentId);
                    Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = document.Tenant,
                        FileSize = document.FileSize,
                    };
                    Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                    datainByte = storageservice.Read(fileInfo);



                    //string containername = "tenant" + tenant.ToString();
                    //CloudBlobContainer blobContainer = StorageAcountDetails.BlobClient.GetContainerReference(containername);
                    //if (blobContainer != null)
                    //{
                    //    string filename = document.Id + "." + document.Extension;

                    //    blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);


                    //    var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));

                    //    if (blobfile.Exists())
                    //    {
                    //        using (MemoryStream memstream = new MemoryStream())
                    //        {

                    //            blobfile.DownloadToStream(memstream);
                    //            datainByte = memstream.ToArray();

                    //        }
                    //        //  string xmlfile = Encoding.ASCII.GetString(DatainByte);
                    //        return datainByte;
                    //    }

                    //    else
                    //        return null;

                    //}


                }
                else
                {
                    Thread.Sleep(2000);
                }

            }



        }

        [WebMethod]
        public byte[] GetXMLDataForCommunicationLog(string communicatinoLogId, int tenant)
        {
            byte[] datainByte;
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository commlogRepository = new CommunicationLogRepository(commonContext);


            DocumentRepository documentrepository = new DocumentRepository(commonContext);

            CommunicationLog log = commlogRepository.GetSingleCommunicationLog(communicatinoLogId, tenant);
            if (log != null)
            {
                Document document = documentrepository.GetSingleDocument(tenant, log.DocumentId);
                Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = document.Tenant,
                    FileSize = document.FileSize,
                };
                Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                datainByte = storageservice.Read(fileInfo);



                return datainByte;

                //string containername = "tenant" + tenant.ToString();
                //CloudBlobContainer blobContainer = StorageAcountDetails.BlobClient.GetContainerReference(containername);
                //if (blobContainer != null)
                //{
                //    string filename = document.Id + "." + document.Extension;

                //    blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);


                //    var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));

                //    if (blobfile.Exists())
                //    {
                //        using (MemoryStream memstream = new MemoryStream())
                //        {

                //            blobfile.DownloadToStream(memstream);
                //            datainByte = memstream.ToArray();

                //        }
                //        //  string xmlfile = Encoding.ASCII.GetString(DatainByte);
                //        return datainByte;
                //    }

                //    else
                //        return null;


                //}
            }
            return null;
        }

        [WebMethod]
        public void SendCommunicationLogToQueue(string communicationLogId, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);

            CommunicationLog commlog = communicationLogRepository.GetSingleCommunicationLog(communicationLogId, tenant);
            commlog.Retries = 0;
            commlog.CommunicationStatusTypeCode = "W";
            commlog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(commlog.Tenant);
            commlog.LastStatusDateUTC = DateTime.UtcNow;
            commlog.NextTryDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            commlog.NextTryDateTimeUTC = DateTime.UtcNow;
            communicationLogRepository.Update(commlog);
            communicationLogRepository.SubmitChanges();

            try
            {
                if (commlog.To == "QBO")
                {
                    #region
                    if (commlog.Subject == "AR Invoice")
                    {
                        ARInvoiceRepository repository = new ARInvoiceRepository(tenant);
                        ARInvoice invoice = repository.GetARInvoiceByInvoiceNumber(tenant, commlog.EntityReference);
                        DbQueueService QBOqueueservice;
                        QBOqueueservice = new DbQueueService();
                        QBOqueueservice.InitializeQueue("QBO", 0);
                        var InvoiceTypeName = "Invoice";
                        if (invoice.ARInvoiceTypeCode == "CD")
                            InvoiceTypeName = "MEMO";
                        Dictionary<string, string> param = new Dictionary<string, string>() { { "QuickbooksOnline", commlog.Id }, { "Tenant", tenant.ToString() }, { "type", InvoiceTypeName } };
                        QBOqueueservice.Send(param);
                        QBOqueueservice.Complete();
                    }

                    else if (commlog.Subject == "AP Invoice")
                    {
                        APInvoiceRepository repository = new APInvoiceRepository(tenant);
                        APInvoice invoice = repository.GetAPInvoiceByInvoiceNumber(tenant, commlog.EntityReference);
                        DbQueueService QBOqueueservice;
                        QBOqueueservice = new DbQueueService();
                        QBOqueueservice.InitializeQueue("QBO", 0);
                        var InvoiceTypeName = "APInvoice";
                        Dictionary<string, string> param = new Dictionary<string, string>() { { "QuickbooksOnline", commlog.Id }, { "Tenant", tenant.ToString() }, { "type", InvoiceTypeName } };
                        QBOqueueservice.Send(param);
                        QBOqueueservice.Complete();
                    }

                    else if (commlog.Subject == "AR Payment")
                    {
                        ARPaymentRepository repository = new ARPaymentRepository(tenant);
                        ARPayment invoice = repository.GetSingleARPayment(commlog.EntityReference, tenant);
                        DbQueueService QBOqueueservice;
                        QBOqueueservice = new DbQueueService();
                        QBOqueueservice.InitializeQueue("QBO", 0);
                        var InvoiceTypeName = "ARPayment";
                        Dictionary<string, string> param = new Dictionary<string, string>() { { "QuickbooksOnline", commlog.Id }, { "Tenant", tenant.ToString() }, { "type", InvoiceTypeName } };
                        QBOqueueservice.Send(param);
                        QBOqueueservice.Complete();
                    }

                    else if (commlog.Subject == "AP Payment")
                    {
                        APPaymentRepository repository = new APPaymentRepository(tenant);
                        APPayment invoice = repository.GetSingleAPPayment(commlog.EntityReference, tenant);
                        DbQueueService QBOqueueservice;
                        QBOqueueservice = new DbQueueService();
                        QBOqueueservice.InitializeQueue("QBO", 0);
                        var InvoiceTypeName = "APPayment";
                        Dictionary<string, string> param = new Dictionary<string, string>() { { "QuickbooksOnline", commlog.Id }, { "Tenant", tenant.ToString() }, { "type", InvoiceTypeName } };
                        QBOqueueservice.Send(param);
                        QBOqueueservice.Complete();
                    }

                    else if (commlog.Subject == "AR Invoice Void")
                    {
                        ARInvoiceRepository repository = new ARInvoiceRepository(tenant);
                        ARInvoice invoice = repository.GetARInvoiceByInvoiceNumber(tenant, commlog.EntityReference);
                        DbQueueService QBOqueueservice;
                        QBOqueueservice = new DbQueueService();
                        QBOqueueservice.InitializeQueue("QBO", 0);
                        var InvoiceTypeName = "ARInvoiceVoid";
                        Dictionary<string, string> param = new Dictionary<string, string>() { { "QuickbooksOnline", commlog.Id }, { "Tenant", tenant.ToString() }, { "type", InvoiceTypeName } };
                        QBOqueueservice.Send(param);
                        QBOqueueservice.Complete();
                    }

                    else if (commlog.Subject == "AP Invoice Credit")
                    {
                        APInvoiceRepository repository = new APInvoiceRepository(tenant);
                        APInvoice invoice = repository.GetAPInvoiceByInvoiceNumber(tenant, commlog.EntityReference);
                        DbQueueService QBOqueueservice;
                        QBOqueueservice = new DbQueueService();
                        QBOqueueservice.InitializeQueue("QBO", 0);
                        var InvoiceTypeName = "VendorCredit";
                        Dictionary<string, string> param = new Dictionary<string, string>() { { "QuickbooksOnline", commlog.Id }, { "Tenant", tenant.ToString() }, { "type", InvoiceTypeName } };
                        QBOqueueservice.Send(param);
                        QBOqueueservice.Complete();
                    }

                    else if (commlog.Subject == "AR Payment Void")
                    {
                        ARPaymentRepository repository = new ARPaymentRepository(tenant);
                        ARPayment invoice = repository.GetSingleARPayment(commlog.EntityReference, tenant);
                        DbQueueService QBOqueueservice;
                        QBOqueueservice = new DbQueueService();
                        QBOqueueservice.InitializeQueue("QBO", 0);
                        var InvoiceTypeName = "ARPaymentVoid";
                        Dictionary<string, string> param = new Dictionary<string, string>() { { "QuickbooksOnline", commlog.Id }, { "Tenant", tenant.ToString() }, { "type", InvoiceTypeName } };
                        QBOqueueservice.Send(param);
                        QBOqueueservice.Complete();


                    }
                    #endregion
                }

                else if (commlog.To == "INTTRA")
                {
                    IQueueService queueservice = new DbQueueService();
                    queueservice.InitializeQueue(commlog.QueueName, 0);
                    queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } });
                }

                else
                {
                    //BrokeredMessage message = new BrokeredMessage();

                    //message.Properties["CommunicationLogId"] = communicationLogId;
                    //message.Properties["Tenant"] = tenant;
                    //// message.TimeToLive = new TimeSpan(0, 15, 0);
                    string emailqueueName = "champmessageoutqueue";//WebFreightEntryPoint.GetQueueByEnviroment("champmessageoutqueue");
                                                                   //QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName);

                    //client.Send(message);
                    if (commlog.CommunicationLogTypeCode == "E")
                    {
                        emailqueueName = "EmailQueue";
                    }

                    if (commlog.QueueName == "SATInterface")
                    {
                        DbQueueService queueservice = new DbQueueService();
                        queueservice.InitializeQueue("SATInterface", 0);
                        Dictionary<string, string> param = new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() }, { "CancellationRequest", (commlog.Subject == "SAT Interface Cancellation Request").ToString() } };
                        queueservice.Send(param);

                    }
                    else
                    {

                        //IQueueService queueservice = QueueServiceManager.GetQueueService(emailqueueName, tenant);
                        //queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } });

						DbQueueService queueservice = new DbQueueService(commlog.QueueName, tenant);
						queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } });
					}

                }
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
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "champ request answer web service", null, ip);
            }
        }
    }
}
