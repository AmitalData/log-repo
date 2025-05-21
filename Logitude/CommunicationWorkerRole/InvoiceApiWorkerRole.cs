
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Interfaces.Magaya;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools;
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
using WebFreight.Web.Helpers.WorkerRoleHelpers;

namespace CommunicationWorkerRole
{
    class InvoiceApiWorkerRole : WorkerEntryPoint
    {

        DbQueueService queueService;
        QueueResponse response = null;
        InvoiceApiService magayaService;
        MagayaCommunicationLogPM magayaCommunicationLog = null;
        CommunicationLog communicationLog = null;

        int tenant = 0;


        public InvoiceApiWorkerRole()
        {

        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "MagayaWorkerRole";
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
                        ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Magaya Worker Role queue worker role start", null, null);
                        Thread.Sleep(new TimeSpan(0, 0, 1));
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }






        public void ExecuteQueue(TimeSpan? timeSpan = null)
        {
            string selectedQueue = "MagayaQueue";
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
                            GetCommunicationLogAndMagayaLog(communicationLogId);
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
                queueService.InitializeQueue("MagayaQueue", tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Magaya worker role start", null, null);
            }
        }


        private void WorkOnce()
        {
            try
            {
               magayaService = new InvoiceApiService();
               ProcessStep();
                queueService.Complete();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Magaya worker role start", null, null);
                queueService.CompleteAsFailed();
            }
        }

        private void ProcessStep()
        {


            if (magayaCommunicationLog == null)
                throw new Exception("MagayaCommunicationLog not found for GUID: " + response.MessageValues["Guid"]);
           
            switch (magayaCommunicationLog.Step)
            {
                case InvoiceApiStepEnum.OpenMagayaSession:
                    OpenMagayaSession();
                    break;
              
                case InvoiceApiStepEnum.GetMagayaInvoice:
                    GetInvoice();
                    break;
                case InvoiceApiStepEnum.CloseMagayaSession:
                    CloseMagayaSession();
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
                    throw new Exception("Unknown step: " + magayaCommunicationLog.Step);
            }
        }

        private void OpenMagayaSession()
        {
            try
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.OpenMagayaSession, MagayaStatusEnum.InProgress);
                magayaService = new InvoiceApiService();

                var success = magayaService.OpenConnection("", "");
                if (!success) {
                    UpdateCommunicationStatus(InvoiceApiStepEnum.OpenMagayaSession, MagayaStatusEnum.Failed, "OpenConnection Failed");
                    queueService.CompleteAsFailed();

                }

                else
                {
                    UpdateCommunicationStatus(InvoiceApiStepEnum.OpenMagayaSession, MagayaStatusEnum.Done);
                    GetInvoice();
                }
            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.OpenMagayaSession, MagayaStatusEnum.Failed, ex.Message);
                queueService.CompleteAsFailed();

            }

        }
        private void GetInvoice() {

            try
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetMagayaInvoice, MagayaStatusEnum.InProgress);
                var (success, xml) = magayaService.GetTransaction(response.MessageValues["Type"], 1, response.MessageValues["Guid"]);
                if (!success || string.IsNullOrWhiteSpace(xml))
                    throw new Exception("GetTransaction failed or returned empty XML");
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetMagayaInvoice, MagayaStatusEnum.Done);

                CloseMagayaSession();
            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetMagayaInvoice, MagayaStatusEnum.Failed, ex.Message);

                queueService.CompleteAsFailed();
            }


        }
        private void CloseMagayaSession()
        {
            try
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.CloseMagayaSession, MagayaStatusEnum.InProgress);
                magayaService.EndSession();
                UpdateCommunicationStatus(InvoiceApiStepEnum.CloseMagayaSession, MagayaStatusEnum.Done);
                GenerateInvoice(null);
            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.CloseMagayaSession, MagayaStatusEnum.Failed, ex.Message);
                queueService.CompleteAsFailed();

            }
        }
        private void GenerateInvoice(string xml) {

            try {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GenerateInvoice, MagayaStatusEnum.InProgress);
                ARInvoicePM aRInvoicePM = MapXmlToArinvoice(xml);
                if (aRInvoicePM == null)
                {
                    throw new Exception("Failed to map XML to ARInvoicePM");
                }
                IInvoiceContext MyContext = InvoiceContext.GetContext(tenant);
                ARInvoiceService service = new ARInvoiceService(MyContext, tenant);
                service.Create(aRInvoicePM);

                UpdateCommunicationStatus(InvoiceApiStepEnum.GenerateInvoice, MagayaStatusEnum.Done);

                SetConfirmationNumberStatusInvoice(aRInvoicePM, service);

            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GenerateInvoice, MagayaStatusEnum.Failed,ex.Message);

                queueService.CompleteAsFailed();
            }

        }
        private void SetConfirmationNumberStatusInvoice(ARInvoicePM aRInvoicePM, ARInvoiceService service)
        {
            try
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetConfirmationNumber, MagayaStatusEnum.InProgress);

                service.SetConfirmationNumberStatus();
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetConfirmationNumber, MagayaStatusEnum.Done);

                ApproveInvoice(aRInvoicePM, service);
             }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.GetConfirmationNumber,MagayaStatusEnum.Failed,ex.Message);
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error setting confirmation number status", null, null);
            }
        }
        private void ApproveInvoice(ARInvoicePM aRInvoicePM, ARInvoiceService service) {
            try
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.ApproveInvoice, MagayaStatusEnum.InProgress);

                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("ARInvoiceApproveWR", tenant);
                queueservice.Send(new Dictionary<string, string>()
                {
                    { "ARInvoiceId", aRInvoicePM.Id },
                    { "Tenant", tenant.ToString() },
                    { "BatchIdFromInterestInvoice", null },
                    {"IsMagaya", "true" }
                   }, tenant);
            }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(InvoiceApiStepEnum.ApproveInvoice, MagayaStatusEnum.Failed, ex.Message);

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
                if(magayaCommunicationLog != null)
                {
                    MagayaCommunicationLogUpdateService magayaCommunicationLogUpdateService = new MagayaCommunicationLogUpdateService(tenant);
                    magayaCommunicationLog.Step = step;
                    magayaCommunicationLog.StatusCode = status;
                    magayaCommunicationLog.Exception = exception;
                    magayaCommunicationLogUpdateService.Update(magayaCommunicationLog ,true);

                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error updating communication status", null, null);
            }
        }


        public void GetCommunicationLogAndMagayaLog(string communicationLogId)
        {
            try
            {
                if (string.IsNullOrEmpty(communicationLogId))
                    throw new Exception("CommunicationLogId is null or empty");

                ICommonDataContext context = CommonDataContext.GetContext(tenant);
                CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
                communicationLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);

                MagayaCommunicationLogQueryService magayaCommunicationLogQueryService = new MagayaCommunicationLogQueryService(tenant);
                magayaCommunicationLog = magayaCommunicationLogQueryService.GetByCommunicationId(communicationLogId, tenant);
                if (magayaCommunicationLog == null)
                    throw new Exception("MagayaCommunicationLog not found for communicationLogId: " + communicationLogId);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error getting communication log", null, null);
            }
        }
    }
  

}
