using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationWorkerRole
{
    public class DocumentFilingBackupQueueBuilderWR : WorkerEntryPoint
    {
        IQueueService queueservice;
      

      
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DocumentFilingBackupQueueBuilderWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            ConnectClient();
            return base.OnStart();
        }
        string CorrelationId;
        int Tenant = 0;
        public override  void Run()
        {
            try
            {
               

                while (IsRunning)
                {
                    if (!General.IsUpdating())
                    {
                        try
                        {
                            queueservice = new DbQueueService();
                            queueservice.InitializeQueue("DocumentFilingBackupQueueBuilderQueue", 0);
                            var response = queueservice.Receive();
                            LastActivity = DateTime.UtcNow;
                          
                            if (response != null && response.MessageId != null)
                            {

                                string BatchId = response.MessageValues["BatchId"].ToString();
                                int.TryParse(response.MessageValues["Tenant"], out Tenant);
                               
                                if (!string.IsNullOrEmpty(BatchId))
                                {
                                    try
                                    {
                                        DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(Tenant);
                                        DocumentFilingBackupBatchRepository documentFilingBackupBatchRepository = new DocumentFilingBackupBatchRepository(Tenant);
                                        DocumentFilingBackupBatch documentsBatch = documentFilingBackupBatchRepository.GetSingleDocumentFilingBackupBatch(BatchId, Tenant);
                                        
                                        if (documentsBatch != null)
                                        {
                                            List<string> documentIds = documentsFilingQuery.GetDocumentFilingIdsByTenantCreateDate(Tenant, documentsBatch.FromDatetime.Value, documentsBatch.ToDatetime.Value, documentsBatch.IncludeBackedUp);
                                            if (documentIds != null && documentIds.Count > 0)
                                            {
                                                documentsBatch.TotalDocuments = documentIds.Count();
                                                documentFilingBackupBatchRepository.Update(documentsBatch);
                                                documentFilingBackupBatchRepository.SubmitChanges();

                                                foreach (var DocumentFilingPMId in documentIds)
                                                {
                                                    queueservice.InitializeQueue("DocumentFilingBackupBatchQueue", 0);
                                                    queueservice.Send(new Dictionary<string, string>() { { "DocumentFilingId", DocumentFilingPMId }, { "Tenant", Tenant.ToString() }, { "BatchId", BatchId } }, null, BatchId, documentsBatch.BatchNumber);

                                                }
                                            }
                                            else
                                            {
                                                documentsBatch.Status = "Done";
                                                documentsBatch.DoneDate = TenantServerConfigration.GetCurrentDateTime(documentsBatch.Tenant);
                                                documentFilingBackupBatchRepository.Update(documentsBatch);
                                                documentFilingBackupBatchRepository.SubmitChanges();

                                                queueservice.Complete();
                                            }
                                        }

                                        queueservice.Complete();
                                        LogDoneItemInMemory();
                                    }
                                    catch (Exception ex)
                                    {
                                        #region Exception handling
                                        ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, "", "WorkerRole", "", null);

                                        var Failmsg = ex.Message + " " + DateTime.Now;
                                        string errorMessage = ex.Message + Environment.NewLine;

                                        if (ex.InnerException != null)
                                        {

                                            errorMessage = errorMessage + " (" + ex.InnerException.Message + ")" + Environment.NewLine;

                                        }

                                        errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                                        if (response.RetryNumber <= 1)
                                        {
                                            queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                                        }

                                        if (response.RetryNumber > 1 && response.RetryNumber <= 2)
                                        {
                                            queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                                        }
                                        if (response.RetryNumber >= 3)
                                        {
                                            queueservice.CompleteAsFailed();
                                            
                                        }



                                        #endregion
                                    }
                                }
                                else
                                {
                                    queueservice.Complete();
                                }
                            }
                            else
                            {
                                Thread.Sleep(10000);
                            }
                        }
                        catch (Exception ex)
                        {
                            ConnectClient();
                            ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DocumentFilingBackupQueueBuilderWR start", null, null);
                            Thread.Sleep(10000);
                        }

                    }
                    else
                    {
                        Thread.Sleep(60000);
                    }
                }
            }
            catch (Exception ex)
            {
                ConnectClient();
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DocumentFilingBackupQueueBuilderWR start", null, null);
                Thread.Sleep(10000);
            }
        }

        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("DocumentFilingBackupQueueBuilderQueue", Tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DocumentFilingBackupQueueBuilderWR start", null, null);
            }
        }

    }
}
