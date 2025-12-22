using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Diagnostics;
using Logitude.Server.Tools.QueueService;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using Logitude.Accounting.Data;
using Logitude.Server.Tools.Models;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Server.Tools;
using Logitude.BL.InvoiceModel.Tools.Exceptions;

namespace CommunicationWorkerRole
{
    public class ARInvoiceApproveWR : WorkerEntryPoint
    {
        bool _OnStartDone = false;
        DateTime _LastGC = DateTime.MinValue;
        private bool _UseQueue = true;
        private DbQueueService _DbQueueService;
        private string _ObjectTable = "ARInvoice";
        private static DateTime _freeTenantsDateTime = DateTime.Now;
        private bool TenantIdled = false;

        public override void Run()
        {

            while (IsRunning)
            {

                if (General.IsUpdating())
                {
                    Thread.Sleep(60000);
                    continue;
                }
                try
                {
                    WorkOnce();
                    Thread.Sleep(
                        TimeSpan.FromSeconds(
                        .5
                        )

                        );
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "ARInvoiceApproveWR : Run() Method", null);
                    Thread.Sleep(10000);
                }

            }

        }

        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;


                if (!_UseQueue)
                {
                    return true;
                }

                _DbQueueService = new DbQueueService("ARInvoiceApproveWR", 0);


            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "amital send data worker role start", null, null);
            }

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;


            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {

            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {

                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }



        public override void WorkOnce()
        {
            try
            {
                if (DateTime.Now.Subtract(_LastGC) > TimeSpan.FromMinutes(10))
                {
                    _LastGC = DateTime.Now;

                }
                OnStart();

                WorkUntilQEmptyQueueDBMultiThreaded();


            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                _OnStartDone = false;
            }

        }

        public void WorkUntilQEmptyQueueDBMultiThreaded(TimeSpan? timeSpan = null)
        {
            string selectedQueue = "ARInvoiceApproveWR";
            Stopwatch stopwatch = null;
            if (timeSpan != null)
            {
                stopwatch = Stopwatch.StartNew();
            }

            QueueResponse response = null;
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
                    _DbQueueService = new DbQueueService(selectedQueue, 0);
                    if (DateTime.Now.Subtract(_freeTenantsDateTime) >= TimeSpan.FromMinutes(10))
                    {
                        _freeTenantsDateTime = DateTime.Now;
                        _DbQueueService.FreeTenants("ARInvoice");
                    }
                    response = _DbQueueService.ReceiveDetailsByTenant(_ObjectTable, new TimeSpan(0, 0, 0, 5));
                }
                catch (Exception)
                {

                    throw;
                }

                if (response == null || (response != null && response.MessageId == null))
                {
                    break;
                }

                if (response != null && response.Tenant != 0)
                {
                    this.TenantIdled = false;
                    try
                    {
                        this.UpdateInvoice(response);
                    }
                    finally
                    {
                        SetTenantIdle(response.Tenant);
                    }
                }
                Thread.Sleep(10);
            }
        }

        private void UpdateInvoice(QueueResponse response)
        {
            string arinvoiceId = response.MessageValues["ARInvoiceId"].ToString();
            int tenant = 0;
            string invoiceApiCommunicationLogId = null;
            int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);
            string interestReportId = response.MessageValues.ContainsKey("InterestReportId") && response.MessageValues["InterestReportId"] != null
                        ? response.MessageValues["InterestReportId"].ToString()
                        : string.Empty;
            string batchId = response.MessageValues.ContainsKey("BatchIdFromInterestInvoice") && response.MessageValues["BatchIdFromInterestInvoice"] != null
                        ? response.MessageValues["BatchIdFromInterestInvoice"].ToString()
                        : string.Empty;

            ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(tenant);
            ARInvoicePM aRInvoicePM = aRInvoiceQuery.GetSinglePM(arinvoiceId, tenant);
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            ARInvoiceRepository invoiceRepository = new ARInvoiceRepository(tenant);

            if (arinvoiceId != null)
            {
                try
                {
                    invoiceApiCommunicationLogId = response.MessageValues.ContainsKey("invoiceApiCommunicationLogId")
                                                                                 ? response.MessageValues["invoiceApiCommunicationLogId"]?.ToString()
                                                                                  : null;


                    aRInvoicePM.SetApproved = true;
                    aRInvoicePM.IsApprovalFailed = false;
                    if (aRInvoicePM.StatusCode == "AC")
                        aRInvoicePM.SetApprovedAutoCredit = true;

                    ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, tenant);
                    invoiceService.Update(aRInvoicePM, true);
                   
                    _DbQueueService.Complete();
                    SetTenantIdle(tenant);

                    if (!string.IsNullOrEmpty(invoiceApiCommunicationLogId))
                    {
                        try
                        {
                            UpdateInvoiceApiCommunication(tenant, Logitude.Accounting.BL.CloseTables.InvoiceApiStepEnum.ApproveInvoice, Logitude.Accounting.BL.CloseTables.InvoiceApiStatusEnum.Done, invoiceApiCommunicationLogId, null);
                            UpdateInvoiceApiCommunication(tenant, Logitude.Accounting.BL.CloseTables.InvoiceApiStepEnum.PrintOrSendInvoice, Logitude.Accounting.BL.CloseTables.InvoiceApiStatusEnum.InProgress, invoiceApiCommunicationLogId, null);

                            invoiceService.PrintOrSendInvoice(aRInvoicePM.Id, aRInvoicePM.InvoiceNumber, tenant, aRInvoicePM.CreatedByUserId);
                            UpdateInvoiceApiCommunication(tenant, Logitude.Accounting.BL.CloseTables.InvoiceApiStepEnum.PrintOrSendInvoice, Logitude.Accounting.BL.CloseTables.InvoiceApiStatusEnum.Done, invoiceApiCommunicationLogId, null);

                        }
                        catch (Exception e)
                        {
                            UpdateInvoiceApiCommunication(tenant, Logitude.Accounting.BL.CloseTables.InvoiceApiStepEnum.PrintOrSendInvoice, Logitude.Accounting.BL.CloseTables.InvoiceApiStatusEnum.Failed, invoiceApiCommunicationLogId, e.Message );
                        }
                    }


                    ARInvoice invoice = invoiceRepository.GetSingle(arinvoiceId, tenant);
                    invoice.ApprovalInProgress = true;
                    UpdateARInvoiceInRepository(invoice, invoiceRepository);



                    if (aRInvoicePM.ARInvoiceTypeCode == "IT" && !string.IsNullOrEmpty(aRInvoicePM.InvoiceNumber) && aRInvoicePM.InvoiceNumber != aRInvoicePM.Id)
                    {
                        try
                        {
                            aRInvoicePM = aRInvoiceQuery.GetSinglePM(arinvoiceId, tenant); // refresh after update
                            aRInvoicePM.InterestReportId = interestReportId;
                            UpdateInterestReportsStatues(interestReportId, tenant, "2", aRInvoicePM.CreatedByUserId, aRInvoicePM);


                                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                                {
                                    InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
                                    InterestReportPM interestReportPM = interestReportQueryService.GetSingle(interestReportId, false, true);
                                    try
                                    {


                                        invoiceService.PrintOrSendInvoice(aRInvoicePM.Id, aRInvoicePM.InvoiceNumber, tenant, aRInvoicePM.CreatedByUserId);

                                        NetCommonHelper.Logger.DevLog.Instance.WriteTrace("End CreateInvoiceForInterestReport (*3*) aRInvoicePM.Id=" + aRInvoicePM.Id);
                                        scope.Complete();
                                    }
                                    catch (Exception ex)
                                    {
                                        scope.Dispose();
                                        NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, "Error in CreateInvoiceForInterestReport (*4*) interestReport.Id=" + interestReportPM.Id);
                                        UpdateInterestReportsStatues(interestReportId, tenant, "10", aRInvoicePM.CreatedByUserId, null, ex.Message);

                                    }

                                }

                        }
                        catch (Exception ex)
                        {
                            NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, "Error in UpdateInterestInvoiceStatus");
                        }
                    }

                    if (aRInvoicePM.IsAutoCredit)
                    {
                        invoiceService.OnCreatingAutoCredit();
                    }

                }
                catch (InvoiceAlreadyApprovedException) 
                {
                    _DbQueueService.CompleteAsFailed();
                }
                catch (BusinessErrorException ex)
                {
                    
                    BatchTaskExecutionQueryService batchTaskExecutionQueryService = new BatchTaskExecutionQueryService(tenant);
                    BatchTaskExecutionPM batchTask = batchTaskExecutionQueryService.GetSingle(batchId, false, true);
                    if (batchTask == null) return;

                    batchTask.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    batchTask.ErrorLog += "\n" + "Report # " + aRInvoicePM.InterestReportNumber + " " + ex.Message;
                    BatchTaskExecutionUpdateService batchTaskExecutionRepository = new BatchTaskExecutionUpdateService(tenant);
                    batchTaskExecutionRepository.Update(batchTask, true);
                    
                    ARInvoice invoice = invoiceRepository.GetSingle(arinvoiceId, tenant);
                    if (!string.IsNullOrEmpty(aRInvoicePM.InvoiceNumber) && aRInvoicePM.InvoiceNumber != aRInvoicePM.Id)
                    {
                        UpdateCounter(tenant, invoice, invoiceRepository);
                    }
                    InvoiceApprovalFailed(invoice, ex.Message, invoiceRepository);

                }
                catch (Exception ex)
                {
                    
                    ARInvoice invoice = invoiceRepository.GetSingle(arinvoiceId, tenant);
                  
                    if (!string.IsNullOrEmpty(aRInvoicePM.InvoiceNumber) && aRInvoicePM.InvoiceNumber != aRInvoicePM.Id)
                    {
                        UpdateCounter(tenant, invoice, invoiceRepository);
                    }
                    if (response.RetryNumber >= 4 || invoice.StatusCode == "AD")
                    {
                        InvoiceApprovalFailed(invoice, ex.Message, invoiceRepository);
                        if (!string.IsNullOrEmpty(invoiceApiCommunicationLogId))
                        {
                            UpdateInvoiceApiCommunication(tenant, Logitude.Accounting.BL.CloseTables.InvoiceApiStepEnum.ApproveInvoice, Logitude.Accounting.BL.CloseTables.InvoiceApiStatusEnum.Failed, invoiceApiCommunicationLogId, ex.Message);


                        }
                        if (aRInvoicePM.ARInvoiceTypeCode == "IT")
                        {
                            UpdateInterestReportsStatues(interestReportId, tenant, "9", aRInvoicePM.CreatedByUserId, null, ex.Message);
                        }
                    }


                }
            }


        }

        private void SetTenantIdle(int tenant)
        {
            if (!this.TenantIdled)
            {
                TenantIdleStatusRepository tenantRepository = new TenantIdleStatusRepository(tenant);
                TenantIdleStatus tenantObj = tenantRepository.GetAllByObjectTable(tenant, _ObjectTable).FirstOrDefault();
                if (tenantObj == null) return;
                tenantObj.Idle = false;
                tenantObj.UpdateDate = DateTime.Now;
                tenantRepository.Update(tenantObj);
                tenantRepository.SubmitChanges();
                this.TenantIdled = true;
            }
        }





        private void UpdateCounter(int tenant, ARInvoice aRInvoice, ARInvoiceRepository repository)
        {
            CounterStatRepository counterStatRepository = new CounterStatRepository(tenant);
            if (TableCounter.counterState == null) return;

            ObjectTableRepository objectTableRepository = new ObjectTableRepository(0);
            ObjectTable objectTable = objectTableRepository.GetObjectTableByName(_ObjectTable, 0, true);

            var key = TableCounter.counterState.Keys.FirstOrDefault(k => k.EndsWith($"_{tenant}_{objectTable.Id}"));
            if (!string.IsNullOrEmpty(key))
            {
                int id = int.Parse(key.Split('_')[0]);
                CounterStat counterStat = counterStatRepository.GetSingleCounter(id, tenant);
                counterStat.LastValue = int.Parse(TableCounter.counterState[key]) - 1;
                counterStatRepository.Update(counterStat);
                counterStatRepository.SubmitChanges();
            }


            if(aRInvoice.InvoiceNumber != aRInvoice.Id)
            {
                aRInvoice.InvoiceNumber = aRInvoice.Id;
                UpdateARInvoiceInRepository(aRInvoice, repository);
            }
        }



        public void UpdateInterestReportsStatues(string interestReportId, int tenant, string statues, string userId, ARInvoicePM aRInvoicePM = null, string invoiceFailureReason = null)
        {

            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            InterestReportPM interestReportPM = interestReportQueryService.GetSingle(interestReportId, false, true);
            var accountingContext = AccountingContext.GetContext(tenant);

            interestReportPM.InterestReportStatusCode = statues;
            interestReportPM.UpdatedByUserId = userId;
            interestReportPM.UpdateDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            if (invoiceFailureReason != null)
            {
                interestReportPM.InvoiceFailureReason = invoiceFailureReason.Substring(0, Math.Min(invoiceFailureReason.Length, 1024));

            }
            if (aRInvoicePM != null)
            {
                interestReportPM.ARinvoiceId = aRInvoicePM.Id;
                interestReportPM.InvoiceAmount = (decimal?)aRInvoicePM.AmountInLocalCurrency;
            }
            interestReportPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            InterestReportUpdateService service = new InterestReportUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            interestReportPM.IsUpdatedFromBatch = true;
            service.Update(interestReportPM, true);
        }

     
        public void UpdateInvoiceApiCommunication(int tenant , string step, string status,string arInvoiceId, string exception = null)
        {
            InvoiceApiCommunicationLogQueryService invoiceApiCommunicationLogQueryService = new InvoiceApiCommunicationLogQueryService(tenant);
            InvoiceApiCommunicationLogPM invoiceApiCommunicationLog = invoiceApiCommunicationLogQueryService.GetSingle(arInvoiceId, false,false);
            if (invoiceApiCommunicationLog != null && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(step))
            {
                IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
                InvoiceApiCommunicationLogUpdateService invoiceApiCommunicationLogUpdateService = new InvoiceApiCommunicationLogUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
               
                invoiceApiCommunicationLogUpdateService.UpdateCommunicationStatus(invoiceApiCommunicationLog, tenant, step, status, exception);

            }

        }

        public void UpdateARInvoiceInRepository(ARInvoice aRInvoice, ARInvoiceRepository repository = null)
        {
            if(repository == null)
                repository = new ARInvoiceRepository(aRInvoice.Tenant);
            repository.Update(aRInvoice);
            repository.SubmitChanges();
        }


        public void InvoiceApprovalFailed(ARInvoice invoice, string exception, ARInvoiceRepository invoiceRepository) 
        {
            _DbQueueService.CompleteAsFailed();
            invoice.ApprovalInProgress = true;
            invoice.IsApprovalFailed = true;
            invoice.StatusCode = "DR";
            invoice.ApprovedDate = null;
           

            UpdateARInvoiceInRepository(invoice, invoiceRepository);

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = invoice.Tenant,
                EventTypeCode = "APF",
                EntityId = invoice.Id,
                ObjectTableName = _ObjectTable,
                Notes = exception
            });
        }

     

    }
}
