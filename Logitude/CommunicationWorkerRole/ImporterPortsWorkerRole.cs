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
            _startDate = DateTime.Now;
            _importerPortService = new();
        }
        public override void Run()
        {
            _startDate = DateTime.Now;
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    _queueService = new DbQueueService();
                    _queueService.InitializeQueue(_queueCode, 0);
                    var response = _queueService.Receive(new TimeSpan(0, 0, 10));
                    LastActivity = DateTime.UtcNow;
                    int Tenant = 0;
                    if (response != null && response.MessageId != null)
                    {
                        try
                        {
                            ExecuteQueue(response);
                            _queueService.Complete();
                        }
                        catch (Exception ex)
                        {
                            #region HandleException
                            ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "ImporterPortsWorkerRole", "", null);
                            if (response.RetryNumber <= 1)
                            {
                                _queueService.Delay(new TimeSpan(0, 0, 0, 5));
                            }

                            if (response.RetryNumber > 1 && response.RetryNumber <= 2)
                            {
                                _queueService.Delay(new TimeSpan(0, 0, 0, 10));
                            }
                            if (response.RetryNumber >= 3)
                            {
                                _queueService.CompleteAsFailed();
                            }
                            #endregion
                        }
                    }
                    //TimeSpan span = (DateTime.Now).Subtract(_startDate);
                    //if (span.Minutes >= 10)
                    //{
                    //    _queueService.InitializeQueue(_queueCode, 0);
                    //    _queueService.Send(new Dictionary<string, string>() { { "Tenant", "0" } }, Tenant, null, null);
                    //    _startDate = DateTime.Now;
                    //}
                }
                else Thread.Sleep(60000);
            }
        }

        private void ExecuteQueue(QueueResponse response)
        {
            int tenant = int.Parse(response.MessageValues["Tenant"].ToString());
            string portId = response.MessageValues["PortId"].ToString();
            importerPortService.Run(portId , tenant);

        }
        private bool ValidateQueue(QueueResponse response)
        {
            string tenant = response.MessageValues["Tenant"].ToString();
            string portId = response.MessageValues["PortId"].ToString();
            if (string.IsNullOrEmpty(portId))
                return false;
            if (string.IsNullOrEmpty(tenant))
                return false;
            return true;
        }
    }
}
