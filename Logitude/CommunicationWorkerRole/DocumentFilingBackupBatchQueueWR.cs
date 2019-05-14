using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.FTP;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
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
    public class DocumentFilingBackupBatchQueueWR : WorkerEntryPoint
    {
        IQueueService queueservice;
        DocumentFilingBackupBatch documentsBatch;
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DocumentFilingBackupBatchQueueWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            ConnectClient();
            return base.OnStart();
        }
        string CorrelationId;
        int Tenant = 0;
        public override void Run()
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
                            queueservice.InitializeQueue("DocumentFilingBackupBatchQueue", 0);
                            var response = queueservice.Receive();
                            LastActivity = DateTime.UtcNow;
                            DocumentFilingBackupBatchRepository documentFilingBackupBatchRepository = new DocumentFilingBackupBatchRepository(Tenant);

                            if (response != null && response.MessageId != null)
                            {

                                string DocumentFilingId = response.MessageValues["DocumentFilingId"].ToString();
                                int.TryParse(response.MessageValues["Tenant"], out Tenant);
                                string BatchId = response.MessageValues["BatchId"].ToString();

                                DocumentFilingBackupBatch documentsBatch = documentFilingBackupBatchRepository.GetSingleDocumentFilingBackupBatch(BatchId, Tenant);
                                if (!string.IsNullOrEmpty(DocumentFilingId))
                                {
                                    try
                                    {
										string p_message = "";
										DocumentsFilingBackupHelper.UploadDocumentToFTP(DocumentFilingId,Tenant, out p_message);
										//documentsBatch.Logs += Environment.NewLine + DateTime.Now.ToString() + " : " + p_message;
										if (documentsBatch != null)
                                        {
                                            documentsBatch.TotalSucceeded += 1;
                                            documentFilingBackupBatchRepository.Update(documentsBatch);
                                            documentFilingBackupBatchRepository.SubmitChanges();
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
                                            if (documentsBatch != null)
                                            {
                                                documentsBatch.TotalFailed += 1;
                                                documentFilingBackupBatchRepository.Update(documentsBatch);
                                                documentFilingBackupBatchRepository.SubmitChanges();
                                            }

                                            queueservice.CompleteAsFailed();
                                        }



                                        #endregion
                                    }
                                    finally
                                    {
                                        if (documentsBatch != null)
                                        {
                                            if (documentsBatch.TotalDocuments == (documentsBatch.TotalSucceeded + documentsBatch.TotalFailed))
                                            {
                                                documentsBatch.Status = "Done";
                                                documentsBatch.DoneDate = TenantServerConfigration.GetCurrentDateTime(documentsBatch.Tenant);
                                            }
                                            else if (documentsBatch.TotalFailed > 0)
                                            {
                                                documentsBatch.Status = "Failed";
                                            }
                                            else
                                            {
                                                documentsBatch.Status = "In Progress";
                                            }
                                            documentFilingBackupBatchRepository.Update(documentsBatch);
                                            documentFilingBackupBatchRepository.SubmitChanges();
                                        }
                                    }
                                }
                                else
                                {
                                    queueservice.CompleteAsFailed();
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
                            ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DocumentFilingBackupBatchQueueWR start", null, null);
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
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DocumentFilingBackupBatchQueueWR start", null, null);
                Thread.Sleep(10000);
            }
        }

        

        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("DocumentFilingBackupBatchQueue", Tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DocumentFilingBackupBatchQueueWR start", null, null);
            }
        }

    }
}
