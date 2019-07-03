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
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace CommunicationWorkerRole
{
    class ReportExecutionLogWorkerRole : WorkerEntryPoint
    {

        IQueueService queueservice;
        int tenant = 0;
      

        public ReportExecutionLogWorkerRole()
        {

        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ReportExecutionLog";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();
            return base.OnStart();
        }

        public override async void AsyncRun()
        {

            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        queueservice = new DbQueueService("ReportExecutionLogQueue", 0);
                        var response = queueservice.Receive(new TimeSpan(0,0,1));
                        LastActivity = DateTime.UtcNow;

                        string tenantString = null;
                    

                        if (response != null && response.MessageId != null)
                        {
                            ReportExecutionLog reportExecutionLog = null;
                            ReportExecutionLogRepository reportExecutionLogRepository = null;
          

                            try
                            {
                                string reportExecutionLogId = response.MessageValues.Keys.Contains("ReportExecutionLogId") ? response.MessageValues["ReportExecutionLogId"].ToString() : "";
                                if (response.MessageValues.Keys.Contains("Tenant"))
                                {
                                    tenantString = response.MessageValues["Tenant"].ToString();
                                    if (!string.IsNullOrEmpty(tenantString)) tenant = int.Parse(tenantString);
                                }

                                if (string.IsNullOrEmpty(reportExecutionLogId) || string.IsNullOrEmpty(tenantString))
                                {
                                    queueservice.Complete();
                                    continue;
                                }

                                reportExecutionLogRepository = new ReportExecutionLogRepository(tenant);
                                reportExecutionLog = reportExecutionLogRepository.GetSingleReportExecutionLog(reportExecutionLogId, tenant);

                                if (reportExecutionLog == null)
                                {
                                    queueservice.Complete();
                                    continue;
                                }

                                if (reportExecutionLog.StatusCode!="W")
                                {
                                    queueservice.Complete();
                                    continue;
                                }


                                ReportFliter reportFliter = null;
                                if (!string.IsNullOrEmpty(reportExecutionLog.ReportFilterXML))
                                {
                                    reportFliter = LogitudeXmlSerializer.DeserializeObject<ReportFliter>(reportExecutionLog.ReportFilterXML);
                                }

                                if (reportFliter != null)
                                {
                                    Thread thread = new Thread(() => BuildReport(reportFliter, reportExecutionLog, reportExecutionLogRepository, queueservice, response));
                                    thread.IsBackground = true;
                                    thread.Start();
                          
                                }
                                else
                                {
                                    UpdateReportExecutionLogArgs updateReportExecutionLogArgs = new UpdateReportExecutionLogArgs() { ReportExecutionLog = reportExecutionLog, ReportExecutionLogRepository = reportExecutionLogRepository,  ExceptionMessage = "Report fliter not found", queueservice = queueservice, StatusCode = "F" };
                                    this.UpdateReportExecutionLog(updateReportExecutionLogArgs);
                                    queueservice.Complete();

                                }

                                LogDoneItemInMemory();
                            }
                            catch (Exception ex)
                            {

                                UpdateReportExecutionLogArgs updateReportExecutionLogArgs = new UpdateReportExecutionLogArgs() { ReportExecutionLog = reportExecutionLog, ReportExecutionLogRepository = reportExecutionLogRepository, Exception = ex, queueservice = queueservice, StatusCode = "F",response = response};
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


        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("ReportExecutionLogQueue", tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Report Execution Log worker role start", null, null);
            }
        }

        private void BuildReport(ReportFliter reportFliter, ReportExecutionLog reportExecutionLog, ReportExecutionLogRepository reportExecutionLogRepository, IQueueService queueservice , QueueResponse response)
        {

            try
            {
                ReportHelper reportHelper = new ReportHelper();
                BuildReportDataResult buildReportDataResult = reportHelper.BuildReport(reportFliter);
                UpdateReportExecutionLogArgs handleReportExecutionLogArgs = new UpdateReportExecutionLogArgs() { ReportExecutionLog = reportExecutionLog, ReportExecutionLogRepository = reportExecutionLogRepository, Exception = buildReportDataResult.Exception, queueservice = queueservice, StatusCode = "F" , response = response ,IsInternalException = buildReportDataResult.IsInternalException };
                
                if (buildReportDataResult.Exception == null)
                {
                    handleReportExecutionLogArgs.StatusCode = "D";
                    this.UpdateReportExecutionLog(handleReportExecutionLogArgs);
                    queueservice.Complete();
                }
                else
                {
                    HandleReportExecutionException(handleReportExecutionLogArgs , true);
                }

            }
            catch (Exception ex)
            {

                UpdateReportExecutionLogArgs handleReportExecutionLogArgs = new UpdateReportExecutionLogArgs() { ReportExecutionLog = reportExecutionLog, ReportExecutionLogRepository = reportExecutionLogRepository, Exception = ex, queueservice = queueservice, StatusCode = "F" , response = response };
                HandleReportExecutionException(handleReportExecutionLogArgs);

            }
        }


        private void HandleReportExecutionException(UpdateReportExecutionLogArgs updateReportExecutionLogArgs, bool isupdateReportExecutionLog = false)
        {
            if (!updateReportExecutionLogArgs.IsInternalException)
            {
                ExceptionHandler.HandleException(updateReportExecutionLogArgs.Exception, DateTime.Now, 0, null, "Report Execution Log Queue worker role start", null, null);
            }


            if (updateReportExecutionLogArgs.ReportExecutionLog != null && updateReportExecutionLogArgs.ReportExecutionLogRepository != null)
            {
                if (updateReportExecutionLogArgs.response != null && updateReportExecutionLogArgs.response.MessageValues.Keys.Contains("ReportExecutionLogId") && !updateReportExecutionLogArgs.IsInternalException)
                {
                    if (updateReportExecutionLogArgs.response.RetryNumber <= 1)
                    {
                        updateReportExecutionLogArgs.queueservice.Delay(new TimeSpan(0, 0, 0, 5));
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

         
         
        }



        private void UpdateReportExecutionLog(UpdateReportExecutionLogArgs updateReportExecutionLogArgs)
        {
            if (updateReportExecutionLogArgs.ReportExecutionLog != null && updateReportExecutionLogArgs.ReportExecutionLogRepository != null)
            {
                if (updateReportExecutionLogArgs.StatusCode != "D" && (updateReportExecutionLogArgs.Exception != null || !string.IsNullOrEmpty(updateReportExecutionLogArgs.ExceptionMessage)))
                {
                    var exceptionMessage = updateReportExecutionLogArgs.ExceptionMessage;

                    if (updateReportExecutionLogArgs.Exception != null)
                    {
                        exceptionMessage = updateReportExecutionLogArgs.Exception.Message;
                        if (updateReportExecutionLogArgs.Exception.InnerException != null)
                        {
                            exceptionMessage = exceptionMessage + Environment.NewLine + updateReportExecutionLogArgs.Exception.InnerException;
                        }
                        if (updateReportExecutionLogArgs.Exception.StackTrace != null)
                        {
                            exceptionMessage = exceptionMessage + Environment.NewLine + "Stack trace: " + updateReportExecutionLogArgs.Exception.StackTrace;
                        }
                    }


                    updateReportExecutionLogArgs.ReportExecutionLog.ExceptionMessage = exceptionMessage;
                }

                if (!string.IsNullOrEmpty(updateReportExecutionLogArgs.ReportExecutionLog.ExceptionMessage) && updateReportExecutionLogArgs.ReportExecutionLog.ExceptionMessage.Length >= 4000)
                {
                    updateReportExecutionLogArgs.ReportExecutionLog.ExceptionMessage = updateReportExecutionLogArgs.ReportExecutionLog.ExceptionMessage.Substring(0, 3999);

                }

                updateReportExecutionLogArgs.ReportExecutionLog.StatusCode = updateReportExecutionLogArgs.StatusCode;
                updateReportExecutionLogArgs.ReportExecutionLog.DoneDate = DateTime.Now;
                updateReportExecutionLogArgs.ReportExecutionLogRepository.Update(updateReportExecutionLogArgs.ReportExecutionLog);
                updateReportExecutionLogArgs.ReportExecutionLogRepository.SubmitChanges();
            }
        }


    }

    public class UpdateReportExecutionLogArgs
    {
        public Exception Exception { get; set; }
        public ReportExecutionLog ReportExecutionLog { get; set; }
        public ReportExecutionLogRepository ReportExecutionLogRepository { get; set; }
        public string StatusCode { get; set; }
        public string ExceptionMessage { get; set; }
        public IQueueService queueservice { get; set; }
        public bool IsInternalException { get; set; }
        public QueueResponse response { get; set; }
        

    }
}
