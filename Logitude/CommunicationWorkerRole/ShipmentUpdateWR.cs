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
    class ShipmentUpdateWR : WorkerEntryPoint
    {
        DbQueueService queueservice;
        string queueName = "CToolShipmentsUpdate";

        public override void Run()
        {
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
                            ShipmentPM entityPM = GetShipmentById(response);

                            var ShipmentUpdateMessageProducer = new Producer();
                            var serializedShipmentUpdateMessage = JsonConvert.SerializeObject(entityPM, Formatting.Indented);
                            if (entityPM.UpdatedByUserName == "System")
                            {
                                var result = ShipmentUpdateMessageProducer.Produce(KafkaTopics.ShipmentsTrackUpdateTopic, KakaMessageTypes.ShipmentUpdate, serializedShipmentUpdateMessage);
                            }
                            else
                            {
                                var result = ShipmentUpdateMessageProducer.Produce(KafkaTopics.ShipmentsUpdateTopic, KakaMessageTypes.ShipmentUpdate, serializedShipmentUpdateMessage);
                            } 
                            queueservice.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        queueservice.CompleteAsFailed();
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "CToolShipmentsUpdate worker role start", null, null);
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
            BatchServiceCode = "CToolShipmentsUpdate";

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



        #region Private Methods
        private ShipmentPM GetShipmentById(QueueResponse response)
        {
            int Tenant = int.Parse(response.MessageValues["Tenant"].ToString());
            string ShipmentId = response.MessageValues["ShipmentId"].ToString();

            ShipmentQuery shipmentQuery = new ShipmentQuery(Tenant);
            ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(ShipmentId, Tenant);
            return shipmentPM;
        }
        #endregion
    }
}
