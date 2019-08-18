using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationWorkerRole
{
    public class LogMessagesWorkerRole : WorkerEntryPoint
    {
        ConcurrentQueueService<LogQueueMessage> queueService;
        private string queueName;

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {

                
                    try
                    {
                        queueService = new ConcurrentQueueService<LogQueueMessage>("LogMessagesQueue");
                        LogQueueMessage message = queueService.TryDequeue();
                        if (message != null)
                        {
                            IBlobService storageservice = new AzureBlobService();
                            BlobFileInfo blobFileinfo = new Logitude.Server.Tools.BlobFileInfo()
                            {
                                FileName = message.FileName,
                                FolderName = message.FolderName,
                                Tenant = message.Tenant,
                                Extension = message.FileExtension,
                            };

                            storageservice.AppendText(message.Message, blobFileinfo);
                                
                            Thread.Sleep(1000);
                        }

                    }

                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "LogMessagesWorkerRole : Run() Method", null);
                        Thread.Sleep(5000);
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "LogMessagesWorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            queueName = "LogMessagesQueue";
            ConnectClient();

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        public void ConnectClient()
        {
            try
            {
                queueService = new ConcurrentQueueService<LogQueueMessage>("LogMessagesQueue");
            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "LogMessagesWorkerRole Connect client", null, null);
            }
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
}
