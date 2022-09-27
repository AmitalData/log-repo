using CommunicationWorkerRole.Services;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CommunicationWorkerRole
{
    public class DigitalPortalQueryExportExecutionLogWR : WorkerEntryPoint
    {
        DbQueueService queueservice;

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DigitalPortalQueryExportExecutionLogWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();
            return base.OnStart();
        }

        private void ExecuteQueue()
        {
            queueservice = new DbQueueService("DigitalPortalQueryExportExecutionQueue", 0);
            var queueResponse = queueservice.Receive(new TimeSpan(0, 0, 1));
            
            if (queueResponse != null && queueResponse.MessageId != null)
            {
                ThreadStart reportExecutionServiceThreadStart = (() =>
                     new DigitalQueryQueryExportLogExecutionService(queueservice, queueResponse)
                        .ExecuteQueryExportExecutionLog());

                reportExecutionServiceThreadStart += () => { LogDoneItemInMemory(); };
                new Thread(reportExecutionServiceThreadStart) { IsBackground = true }.Start();
                queueservice.Complete();
            }
            else
            {
                Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        ExecuteQueue();
                    }
                    catch (Exception exception)
                    {
                        ExceptionHandler.HandleException(exception,
                                                         DateTime.Now,
                                                         0,
                                                         null,
                                                         "Query Export execution log queue worker role start",
                                                         null,
                                                         null);
                        Thread.Sleep(new TimeSpan(0, 0, 1));
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("DigitalPortalQueryExportExecutionQueue", 0);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex,
                                                 DateTime.Now,
                                                 0,
                                                 null,
                                                 "Query Export Execution Log worker role start",
                                                 null,
                                                 null);
            }
        }
    }
}