using System;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;

using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;

using System.Net.Mail;
using System.Text.RegularExpressions;

using Simplog.Server.Infrastructure.Azure;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using Microsoft.WindowsAzure.Storage.Queue;

using Logitude.SystemLogs;
using WebFreight.Web.Azure.AzureCommunicationLog;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;

namespace CommunicationWorkerRole
{
    class CommunicationLogWorkerRole : WorkerEntryPoint
    {
        CloudStorageAccount storageAccount;
        CloudQueueClient queueclient;
        CloudQueue queue;
        private bool _OnStartDone;

        public override void Run()
        {
            while (IsRunning)
            {
                WorkOnce();
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        public  void WorkOnce()
        {
            OnStart();
            if (General.IsUpdating())
            {
                Thread.Sleep(60000);
                return;
            }
            if (!queue.Exists())
            {
                return;
            }
            try
            {
                var msg = queue.GetMessage();
                LastActivity = DateTime.UtcNow;
                    //queue.Receive(TimeSpan.FromSeconds(5));
                if (msg == null)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    return;
                }

                string[] msgData = msg.AsString.Split(',');


                //CommunicationLogsAzureDomainService commLogService = new CommunicationLogsAzureDomainService();

                //CommunicationLogsAzure communicationLogsAzure = commLogService.GetSingleCommunicationLogsAzure(msgData[0], Int32.Parse(msgData[1]));
                int tenant = Int32.Parse(msgData[1]);

                CommunicationLogsAzureDomainService communicationLogsDomainService = new CommunicationLogsAzureDomainService();
                CommunicationLogsAzure communicationLogsAzure = communicationLogsDomainService.GetSingleCommunicationLogsAzure(msgData[0], tenant);

                //context.CommunicationLogsAzureEntity.Where(c => c.Id == msgData[0] && c.Tenant == tenant).FirstOrDefault();
                //SendEmail(communicationLogsAzure);

                //EmailParameters parameters = new EmailParameters()
                //{
                //    From = communicationLogsAzure.From,
                //    To = communicationLogsAzure.To,
                //    Cc = communicationLogsAzure.CC,
                //    Bcc = communicationLogsAzure.BCC,
                //    //Attachments = attachements,
                //    EmailView = System.Net.Mime.MediaTypeNames.Text.Html,
                //    IsBodyHtml = communicationLogsAzure.IsBodyHtml,
                //    Tenant = communicationLogsAzure.Tenant,
                //    Body = communicationLogsAzure.BodyDocumentId,
                //    //SentByUser = communicationLogsAzure.CreatedByUserName,
                //    Subject = communicationLogsAzure.Subject,
                    

                //};

                EmailCommunicationParams emailParams = new EmailCommunicationParams()
                {
                    Subject = communicationLogsAzure.Subject,
                    From = communicationLogsAzure.From,
                    To = communicationLogsAzure.To,
                    CC = communicationLogsAzure.CC,
                    BCC = communicationLogsAzure.BCC,
                    EmailBody = communicationLogsAzure.BodyDocumentId,
                    Tenant = communicationLogsAzure.Tenant,
                    
                };
                Communications.AddEmailCommunicationLogQueue(emailParams, communicationLogsAzure.Tenant);
                //EmailingHelper.SendEmail(parameters);

                queue.DeleteMessage(msg);

                communicationLogsAzure.StatusCode = "D";
                communicationLogsDomainService.UpdateMyChildEntity(communicationLogsAzure);
                //communicationLogsDomainService.SaveChanges();
                LogDoneItemInMemory();
            }

            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "CommunicationLogWorkerRole : Run() Method", null);
                Thread.Sleep(10000);
            }

        }

        public override bool OnStart()
        {
            if (_OnStartDone) return true;
            _OnStartDone = true;
            storageAccount = StorageAcountDetails.StorageAccount; //CloudStorageAccount.FromConfigurationSetting("DiagnosticsConnectionString");
            queueclient = storageAccount.CreateCloudQueueClient();
            queue = queueclient.GetQueueReference("communicationlogqueue");
            queue.CreateIfNotExists();
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;


            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CommunicationLog";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");
            if (!LogitudeSettings.IsCostomsDeploy) //no window azure !!!
            {

                // For information on handling configuration changes
                // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
                RoleEnvironment.Changing += RoleEnvironmentChanging;
            }
            
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



    }
    public class CommunicationLogWorkerRoleWinService : Logitude.Server.Tools.WorkerEntryPointDoneLog
    {
        CommunicationLogWorkerRole _CommunicationLogWorkerRole;
        public CommunicationLogWorkerRoleWinService()
        {
            _CommunicationLogWorkerRole = new CommunicationLogWorkerRole();
        }
        public override void StartMe()
        {
            throw new NotImplementedException();
        }

        public override void WorkOnce()
        {
            _CommunicationLogWorkerRole.WorkOnce();
        }
    }
}
