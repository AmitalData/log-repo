
using Intuit.Ipp.Data;
using Logitude.Accounting.BL.Interfaces.Magaya;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
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
    class MagayaWorkerRole : WorkerEntryPoint
    {

        DbQueueService queueService;
        QueueResponse response = null;
        MagayaService magayaService;
        int tenant = 0;


        public MagayaWorkerRole()
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
                        tenant = response.Tenant;
                        if (response.MessageValues.ContainsKey("Guid"))
                        {
                            this.WorkOnce();
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
               magayaService = new MagayaService();
               if( magayaService.OpenConnection("TEST","TEZT"))
                {
                    ProcessStep();
                }
                else
                {
                    throw new Exception("Failed to connect to Magaya API");
                }

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
            //switch (switch_on)
            //{
            //    default:
            //}
        }


        private void GetInvoice() {

            var (success, xml) = magayaService.GetTransaction(response.MessageValues["Type"], 1, response.MessageValues["Guid"]);
            if (!success || string.IsNullOrWhiteSpace(xml))
                throw new Exception("GetTransaction failed or returned empty XML");
            GenerateInvoice(xml);

        }
        private void GenerateInvoice(string xml) {

            try {
                ARInvoicePM aRInvoicePM = MapXmlToArinvoice(xml);
                if (aRInvoicePM == null)
                {
                    throw new Exception("Failed to map XML to ARInvoicePM");
                }
                IInvoiceContext MyContext = InvoiceContext.GetContext(tenant);
                ARInvoiceService service = new ARInvoiceService(MyContext, tenant);
                service.Create(aRInvoicePM);
                SetConfirmationNumberStatusInvoice(aRInvoicePM, service);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error generating invoice", null, null);
            }

        }
        private void SetConfirmationNumberStatusInvoice(ARInvoicePM aRInvoicePM, ARInvoiceService service)
        {
            try
            {
                service.SetConfirmationNumberStatus();
                ApproveInvoice(aRInvoicePM, service);
             }
            catch (Exception ex)
            {
                UpdateCommunicationStatus(aRInvoicePM);
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error setting confirmation number status", null, null);
            }
        }
        private void ApproveInvoice(ARInvoicePM aRInvoicePM, ARInvoiceService service) {
            try
            {
                aRInvoicePM.StatusCode = "PR";
                service.Update(aRInvoicePM);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error approving invoice", null, null);
            }
        }
        private void PrintOrSendInvoice(string xml, int tenant) { }
        private void CloseMagayaSession() {}

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


        public void UpdateCommunicationStatus(ARInvoicePM aRInvoicePM)
        {
            try
            {
                if (response != null)
                {
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Error updating communication status", null, null);
            }
        }
    }
  

}
