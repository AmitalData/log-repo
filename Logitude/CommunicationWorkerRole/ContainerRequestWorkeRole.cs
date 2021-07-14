using CommunicationWorkerRole.Analyzers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace CommunicationWorkerRole
{
    public class ContainerRequestWorkeRole : WorkerEntryPoint
    {

        private int tenant;
        IQueueService queueservice;
        CommunicationLog communicationLog;
        QueueResponse queueResponse;
        string communicationLogId;
        CommunicationLogRepository communicationLogRepository;
        ICommonDataContext context;
        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        queueservice = new DbQueueService();
                        queueservice.InitializeQueue("ContainerStatusesCommunicationLogQueue", 0);
                        queueResponse = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
                        if (queueResponse.MessageId != null)
                        {
                            communicationLogId = queueResponse.MessageValues["CommunicationLogId"].ToString();
                            int.TryParse(queueResponse.MessageValues["Tenant"].ToString(), out tenant);
                            this.SetCommunicationLog();
                            if (communicationLog != null)
                            {
                                if (communicationLog.CommunicationStatusTypeCode == "D")
                                {
                                    queueservice.Complete();
                                }
                                else
                                {
                                    try
                                    {
                                        ContainerRequestSender analyzer = new ContainerRequestSender(communicationLog, context, communicationLogRepository, tenant);
                                        analyzer.Send();
                                        queueservice.Complete();
                                        LogDoneItemInMemory();
                                    }
                                    catch (Exception exc)
                                    {
                                        this.HandelCommunicationLogErrorException(exc);
                                    }
                                }
                            }
                            else
                            {
                                this.HandelQueueResponseRetries();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "ContainerStatusesMonitorWorkerRole : Run() Method", null);
                        //queueservice.CompleteAsFailed();
                        Thread.Sleep(5000);
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }
        private void SetCommunicationLog()
        {
            context = CommonDataContext.GetContext(tenant);
            communicationLogRepository = new CommunicationLogRepository(context);
            communicationLog = communicationLogRepository.GetSingleCommunicationLog(communicationLogId, tenant);
        }
        private static string GetExceptionMessage(Exception exc)
        {
            string exceptionMessage = exc.Message;
            if (exc.InnerException != null)
            {
                exceptionMessage = exceptionMessage + Environment.NewLine + exc.InnerException;
            }
            if (exc.StackTrace != null)
            {
                exceptionMessage = exceptionMessage + Environment.NewLine + "Stack trace: " + exc.StackTrace;
            }
            exceptionMessage = StringHelper.TruncateLongString(exceptionMessage, 4000);

            return exceptionMessage;
        }
        private void HandelCommunicationLogErrorException(Exception exc)
        {
            communicationLog.Retries++;
            communicationLog.ExceptionMessage = GetExceptionMessage(exc);
            if (queueResponse.RetryNumber <= 1)
            {
                queueservice.Delay(new TimeSpan(0, 0, 0, 5));
            }

            if (queueResponse.RetryNumber > 1 && queueResponse.RetryNumber <= 2)
            {
                queueservice.Delay(new TimeSpan(0, 0, 0, 10));
            }
            if (queueResponse.RetryNumber >= 3)
            {
                queueservice.CompleteAsFailed();
                communicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(communicationLog.Tenant);
                communicationLog.DoneDateUTC = DateTime.UtcNow;
                communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(communicationLog.Tenant);
                communicationLog.LastStatusDateUTC = DateTime.UtcNow;
                communicationLog.CommunicationStatusTypeCode = "F";
            }
            communicationLogRepository.Update(communicationLog);
            communicationLogRepository.SubmitChanges();
        }

        private void HandelQueueResponseRetries()
        {
            if (queueResponse.RetryNumber <= 10)
            {
                if (queueResponse.RetryNumber < 3)
                {
                    queueservice.Delay(new TimeSpan(0, 0, 0, 1));
                }
                if (queueResponse.RetryNumber >= 3 && queueResponse.RetryNumber <= 5)
                {
                    queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                }
                if (queueResponse.RetryNumber > 5 && queueResponse.RetryNumber <= 10)
                {
                    queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                    AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + queueResponse.RetryNumber
                        + ",at utc time:" + DateTime.UtcNow + ",at ContainerRequestWorkeRole worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                    Thread.Sleep(3000);
                }
                if (queueResponse.RetryNumber == 11)
                {

                    queueservice.Delay(new TimeSpan(0, 0, 2, 0));
                    AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + queueResponse.RetryNumber
                    + ",at utc time:" + DateTime.UtcNow + ",at ContainerRequestWorkeRole worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                    Thread.Sleep(10000);
                }
            }
            else
            {
                queueservice.Complete();
                AzureLog.SaveLogsInStorage("couldn't find communication log and the message is completed: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + queueResponse.RetryNumber
                    + ",at utc time:" + DateTime.UtcNow + ",at ContainerRequestWorkeRole worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ContainerRequestWorkeRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                e.Cancel = true;
            }
        }
    }
}
