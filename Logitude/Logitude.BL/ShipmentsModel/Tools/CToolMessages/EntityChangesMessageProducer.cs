using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.CToolWorkflows.Models;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.KafkaConfigurations;
using Logitude.Server.Tools.Messages;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using static Confluent.Kafka.ConfigPropertyNames;
using Producer = Logitude.Server.Tools.Messages.Producer;

namespace Logitude.Server.Tools.CToolWorkflows
{
    public static class EntityChangesMessageProducer
    {
        public static ConcurrentQueueService<Producer> producerQueueService = new ConcurrentQueueService<Producer>("kafkaproducer");

        private static Producer producerInstatnce;

        public static Producer GetInstatnce(int tenant)
        {
            var instance = producerQueueService.TryDequeue();
            if (instance != null)
            {
                return instance;
            }
            return new Producer();
        }

        public static void AddProducerToQueue(Producer producer)
        {
            producerQueueService.Enqueue(producer);
        }

        private static bool UseSyncDisposableProducer(int tenant)
        {
            return FeatureToggleHelper.HasFeatureToggle("SKP", tenant);
        }
        #region Shipment
        public static void ProduceShipmentCreateMessage(Shipment entityPoco, ShipmentPM entityPM)
        {
            try
            {
                if (entityPM != null && FeatureToggleHelper.HasFeatureToggle("CTL", entityPM.Tenant))
                {
                    CToolWorkflowMessage ctoolWorkflowMessage = new CToolWorkflowMessage()
                    {
                        Entity = entityPM,
                        Changes = new List<PropertyChange>()
                    };
                    var serializedCToolWorkflowMessage = JsonConvert.SerializeObject(ctoolWorkflowMessage, Formatting.Indented);
                    if (UseSyncDisposableProducer(entityPM.Tenant))
                    {

                        using (var shipmentCreateMessageProducer = new Producer())
                        {
                            var result = shipmentCreateMessageProducer.Produce(KafkaTopics.ShipmentsCreateTopic,
                           KakaMessageTypes.ShipmentCreate, serializedCToolWorkflowMessage, true);
                        }

                    }
                    else
                    {
                        var shipmentCreateMessageProducer = GetInstatnce(entityPM.Tenant);
                        var result = shipmentCreateMessageProducer.Produce(KafkaTopics.ShipmentsCreateTopic,
                            KakaMessageTypes.ShipmentCreate, serializedCToolWorkflowMessage, UseSyncDisposableProducer(entityPM.Tenant));
                        AddProducerToQueue(shipmentCreateMessageProducer);
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, entityPM.Tenant, null, "ProduceShipmentCreateMessage", null, null);
            }
        }

        public static void ProduceShipmentUpdateMessage(Shipment entityPoco, ShipmentPM entityPM)
        {
            try
            {
                if (entityPM != null && FeatureToggleHelper.HasFeatureToggle("CTL", entityPM.Tenant))
                {
                    var shipmentChanges = GetShipmentUpdateChanges(entityPoco, entityPM);

                    entityPM.FirstPickupATA = entityPoco.ShipmentComputedFields.FirstPickupATA;
                    entityPM.FirstPickupATD = entityPoco.ShipmentComputedFields.FirstPickupATD;

                    var shipmentPMString = JsonConvert.SerializeObject(entityPM, Formatting.Indented);
                    Dictionary<string, object> shipmentPMDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(shipmentPMString);
                    shipmentPMDictionary.Add("DocumentsFilingPM", GetShipmentDocumentsFilingPM(entityPM.ShipmentNumber, entityPM.Tenant));
                    shipmentPMString = JsonConvert.SerializeObject(shipmentPMDictionary, Formatting.Indented);

                    CToolWorkflowMessage ctoolWorkflowMessage = new CToolWorkflowMessage()
                    {
                        Entity = JsonConvert.DeserializeObject(shipmentPMString),
                        Changes = shipmentChanges
                    };

                    var serializedCToolWorkflowMessage = JsonConvert.SerializeObject(ctoolWorkflowMessage, Formatting.Indented);

                    if (UseSyncDisposableProducer(entityPM.Tenant))
                    {
                        using (var shipmentUpdateMessageProducer = new Producer())
                        {
                            var result = shipmentUpdateMessageProducer.Produce(KafkaTopics.ShipmentsUpdateTopic,
                           KakaMessageTypes.ShipmentUpdate, serializedCToolWorkflowMessage, true);
                        }
                    }
                    else
                    {

                        var shipmentUpdateMessageProducer = GetInstatnce(entityPM.Tenant);
                        var result = shipmentUpdateMessageProducer.Produce(KafkaTopics.ShipmentsUpdateTopic,
                        KakaMessageTypes.ShipmentUpdate, serializedCToolWorkflowMessage, UseSyncDisposableProducer(entityPM.Tenant));
                        AddProducerToQueue(shipmentUpdateMessageProducer);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, entityPM.Tenant, null, "ProduceShipmentUpdateMessage", null, null);
            }
        }

        public static void ProduceShipmentDocumentUpload(string shipmentId, int tenant)
        {
            try
            {
                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(shipmentId, tenant);
                var shipmentPMString = JsonConvert.SerializeObject(shipmentPM, Formatting.Indented);
                Dictionary<string, object> shipmentPMDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(shipmentPMString);
                shipmentPMDictionary.Add("DocumentsFilingPM", GetShipmentDocumentsFilingPM(shipmentPM.ShipmentNumber, tenant));
                shipmentPMString = JsonConvert.SerializeObject(shipmentPMDictionary, Formatting.Indented);

                CToolWorkflowMessage ctoolWorkflowMessage = new CToolWorkflowMessage()
                {
                    Entity = JsonConvert.DeserializeObject(shipmentPMString),
                    Changes = new List<PropertyChange>()
                };

                var serializedCToolWorkflowMessage = JsonConvert.SerializeObject(ctoolWorkflowMessage, Formatting.Indented);
                if (UseSyncDisposableProducer(tenant))
                {
                    using (var shipmentUpdateMessageProducer = new Producer())
                    {
                        var result = shipmentUpdateMessageProducer.Produce(KafkaTopics.ShipmentsCreateTopic,
                       KakaMessageTypes.ShipmentCreate, serializedCToolWorkflowMessage, true);
                    }
                }
                else
                {

                    var shipmentUpdateMessageProducer = GetInstatnce(tenant);
                    var result = shipmentUpdateMessageProducer.Produce(KafkaTopics.ShipmentsUpdateTopic,
                        KakaMessageTypes.ShipmentUpdate, serializedCToolWorkflowMessage, UseSyncDisposableProducer(tenant));
                    AddProducerToQueue(shipmentUpdateMessageProducer);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, null, "ProduceShipmentDocumentUpload", null, null);
            }
        }

        public static void ProduceSendEmailMessage(CommunicationLog communicationLog)
        {
            try
            {
                ProduceMailMessage(communicationLog);
            }
            catch (Exception ex)
            {
                try
                {
                    if (ex.Message.Contains("Message size too large"))
                    {
                        communicationLog.ExternalDocument = null;
                        communicationLog.ResponseDocument = null;
                        communicationLog.InternalDocument = null;
                        ProduceMailMessage(communicationLog);

                    }
                }
                catch (Exception)
                { 
                    ExceptionHandler.HandleException(ex, DateTime.Now, communicationLog.Tenant, null, "ProduceSendEmailMessage", null, null);
                }
            }
        }

        private static void ProduceMailMessage(CommunicationLog communicationLog)
        {
            var serializedSendEmailMessage = JsonConvert.SerializeObject(communicationLog, Formatting.Indented,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
            if (UseSyncDisposableProducer(communicationLog.Tenant))
            {
                using (var sendEmailProducer = new Producer())
                {
                    var result = sendEmailProducer.Produce(KafkaTopics.LookupsTopic, KakaMessageTypes.CommunicationLog, serializedSendEmailMessage, UseSyncDisposableProducer(communicationLog.Tenant));
                }
            }
            else
            {

                Producer sendEmailProducer = GetInstatnce(communicationLog.Tenant);
                var result = sendEmailProducer.Produce(KafkaTopics.LookupsTopic, KakaMessageTypes.CommunicationLog, serializedSendEmailMessage, UseSyncDisposableProducer(communicationLog.Tenant));
                AddProducerToQueue(sendEmailProducer);
            }
        }

        private static List<DocumentsFilingPM> GetShipmentDocumentsFilingPM(string shipmentNumber, int tenant)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            string entityId = shipmentQuery.GetEntitiyIdByShipmentNumber(shipmentNumber, tenant);
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            List<DocumentsFilingPM> documentsFilingPM = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(entityId, "", ObjectTableRepository.GetObjectTableByName("Shipment"), "I", tenant);
            documentsFilingPM = documentsFilingPM.Where(d => d.DocumentId != null && d.HasFile == true).ToList();

            return documentsFilingPM;
        }

        private static List<PropertyChange> GetShipmentUpdateChanges(Shipment entityPoco, ShipmentPM entityPM)
        {
            List<PropertyChange> shipmentChanges = WorkflowEntityChanges.GetChangedProperties(entityPoco, entityPM);
            List<PropertyChange> ShipmentMasterDataChanges = WorkflowEntityChanges.GetChangedProperties(entityPoco.ShipmentMasterData, entityPM);
            List<PropertyChange> ShipmentComputedFieldsChanges = WorkflowEntityChanges.GetChangedProperties(entityPoco.ShipmentComputedFields, entityPM);
            return shipmentChanges.Union(ShipmentMasterDataChanges).Union(ShipmentComputedFieldsChanges).ToList();
        }
        #endregion

        #region Container
        public static void ProduceContainerCreateMessage(Container containerPoco, ContainerPM containerPm)
        {
            try
            {
                if (containerPm != null && FeatureToggleHelper.HasFeatureToggle("CTL", containerPm.Tenant))
                {
                    CToolWorkflowMessage ctoolWorkflowMessage = new CToolWorkflowMessage()
                    {
                        Entity = containerPm,
                        Changes = new List<PropertyChange>()
                    };

                    var serializedCToolWorkflowMessage = JsonConvert.SerializeObject(ctoolWorkflowMessage, Formatting.Indented);
                    if (UseSyncDisposableProducer(containerPm.Tenant))
                    {
                        using (var containerCreateMessageProducer = new Producer())
                        {
                            var result = containerCreateMessageProducer.Produce(KafkaTopics.ContainerCreateTopic,
                                                      KakaMessageTypes.ContainerCreate, serializedCToolWorkflowMessage, UseSyncDisposableProducer(containerPm.Tenant));
                        }
                    }
                    else
                    {
                        var containerCreateMessageProducer = GetInstatnce(containerPm.Tenant);
                        var result = containerCreateMessageProducer.Produce(KafkaTopics.ContainerCreateTopic,
                            KakaMessageTypes.ContainerCreate, serializedCToolWorkflowMessage, UseSyncDisposableProducer(containerPm.Tenant));
                        AddProducerToQueue(containerCreateMessageProducer);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, containerPm.Tenant, null, "ProduceContainerCreateMessage", null, null);
            }
        }

        public static void ProduceContainerUpdateMessage(Container containerPoco, ContainerPM containerPm)
        {
            try
            {
                if (containerPm != null && FeatureToggleHelper.HasFeatureToggle("CTL", containerPm.Tenant))
                {
                    var containerPMString = JsonConvert.SerializeObject(containerPm, Formatting.Indented);

                    CToolWorkflowMessage ctoolWorkflowMessage = new CToolWorkflowMessage()
                    {
                        Entity = JsonConvert.DeserializeObject(containerPMString),
                        Changes = WorkflowEntityChanges.GetChangedProperties(containerPoco, containerPm)
                    };

                    var serializedCToolWorkflowMessage = JsonConvert.SerializeObject(ctoolWorkflowMessage, Formatting.Indented);
                    if (UseSyncDisposableProducer(containerPm.Tenant))
                    {
                        using (var containerUpdateMessageProducer = new Producer())
                        {
                            var result = containerUpdateMessageProducer.Produce(KafkaTopics.ContainerUpdateTopic,
                          KakaMessageTypes.ContainerUpdate, serializedCToolWorkflowMessage, UseSyncDisposableProducer(containerPm.Tenant));
                        }
                    }
                    else
                    {
                        var containerUpdateMessageProducer = GetInstatnce(containerPm.Tenant);
                        var result = containerUpdateMessageProducer.Produce(KafkaTopics.ContainerUpdateTopic,
                            KakaMessageTypes.ContainerUpdate, serializedCToolWorkflowMessage, UseSyncDisposableProducer(containerPm.Tenant));
                        AddProducerToQueue(containerUpdateMessageProducer);
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, containerPm.Tenant, null, "ProduceContainerUpdateMessage", null, null);
            }
        }
        #endregion
    }
}
