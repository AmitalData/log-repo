using Confluent.Kafka;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.KafkaConfigurations;
using Logitude.Server.Tools.Messages;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading;

namespace CommunicationWorkerRole
{
    public class CollaborationToolShipmentUpdateWR : WorkerEntryPoint
    {
        public override void Run()
        {
            var LogitudeConsumer = new Consumer(KafkaConsumerGroups.UpdateShipment,
                    new List<string> { KafkaTopics.TasksDoneTopic, KafkaTopics.ShipmentSetValues }, null);

            while (IsRunning)
            {
                 
                if (!General.IsUpdating() && !Debugger.IsAttached)
                {
                    try
                    {  
                        var msg = LogitudeConsumer.Consume();
                        if (msg != null)
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

                    var fieldTypeName = GetTypeName(shipment, entry.Key);
                    if (fieldTypeName == "CustomFieldClass")
                    {
                        SetShipmentCustomField(shipment, entry);
                    }
                    else
                    {
                        SetValue(shipment, entry.Key, entry.Value);
                    }
                }

                ShipmentService myService = new ShipmentService(shipmentRepository.context, shipment, "system@tenant" + shipment.Tenant + ".com");
                myService.Update();

            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 1, null, "Shipment Update Error", null, null);
            }
        }

        private string GetTypeName(object model, string field)
        {
            PropertyInfo propertyInfo = model.GetType().GetProperty(field);
            Type type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            return type.Name;
        }

        private void SetShipmentCustomField(ShipmentPM shipment, KeyValuePair<string, string> shipmentField)
        {
            CustomFieldClass customFieldValue = new()
            {
                FieldName = shipmentField.Key,
                TableName = "Shipment",
                Value = shipmentField.Value
            };

            SetValue(shipment, shipmentField.Key, customFieldValue);
        }

        private void SetValue(object model, string field, object value)
        {
            PropertyInfo propertyInfo = model.GetType().GetProperty(field);
            propertyInfo.SetValue(model, value, null);
        }

        private void SetValue(object model, string field, string value)
        {
            PropertyInfo propertyInfo = model.GetType().GetProperty(field);
            Type type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            object safeValue = (string.IsNullOrEmpty(value)) ? null : Convert.ChangeType(value, type);
            propertyInfo.SetValue(model, safeValue, null);
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
