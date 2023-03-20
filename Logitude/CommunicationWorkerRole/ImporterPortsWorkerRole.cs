using CommunicationWorkerRole.Services.Logbox;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationWorkerRole
{
    class ImporterPortsWorkerRole : WorkerEntryPoint
    {
        private DbQueueService queueService;
        private DateTime startDate;
        private ImporterPortService importerPortService;
        private const string queueCode = "ImporterPortsQueue";
        public ImporterPortsWorkerRole()
        {
            startDate = DateTime.Now;
        }
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ImporterPortsWorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            return base.OnStart();
        }
        public override void Run()
        {
            startDate = DateTime.Now;
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    queueService = new DbQueueService();
                    queueService.InitializeQueue(queueCode, 0);
                    var response = queueService.Receive(new TimeSpan(0, 0, 10));
                    LastActivity = DateTime.UtcNow;
                    if (response != null && response.MessageId != null)
                    {
                        try
                        {
                            ExecuteQueue(response);
                            queueService.Complete();
                        }
                        catch (Exception ex)
                        {
                            #region HandleException
                            ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "ImporterPortsWorkerRole", "", null);
                            if (response.RetryNumber <= 1)
                            {
                                queueService.Delay(new TimeSpan(0, 0, 0, 5));
                            }

                            if (response.RetryNumber > 1 && response.RetryNumber <= 2)
                            {
                                queueService.Delay(new TimeSpan(0, 0, 0, 10));
                            }
                            if (response.RetryNumber >= 3)
                            {
                                queueService.CompleteAsFailed();
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        Thread.Sleep(10000);
                    }
                }
                else Thread.Sleep(60000);
            }
        }

        private void ExecuteQueue(QueueResponse response)
        {
            int tenant = int.Parse(response.MessageValues["Tenant"].ToString());
            string portId = response.MessageValues["PortId"].ToString();
            importerPortService = new ImporterPortService(response);
            importerPortService.Run(portId , tenant);
        }
    }
}
