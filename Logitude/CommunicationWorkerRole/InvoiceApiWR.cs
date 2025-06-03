
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InvoiceModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Logitude.Accounting.BL.Interfaces.Magaya;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Microsoft.Practices.Unity;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Logitude.Server.Tools.StorageService;
using Logitude.Accounting.Data;

namespace CommunicationWorkerRole
{
    class InvoiceApiWR : WorkerEntryPoint
    {

        DbQueueService queueService;
        QueueResponse response = null;
        InvoiceApiService invoiceApiService;
        InvoiceApiCommunicationLogPM invoiceApiCommunicationLog = null;

        int tenant = 0;


        public InvoiceApiWR()
        {

        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "InvoiceApiWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();
            return base.OnStart();
        }


        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try { 
                    
                        ExecuteQueue();
                    }
                    catch (Exception exception)
                    {
                        ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "InvoiceApi Worker Role queue worker role start", null, null);
                        Thread.Sleep(new TimeSpan(0, 0, 1));
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }






        public void ExecuteQueue(TimeSpan? timeSpan = null)
        {
            string selectedQueue = "InvoiceApiWR";
            Stopwatch stopwatch = null;
            if (timeSpan != null)
            {
                stopwatch = Stopwatch.StartNew();
            }

            response = null;
            while (true)
            {
                if (stopwatch != null && timeSpan != null)
                {
                    if (stopwatch.Elapsed > timeSpan)
                    {
                        return;
                    }
                }
                try
                {
                    queueService = new DbQueueService(selectedQueue, 0);
                    response = queueService.Receive(new TimeSpan(0, 0, 1));
                    
                }
                catch (Exception)
                {

                    throw;
                }

                if (response == null || (response != null && response.MessageId == null))
                {
                    break;
                }

                if (response != null  && response.MessageValues!=null)
                {
                    try
                    {

                        if (response?.MessageValues?.ContainsKey("InvoiceApiId") == true)
                        {
                            string InvoiceApiId = response.MessageValues["InvoiceApiId"].ToString();
                            GeInvoiceApiLog(InvoiceApiId);
                            tenant = invoiceApiCommunicationLog.Tenant;
                             WorkOnce();
                        }

                    }
                    catch
                    {
                        throw;
                    }
                    
                }



                Thread.Sleep(10);
            }
        }


        private void ConnectClient()
        {
            try
            {
                queueService = new DbQueueService();
                queueService.InitializeQueue("InvoiceApiWR", tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "InvoiceApi worker role start", null, null);
            }
        }


