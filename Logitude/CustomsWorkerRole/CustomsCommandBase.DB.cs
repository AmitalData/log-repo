//using Microsoft.WindowsAzure.ServiceRuntime;
using Logitude.Customs.BL.Messaging.Customs.PerformanceLogger;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace CustomsWorkerRole
{
    public partial class CustomsCommandBase
    {
#if false
        void WorkUntilQEmpty_Db()
        {



            CustomDBQueueMessage response = null;
            List<long> deferredSequenceNumbers = new List<long>();


            while (!WorkerRoleServiceLocator.PleaseShutDown)
            {
                LogMessagingUtilWR.Instance.Clear();
                LogMessagingUtil.Instance.Clear();

                DateTime QueueStartDate = DateTime.Now;

                try
                {
                    bool successProcessMessage = false;


                    LogMessagingUtilWR.Instance.AppendLine("QRecive");
                    response = _CustomDbQueueService.Receive(nextRunDelayInSec: CustomsWorkerRole.Utils.GenUtil.GetQueueTimeOutInMin() * 60);//is own transaction !!!!
                    LogMessagingUtilWR.Instance.AppendLine("QRecive:after");
                    if (response == null || (response != null && response.MessageId == null))
                    {
                        QueueThreadStateService.Upsert(QueueThreadStateService.GetWRKey(this.GetType().Name), "No Work");
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                        //Thread.Sleep(TimeSpan.FromMilliseconds(300));
                        break;
                    }


                    try
                    {

                        var cancelToken = new CancellationTokenSource(TimeSpan.FromMinutes(19));
                        Task.Factory.StartNew(async () => {
                            successProcessMessage = DoWork(response, QueueStartDate);
                        }, cancelToken.Token);
                        

                    }
                    finally
                    {
                        if (successProcessMessage)
                        {
                            LogMessagingUtilWR.Instance.AppendLine("_CustomDbQueueService.SafeComplete();");
                            _CustomDbQueueService.SafeComplete();
                            PerformanceM.LastInstance.QueueSuccessComplete = true;
                        }
                        else if (!successProcessMessage)/// IF FAILED USE NEW TRANS !!!!
                        {


                            LogMessagingUtilWR.Instance.AppendLine("_CustomDbQueueService.SafeComplete();");
                            _CustomDbQueueService.SafeAbandon();
                            
                        }

                    }




                    LogMessagingUtilWR.Instance.AppendLine("LogDoneItemInMemory();");
                    LogDoneItemInMemory();
                    LogMessagingUtilWR.Instance.AppendLine("LogDoneItemInMemory();AFTER");
                    PerformanceM.LastInstance.QueueEndDate = DateTime.Now;
                    PerformanceM.EnqueueLastInstance();

                    string logItMessagingUtilWR = ConfigurationManager.AppSettings.Get("LogMessagingUtilWR");

                    if (!string.IsNullOrWhiteSpace(logItMessagingUtilWR))
                    {
                        string morethan = "";
                        string str = LogMessagingUtilWR.Instance.GetString(out morethan);
                        Logger.LogMe(str, false, this.GetType().ToString() + "_" + morethan);
                    }

                }
                finally
                {
                    PerformanceM.SleepMSAfterEachQueuePeek();
                }
            }
        }

        private bool DoWork(CustomDBQueueMessage response, DateTime QueueStartDate)
        {
            bool successProcessMessage;
            LogMessagingUtilWR.Instance.Clear();
            LogMessagingUtil.Instance.Clear();

            LogMessagingUtilWR.Instance.AppendLine("TransactionFactory.GetTransaction");
            using (TransactionScope Queue_scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(19)))
            {




                PerformanceM.EnqueueLastInstance();
                PerformanceM.LastInstance.QueueStartDate = QueueStartDate;
                PerformanceM.LastInstance.QueueReceiveDate = DateTime.Now;

                LastActivity = DateTime.UtcNow;

                LogMessagingUtilWR.Instance.AppendLine("ProcessMessage_Db");

                successProcessMessage = ProcessMessage_Db(response);
                LogMessagingUtilWR.Instance.AppendLine($"successProcessMessage  {successProcessMessage}");

            }

            return successProcessMessage;
        }


#endif
    }


}


