using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.KafkaConfigurations;
using Logitude.Server.Tools.Messages;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;

namespace CommunicationWorkerRole
{
    public class ContainerCreateWR : WorkerEntryPoint
    {
        DbQueueService queueservice;
        string queueName = "CToolContainerCreate";

        public override void Run()
        {
            var producer = new Producer();

            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        queueservice = new DbQueueService(queueName, 0);
                        var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));

                        if (response.MessageId != null)
                        {
                            ContainerPM containerPM = GetContainerById(response);

                            var JsonContainerPM = JsonConvert.SerializeObject(containerPM, Formatting.Indented);
                            var result = producer.Produce(KafkaTopics.ContainerCreateTopic, KakaMessageTypes.ContainerCreate, JsonContainerPM);
                            queueservice.Complete();
                        }
                        else
                        {
                            Thread.Sleep(5000);
                        }
                    }
                    catch (Exception ex)
                    {
                        queueservice.CompleteAsFailed();
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "CToolContainerCreate worker role start", null, null);
                        Thread.Sleep(10000);
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
            ConnectClient();

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CToolContainerCreate";

            return base.OnStart();
        }

        public void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService(queueName, 0);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "Connect client method", null, null);
            }
        }

        private ContainerPM GetContainerById(QueueResponse response)
        {
            int Tenant = int.Parse(response.MessageValues["Tenant"].ToString());
            string ContainerId = response.MessageValues["ContainerId"].ToString();

            ContainerQuery containerQuery = new ContainerQuery(Tenant);
            ContainerPM containerPM = containerQuery.GetSinglePM(ContainerId, Tenant);
            return containerPM;
        }
    }
}