        private void WorkOnce()
        {
            try
            {
               invoiceApiService = new InvoiceApiService();
               ProcessStep();
                queueService.Complete();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "InvoiceApi worker role start", null, null);
                queueService.CompleteAsFailed();
            }
        }

        private void ProcessStep()
        {


            if (invoiceApiCommunicationLog == null)
                throw new Exception("InvoiceApiCommunicationLog not found for GUID: " + response.MessageValues["Guid"]);
           
            switch (invoiceApiCommunicationLog.Step)
            {
                case InvoiceApiStepEnum.OpenInvoiceApiSession:
                    OpenInvoiceApiSession();
                    break;
              
                case InvoiceApiStepEnum.GetInvoiceApiInvoice:
                    GetInvoice();
                    break;
                case InvoiceApiStepEnum.CloseInvoiceApiSession:
                    CloseInvoiceApiSession();
                    break;
                case InvoiceApiStepEnum.GenerateInvoice:
                    GenerateInvoice(null);
                    break;
                case InvoiceApiStepEnum.GetConfirmationNumber:
                    SetConfirmationNumberStatusInvoice(null,null);
                    break;
                case InvoiceApiStepEnum.ApproveInvoice:
                    ApproveInvoice(null,null);
                    break;
                case InvoiceApiStepEnum.PrintOrSendInvoice:
                    PrintOrSendInvoice(null,tenant);
                    break;
                
                default:
                    throw new Exception("Unknown step: " + invoiceApiCommunicationLog.Step);
            }
        }

        private void OpenInvoiceApiSession()
        {
            try
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.OpenInvoiceApiSession, InvoiceApiStatusEnum.InProgress);
                invoiceApiService = new InvoiceApiService();

                var success = invoiceApiService.OpenConnection();
                if (!success) {
                    throw new Exception("OpenConnection Failed");
                  }

                else
                {
                    UpdateCommunicationStatus(InvoiceApiStepEnum.OpenInvoiceApiSession, InvoiceApiStatusEnum.Done);
                    GetInvoice();
                }
            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.OpenInvoiceApiSession, InvoiceApiStatusEnum.Failed, ex.Message);
                queueService.CompleteAsFailed();

            }

        }
        private void GetInvoice() {

            try
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetInvoiceApiInvoice, InvoiceApiStatusEnum.InProgress);
                var (success, xml) = invoiceApiService.GetTransaction(response.MessageValues["Type"], 1, response.MessageValues["Guid"]);
                if (!success || string.IsNullOrWhiteSpace(xml))
                    throw new Exception("GetTransaction failed or returned empty XML");
                AddDocumentToApiCommunicationLog(System.Text.Encoding.UTF8.GetBytes(xml));
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetInvoiceApiInvoice, InvoiceApiStatusEnum.Done);

                CloseInvoiceApiSession();
            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetInvoiceApiInvoice, InvoiceApiStatusEnum.Failed, ex.Message);

                queueService.CompleteAsFailed();
            }


        }
        private void CloseInvoiceApiSession()
        {
            try
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.CloseInvoiceApiSession, InvoiceApiStatusEnum.InProgress);
                if(!invoiceApiService.EndSession()) throw new Exception("Failed to CloseSession");

                UpdateCommunicationStatus(InvoiceApiStepEnum.CloseInvoiceApiSession, InvoiceApiStatusEnum.Done);
                GenerateInvoice(null);
            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.CloseInvoiceApiSession, InvoiceApiStatusEnum.Failed, ex.Message);
                queueService.CompleteAsFailed();

            }
        }
        private void GenerateInvoice(string xml) {

            try {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GenerateInvoice, InvoiceApiStatusEnum.InProgress);
                ARInvoicePM aRInvoicePM = MapXmlToArinvoice(xml);
                if (aRInvoicePM == null)
                {
                    throw new Exception("Failed to map XML to ARInvoicePM");
                }
                IInvoiceContext MyContext = InvoiceContext.GetContext(tenant);
                ARInvoiceService service = new ARInvoiceService(MyContext, tenant);
                service.Create(aRInvoicePM);

                UpdateCommunicationStatus(InvoiceApiStepEnum.GenerateInvoice, InvoiceApiStatusEnum.Done);

                SetConfirmationNumberStatusInvoice(aRInvoicePM, service);

            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GenerateInvoice, InvoiceApiStatusEnum.Failed,ex.Message);

                queueService.CompleteAsFailed();
            }

        }
        private void SetConfirmationNumberStatusInvoice(ARInvoicePM aRInvoicePM, ARInvoiceService service)
        {
            try
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetConfirmationNumber, InvoiceApiStatusEnum.InProgress);

                service.SetConfirmationNumberStatus();
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetConfirmationNumber, InvoiceApiStatusEnum.Done);

                ApproveInvoice(aRInvoicePM, service);
             }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetConfirmationNumber,InvoiceApiStatusEnum.Failed,ex.Message);
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error setting confirmation number status", null, null);
            }
        }
        private void ApproveInvoice(ARInvoicePM aRInvoicePM, ARInvoiceService service) {
            try
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.ApproveInvoice, InvoiceApiStatusEnum.InProgress);

                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("ARInvoiceApproveWR", tenant);
                queueservice.Send(new Dictionary<string, string>()
                {
                    { "ARInvoiceId", aRInvoicePM.Id },
                    { "Tenant", tenant.ToString() },
                    { "BatchIdFromInterestInvoice", null },
                    {"IsInvoiceApi", "true" }
                   }, tenant);
            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.ApproveInvoice, InvoiceApiStatusEnum.Failed, ex.Message);

                queueService.CompleteAsFailed();
            }
        }
        private void PrintOrSendInvoice(string xml, int tenant) { }
       




        private ARInvoicePM MapXmlToArinvoice(string xml)
        {
            try
            {
                ARInvoicePM aRInvoicePM = new ARInvoicePM();
                XDocument xdoc = XDocument.Parse(xml);
                var invoice = xdoc.Descendants("Invoice").FirstOrDefault();
                if (invoice != null)
                {
                    aRInvoicePM.IsExternalEntity = false;
                    aRInvoicePM.SetApproved = false;


                    aRInvoicePM.AccountingExternalName = invoice.Element("AccountingExternalName")?.Value;
                }
                return aRInvoicePM;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error parsing XML", null, null);
                return null;
            }
        }


        public void UpdateCommunicationStatus(string step , string status ,string exception = null)
        {
            try
            {
                if(invoiceApiCommunicationLog != null && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(step))
                {
                    IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
                    InvoiceApiCommunicationLogUpdateService invoiceApiCommunicationLogUpdateService = new InvoiceApiCommunicationLogUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                    invoiceApiCommunicationLog.Step = step;
                    invoiceApiCommunicationLog.StatusCode = status;
                    invoiceApiCommunicationLog.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    if (exception != null)
                    {
                        var exceptionDict = new Dictionary<string, object>
                        {
                            { "exception", exception },
                            { "MessageValues", response?.MessageValues }
                        };
                        invoiceApiCommunicationLog.Exception = Newtonsoft.Json.JsonConvert.SerializeObject(exceptionDict);
                    }
                    else
                    {
                        invoiceApiCommunicationLog.Exception = null;
                    }
                    invoiceApiCommunicationLogUpdateService.Update(invoiceApiCommunicationLog ,true);

                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error updating communication status", null, null);
            }
        }


        public void GeInvoiceApiLog(string invoiceApiId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(invoiceApiId))
                    throw new ArgumentException("InvoiceApiId is null or empty", nameof(invoiceApiId));

                var queryService = new InvoiceApiCommunicationLogQueryService(tenant);
                invoiceApiCommunicationLog = queryService.GetSingle(invoiceApiId, false,false) ??  throw new InvalidOperationException($"Log not found for InvoiceApiId: {invoiceApiId}");
                if (invoiceApiCommunicationLog == null)
                    throw new Exception("InvoiceApiCommunicationLog not found for InvoiceApiId: " + invoiceApiId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void SaveToBlob(byte[] byteData, Document document)
        {
            var filename = $"{document.Id}.{document.Extension}";
            var filePath = $"tenant{tenant}/{StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder)}";


            var fileInfo = new BlobFileInfo
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,
                FileSize = byteData.Length
            };

            var blobService = (IBlobService)ContainerAccessor.Container.Resolve(
                typeof(IBlobService), "StorageService", new ParameterOverride("", 1));

            blobService.Write(byteData, fileInfo);
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine($"SetBlob: {filePath}");
        }
        public void AddDocumentToApiCommunicationLog(byte[] ByteData)
        {
            try
            {

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                    UserRepository userRepository = new UserRepository(commonContext);
                    DocumentRepository documentrepository = new DocumentRepository(commonContext);
                    ObjectTableRepository objecttableRep = new ObjectTableRepository(tenant);
                    IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
                    InvoiceApiCommunicationLogUpdateService invoiceApiCommunicationLogUpdateService = new InvoiceApiCommunicationLogUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);

                    Document document = new Document()
                    {
                        CreateDate = DateTime.Now,
                        Extension = "xml",
                        FileSize = ByteData.Length,
                        Tenant = Convert.ToInt32(tenant),
                        Id = IdCounter.GetNumber("Document", tenant),
                        HasFile = true,
                        Folder = "others",
                    };

                    documentrepository.Add(document);
                    documentrepository.SubmitChanges();

                    invoiceApiCommunicationLog.DocumentId = document.Id;
                    invoiceApiCommunicationLog.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                    invoiceApiCommunicationLogUpdateService.Update(invoiceApiCommunicationLog, true);


                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                   
                    SaveToBlob(ByteData, document);

                    stopwatch.Stop();


                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }


}
