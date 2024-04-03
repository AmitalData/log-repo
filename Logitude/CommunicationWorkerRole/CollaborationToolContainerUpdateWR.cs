using Confluent.Kafka;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.KafkaConfigurations;
using Logitude.Server.Tools.Messages;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading;

namespace CommunicationWorkerRole
{
    public class CollaborationToolContainerUpdateWR : WorkerEntryPoint
    {
        public override void Run()
        {
            var LogitudeConsumer = new Consumer(KafkaConsumerGroups.UpdateContainer,
                new List<string> { KafkaTopics.ContainerSetValues }, null);

            while (IsRunning)
            {
                if (!General.IsUpdating() && !Debugger.IsAttached)
                {
                    try
                    {
                        var msg = LogitudeConsumer.Consume();
                        if (msg != null)
                        {
                            UpdateContainerPM(msg.Message.Value);
                        }
                        else
                        {
                            Thread.Sleep(5000);
                        }
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Consume error: {e.Error.Reason}");
                        ExceptionHandler.HandleException(e, DateTime.Now, 1, null, "CollaborationToolContainerUpdate worker role start", null, null);
                        //throw e;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "CollaborationToolContainerUpdate worker role start", null, null);
                        Thread.Sleep(10000);
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private void UpdateContainerPM(string jsonLogitudeUpdateMessage)
        {
            try
            {
                LogitudeUpdateMessage LogitudeUpdateMessage = JsonConvert.DeserializeObject<LogitudeUpdateMessage>(jsonLogitudeUpdateMessage);

                ContainerRepository containerRepository = new ContainerRepository(LogitudeUpdateMessage.Tenant);
                ContainerQuery containerQuery = new ContainerQuery(containerRepository);
                ContainerPM container = containerQuery.GetSinglePM(LogitudeUpdateMessage.EntityNumber, LogitudeUpdateMessage.Tenant);

                if (LogitudeUpdateMessage.EntryFields == null || LogitudeUpdateMessage.EntityNumber == null)
                {
                    return;
                }
                var entryFields = JsonConvert.DeserializeObject<Dictionary<string, string>>(LogitudeUpdateMessage.EntryFields);

                foreach (KeyValuePair<string, string> entry in entryFields)
                {
                    PropertyInfo propertyInfo = container.GetType().GetProperty(entry.Key);
                    Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                    object safeValue = (entry.Value == null) ? null : Convert.ChangeType(entry.Value, t);
                    propertyInfo.SetValue(container, safeValue, null);
                }

                ContainerService containerService = new ContainerService(containerRepository.context, container.Tenant);
                containerService.Update(container);

            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 1, null, "Container Update Error", null, null);
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CollaborationToolContainerUpdate";

            return base.OnStart();
        }
    }
}
