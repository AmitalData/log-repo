using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;

namespace CommunicationWorkerRole
{
    public class BIReportsExecutionLogWorkerRole : WorkerEntryPoint
    {
        DbQueueService queueservice;
        int tenant = 0;

        public BIReportsExecutionLogWorkerRole()
        {

        }

        public override async void AsyncRun()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        queueservice = new DbQueueService("BIReportsExecutionLogQueue", 0);
                        var response = queueservice.Receive(new TimeSpan(0, 0, 1));
                        LastActivity = DateTime.UtcNow;

                        string tenantString = null;

                        if (response != null && response.MessageId != null)
                        {
                            BIReportsExecutionLog reportExecutionLog = null;
                            BIReportsExecutionLogRepository reportExecutionLogRepository = null;

                            try
                            {
                                string bIReportExecutionLogId = response.MessageValues.Keys.Contains("BIReportExecutionLogId") ? response.MessageValues["BIReportExecutionLogId"].ToString() : "";
                                if (response.MessageValues.Keys.Contains("Tenant"))
                                {
                                    tenantString = response.MessageValues["Tenant"].ToString();
                                    if (!string.IsNullOrEmpty(tenantString)) tenant = int.Parse(tenantString);
                                }

                                if (string.IsNullOrEmpty(bIReportExecutionLogId) || string.IsNullOrEmpty(tenantString))
                                {
                                    queueservice.Complete();
                                    continue;
                                }

                                reportExecutionLogRepository = new BIReportsExecutionLogRepository(tenant);
                                reportExecutionLog = reportExecutionLogRepository.GetSingleBIReportExecutionLog(bIReportExecutionLogId, tenant);

                                if (reportExecutionLog == null)
                                {
                                    queueservice.Complete();
                                    continue;
                                }

                                if (reportExecutionLog.StatusCode != "W")
                                {
                                    queueservice.Complete();
                                    continue;
                                }

                                BIReportXMLData reportFliter = null;
                                if (!string.IsNullOrEmpty(reportExecutionLog.ReportFilterXML))
                                {
                                    reportFliter = LogitudeXmlSerializer.DeserializeObject<BIReportXMLData>(reportExecutionLog.ReportFilterXML);
                                }

                                if (reportFliter != null)
                                {
                                    Thread thread = new Thread(() => BuildReport(reportFliter, reportExecutionLog, reportExecutionLogRepository, queueservice, response));
                                    thread.IsBackground = true;
                                    thread.Start();
                                }
                                else
                                {
                                    BIReportExecutionLogArgs updateReportExecutionLogArgs = new BIReportExecutionLogArgs() { ReportExecutionLog = reportExecutionLog, ReportExecutionLogRepository = reportExecutionLogRepository, ExceptionMessage = "BI Report xml data not found", queueservice = queueservice, StatusCode = "F" };
                                    this.UpdateReportExecutionLog(updateReportExecutionLogArgs);
                                    LogDoneItemInMemory();
                                }
                                queueservice.Complete();
                            }
                            catch (Exception ex)
                            {
                                BIReportExecutionLogArgs updateReportExecutionLogArgs = new BIReportExecutionLogArgs() { ReportExecutionLog = reportExecutionLog, ReportExecutionLogRepository = reportExecutionLogRepository, Exception = ex, queueservice = queueservice, StatusCode = "F", response = response };
                                HandleReportExecutionException(updateReportExecutionLogArgs);
                            }
                        }
                        else
                        {
                            Thread.Sleep(new TimeSpan(0, 0, 1));
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Report Execution Log Queue worker role start", null, null);
                        Thread.Sleep(new TimeSpan(0, 0, 1));
                    }
                }
                else
                {
                    Thread.Sleep(new TimeSpan(0, 0, 1));
                }
            }
        }

        private void BuildReport(BIReportXMLData bIReportXMLData, BIReportsExecutionLog reportExecutionLog, BIReportsExecutionLogRepository reportExecutionLogRepository, DbQueueService queueservice, QueueResponse response)
        {

            try
            {
                string ObjectTableName = "Shipment";
                var data = new ExportToExcelHelper().ExportBIQueryToExcel(bIReportXMLData, tenant);

                BIReportExecutionLogArgs handleReportExecutionLogArgs = new BIReportExecutionLogArgs() { ReportExecutionLog = reportExecutionLog, ReportExecutionLogRepository = reportExecutionLogRepository, queueservice = queueservice, StatusCode = "F", response = response };
                if (data != null)
                {
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = bIReportXMLData.BIReportKey,
                        FolderName = "others",
                        Extension = "xlsx",
                        Tenant = tenant,
                        FileSize = data.Length,
                    };
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    storageservice.Write(data, fileInfo);
                    handleReportExecutionLogArgs.StatusCode = "D";
                    handleReportExecutionLogArgs.FileName = fileInfo.FileName;
                    this.UpdateReportExecutionLog(handleReportExecutionLogArgs);
                    LogDoneItemInMemory();
                }
                else
                {
                    handleReportExecutionLogArgs.Exception = new Exception("BI Report Excel file Data is null");
                    HandleReportExecutionException(handleReportExecutionLogArgs, true);
                }
            }
            catch (Exception ex)
            {
                BIReportExecutionLogArgs handleReportExecutionLogArgs = new BIReportExecutionLogArgs() { ReportExecutionLog = reportExecutionLog, ReportExecutionLogRepository = reportExecutionLogRepository, Exception = ex, queueservice = queueservice, StatusCode = "F", response = response };
                HandleReportExecutionException(handleReportExecutionLogArgs);
            }
        }

        private void HandleReportExecutionException(BIReportExecutionLogArgs updateReportExecutionLogArgs, bool isupdateReportExecutionLog = false)
        {
            if (!updateReportExecutionLogArgs.IsInternalException)
            {
                ExceptionHandler.HandleException(updateReportExecutionLogArgs.Exception, DateTime.Now, 0, null, "BI Report Execution Log Queue worker role start", null, null);
            }

            if (updateReportExecutionLogArgs.response != null && updateReportExecutionLogArgs.response.MessageValues.Keys.Contains("BIReportExecutionLogId") && !updateReportExecutionLogArgs.IsInternalException)
            {
                if (updateReportExecutionLogArgs.response.RetryNumber <= 1)
                {
                    updateReportExecutionLogArgs.queueservice.DelayAndReturnBackToQueue(new TimeSpan(0, 0, 0, 5), updateReportExecutionLogArgs.response.MessageId);
                }

                if (updateReportExecutionLogArgs.response.RetryNumber >= 2)
                {
                    queueservice.CompleteAsFailed();
                    if (isupdateReportExecutionLog) this.UpdateReportExecutionLog(updateReportExecutionLogArgs);
                }
            }
            else
            {
                queueservice.CompleteAsFailed();
                if (isupdateReportExecutionLog) this.UpdateReportExecutionLog(updateReportExecutionLogArgs);
            }
        }

        private void UpdateReportExecutionLog(BIReportExecutionLogArgs bIReportExecutionLogArgs)
        {
            if (bIReportExecutionLogArgs.ReportExecutionLog != null && bIReportExecutionLogArgs.ReportExecutionLogRepository != null)
            {
                if (bIReportExecutionLogArgs.StatusCode != "D" && (bIReportExecutionLogArgs.Exception != null || !string.IsNullOrEmpty(bIReportExecutionLogArgs.ExceptionMessage)))
                {
                    var exceptionMessage = bIReportExecutionLogArgs.ExceptionMessage;

                    if (bIReportExecutionLogArgs.Exception != null)
                    {
                        exceptionMessage = bIReportExecutionLogArgs.Exception.Message;
                        if (bIReportExecutionLogArgs.Exception.InnerException != null)
                        {
                            exceptionMessage = exceptionMessage + Environment.NewLine + bIReportExecutionLogArgs.Exception.InnerException;
                        }
                        if (bIReportExecutionLogArgs.Exception.StackTrace != null)
                        {
                            exceptionMessage = exceptionMessage + Environment.NewLine + "Stack trace: " + bIReportExecutionLogArgs.Exception.StackTrace;
                        }
                    }

                    bIReportExecutionLogArgs.ReportExecutionLog.ExceptionMessage = exceptionMessage;
                }

                if (!string.IsNullOrEmpty(bIReportExecutionLogArgs.ReportExecutionLog.ExceptionMessage) && bIReportExecutionLogArgs.ReportExecutionLog.ExceptionMessage.Length >= 4000)
                {
                    bIReportExecutionLogArgs.ReportExecutionLog.ExceptionMessage = bIReportExecutionLogArgs.ReportExecutionLog.ExceptionMessage.Substring(0, 3999);
                }

                bIReportExecutionLogArgs.ReportExecutionLog.StatusCode = bIReportExecutionLogArgs.StatusCode;
                bIReportExecutionLogArgs.ReportExecutionLog.DoneDate = DateTime.Now;
                bIReportExecutionLogArgs.ReportExecutionLogRepository.Update(bIReportExecutionLogArgs.ReportExecutionLog);
                bIReportExecutionLogArgs.ReportExecutionLogRepository.SubmitChanges();
            }
        }
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "BIReportExecutionLog";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();
            return base.OnStart();
        }
        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("BIReportsExecutionLogQueue", tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "BI Report Execution Log worker role start", null, null);
            }
        }
    }

    public class BIReportExecutionLogArgs
    {
        public Exception Exception { get; set; }
        public BIReportsExecutionLog ReportExecutionLog { get; set; }
        public BIReportsExecutionLogRepository ReportExecutionLogRepository { get; set; }
        public string StatusCode { get; set; }
        public string FileName { get; set; }
        public string ExceptionMessage { get; set; }
        public DbQueueService queueservice { get; set; }
        public bool IsInternalException { get; set; }
        public QueueResponse response { get; set; }
    }
}
