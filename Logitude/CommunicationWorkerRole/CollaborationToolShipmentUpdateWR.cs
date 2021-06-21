using Confluent.Kafka;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.KafkaConfigurations;
using Logitude.Server.Tools.Messages;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading;

namespace CommunicationWorkerRole
{
    public class CollaborationToolShipmentUpdateWR : WorkerEntryPoint
    {
        public override void Run()
        {
            var LogitudeConsumer = new Consumer(KafkaConsumerGroups.UpdateTask, new List<string> { KafkaTopics.TasksTopic },
                                    new TopicPartition(KafkaTopics.TasksTopic, KakaPartitions.TasksTopic_UpdatePartition));

            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        var msg = LogitudeConsumer.Consume();
                        if (msg != null && msg.Message.Key == KakaMessageTypes.Task)
                        {
                            UpdateShipmentPM(msg.Message.Value);
                        }
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Consume error: {e.Error.Reason}");
                        ExceptionHandler.HandleException(e, DateTime.Now, 1, null, "CollaborationToolShipmentUpdate worker role start", null, null);
                        //throw e;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "CollaborationToolShipmentUpdate worker role start", null, null);
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
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CollaborationToolShipmentUpdate";

            return base.OnStart();
        }

        #region Private methods
        private void UpdateShipment(string jsonLogitudeUpdateMessage)
        {
            try
            {
                LogitudeUpdateMessage LogitudeUpdateMessage = JsonConvert.DeserializeObject<LogitudeUpdateMessage>(jsonLogitudeUpdateMessage);

                ShipmentRepository shipmentRepository = new ShipmentRepository(LogitudeUpdateMessage.Tenant);
                Shipment shipment = shipmentRepository.GetSingleShipmentByShipmentNumber(LogitudeUpdateMessage.EntityNumber, LogitudeUpdateMessage.Tenant);

                var entryFields = JsonConvert.DeserializeObject<Dictionary<string, string>>(LogitudeUpdateMessage.EntryFields);

                foreach (KeyValuePair<string, string> entry in entryFields)
                {
                    PropertyInfo propertyInfo = shipment.GetType().GetProperty(entry.Key);
                    Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                    object safeValue = (entry.Value == null) ? null : Convert.ChangeType(entry.Value, t);
                    propertyInfo.SetValue(shipment, safeValue, null);
                }

                shipmentRepository.Update(shipment);
                shipmentRepository.SubmitChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void UpdateShipmentPM(string jsonLogitudeUpdateMessage)
        {
            try
            {
                LogitudeUpdateMessage LogitudeUpdateMessage = JsonConvert.DeserializeObject<LogitudeUpdateMessage>(jsonLogitudeUpdateMessage);

                ShipmentRepository shipmentRepository = new ShipmentRepository(LogitudeUpdateMessage.Tenant);
                ShipmentQuery myQuery = new ShipmentQuery(shipmentRepository);
                ShipmentPM shipment = myQuery.GetSinglePMByShipmentNumber(LogitudeUpdateMessage.EntityNumber, LogitudeUpdateMessage.Tenant);

                if (LogitudeUpdateMessage.EntryFields == null || LogitudeUpdateMessage.EntityNumber == null)
                {
                    return;
                }
                var entryFields = JsonConvert.DeserializeObject<Dictionary<string, string>>(LogitudeUpdateMessage.EntryFields);

                foreach (KeyValuePair<string, string> entry in entryFields)
                {
                    PropertyInfo propertyInfo = shipment.GetType().GetProperty(entry.Key);
                    Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                    object safeValue = (entry.Value == null) ? null : Convert.ChangeType(entry.Value, t);
                    propertyInfo.SetValue(shipment, safeValue, null);
                }

                ShipmentService myService = new ShipmentService(shipmentRepository.context, shipment, "system@tenant" + shipment.Tenant + ".com");
                myService.Update();

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }

    public class LogitudeUpdateMessage
    {
        public int Tenant { get; set; }
        public string EntityNumber { get; set; }
        public string EntryFields { get; set; }
    }
}
