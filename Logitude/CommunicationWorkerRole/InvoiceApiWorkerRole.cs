
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

namespace CommunicationWorkerRole
{
    class InvoiceApiWorkerRole : WorkerEntryPoint
    {

        DbQueueService queueService;
        QueueResponse response = null;
        InvoiceApiService invoiceApiService;
        InvoiceApiCommunicationLogPM invoiceApiCommunicationLog = null;
        CommunicationLog communicationLog = null;

        int tenant = 0;


        public InvoiceApiWorkerRole()
        {

        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "InvoiceApiWorkerRole";
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
            string selectedQueue = "InvoiceApiQueue";
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

                if (response != null && response.Tenant != 0 && response.MessageValues!=null)
                {
                    try
                    {
                       
                        if (response.MessageValues.ContainsKey("communicationLogId"))
                        {
                            string communicationLogId = response.MessageValues["communicationLogId"].ToString();
                            tenant = response.Tenant;
                            GetCommunicationLogAndInvoiceApiLog(communicationLogId);
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
                queueService.InitializeQueue("InvoiceApiQueue", tenant);

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

                var success = invoiceApiService.OpenConnection("", "");
                if (!success) {
                    UpdateCommunicationStatus(InvoiceApiStepEnum.OpenInvoiceApiSession, InvoiceApiStatusEnum.Failed, "OpenConnection Failed");
                    queueService.CompleteAsFailed();

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
                invoiceApiService.EndSession();
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
                if(invoiceApiCommunicationLog != null)
                {
                    InvoiceApiCommunicationLogUpdateService invoiceApiCommunicationLogUpdateService = new InvoiceApiCommunicationLogUpdateService(tenant);
                    invoiceApiCommunicationLog.Step = step;
                    invoiceApiCommunicationLog.StatusCode = status;
                    invoiceApiCommunicationLog.Exception = exception;
                    invoiceApiCommunicationLogUpdateService.Update(invoiceApiCommunicationLog ,true);

                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error updating communication status", null, null);
            }
        }


        public void GetCommunicationLogAndInvoiceApiLog(string communicationLogId)
        {
            try
            {
                if (string.IsNullOrEmpty(communicationLogId))
                    throw new Exception("CommunicationLogId is null or empty");

                ICommonDataContext context = CommonDataContext.GetContext(tenant);
                CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
                communicationLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);

                InvoiceApiCommunicationLogQueryService invoiceApiCommunicationLogQueryService = new InvoiceApiCommunicationLogQueryService(tenant);
                invoiceApiCommunicationLog = invoiceApiCommunicationLogQueryService.GetByCommunicationId(communicationLogId, tenant);
                if (invoiceApiCommunicationLog == null)
                    throw new Exception("InvoiceApiCommunicationLog not found for communicationLogId: " + communicationLogId);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error getting communication log", null, null);
            }
        }
    }
  

}
