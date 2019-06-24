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
                                    Thread thread = new Thread(() => BuildReport(reportFliter, reportExecutionLog, reportExecutionLogRepository, queueservice));
                                    thread.IsBackground = true;
                                    thread.Start();
                          
                                }
                                else
                                {
                                    this.UpdateReportExecutionLog(null, reportExecutionLog, reportExecutionLogRepository, "F" , "Report fliter not found");
                                    queueservice.Complete();
                                }


                             

                                LogDoneItemInMemory();
                            }
                            catch (Exception ex)
                            {
                                #region HandleException
                                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Report Execution Log Queue worker role start", null, null);
                                this.UpdateReportExecutionLog(ex, reportExecutionLog, reportExecutionLogRepository, "F");
                                queueservice.CompleteAsFailed();

                                #endregion
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

        private void BuildReport(ReportFliter reportFliter, ReportExecutionLog reportExecutionLog, ReportExecutionLogRepository reportExecutionLogRepository, IQueueService queueservice)
        {

            try
            {
                ReportHelper reportHelper = new ReportHelper();
                reportHelper.BuildReport(reportFliter);
                this.UpdateReportExecutionLog(null, reportExecutionLog, reportExecutionLogRepository, "D");
                queueservice.Complete();
            }
            catch (Exception ex)
            {
                if (reportExecutionLog != null && reportExecutionLogRepository != null)
                {
                    this.UpdateReportExecutionLog(ex, reportExecutionLog, reportExecutionLogRepository, "F");
                }

                queueservice.CompleteAsFailed();
            }
        }


        private void UpdateReportExecutionLog(Exception ex , ReportExecutionLog reportExecutionLog, ReportExecutionLogRepository reportExecutionLogRepository, string statusCode , string exception = null)
        {
            if (reportExecutionLog != null && reportExecutionLogRepository!=null)
            {
                if (statusCode != "D" && (ex!=null || !string.IsNullOrEmpty(exception))) {
                    var exceptionMessage = exception;

                    if (ex != null)
                    {
                        exceptionMessage = ex.Message;
                        if (ex.InnerException != null)
                        {
                            exceptionMessage = exceptionMessage + Environment.NewLine + ex.InnerException;
                        }
                        if (ex.StackTrace != null)
                        {
                            exceptionMessage = exceptionMessage + Environment.NewLine + "Stack trace: " + ex.StackTrace;
                        }
                    }


                    reportExecutionLog.ExceptionMessage = exceptionMessage;
                }

                reportExecutionLog.StatusCode = statusCode;
                reportExecutionLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                reportExecutionLogRepository.Update(reportExecutionLog);
                reportExecutionLogRepository.SubmitChanges();
            }
        }


    }
}
