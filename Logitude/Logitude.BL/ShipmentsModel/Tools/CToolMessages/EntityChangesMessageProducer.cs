using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.CToolWorkflows.Models;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.KafkaConfigurations;
using Logitude.Server.Tools.Messages;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Server.Tools.CToolWorkflows
{
    public static class EntityChangesMessageProducer
    {
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

                    var shipmentCreateMessageProducer = new Producer();
                    var result = shipmentCreateMessageProducer.Produce(KafkaTopics.ShipmentsCreateTopic, 
                        KakaMessageTypes.ShipmentCreate, serializedCToolWorkflowMessage);
                    shipmentCreateMessageProducer.ProducerBuilder.Flush();
                    shipmentCreateMessageProducer.ProducerBuilder.Dispose();
                }
            }
            catch(Exception ex)
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
                    shipmentPMString =  JsonConvert.SerializeObject(shipmentPMDictionary, Formatting.Indented);

                    CToolWorkflowMessage ctoolWorkflowMessage = new CToolWorkflowMessage()
                    {
                        Entity = JsonConvert.DeserializeObject(shipmentPMString),
                        Changes = shipmentChanges
                    };

                    var serializedCToolWorkflowMessage = JsonConvert.SerializeObject(ctoolWorkflowMessage, Formatting.Indented);

                    var shipmentUpdateMessageProducer = new Producer();
                    var result = shipmentUpdateMessageProducer.Produce(KafkaTopics.ShipmentsUpdateTopic,
                        KakaMessageTypes.ShipmentUpdate, serializedCToolWorkflowMessage);
                    shipmentUpdateMessageProducer.ProducerBuilder.Flush();
                    shipmentUpdateMessageProducer.ProducerBuilder.Dispose();
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
                shipmentPMString =  JsonConvert.SerializeObject(shipmentPMDictionary, Formatting.Indented);

                CToolWorkflowMessage ctoolWorkflowMessage = new CToolWorkflowMessage()
                {
                    Entity = JsonConvert.DeserializeObject(shipmentPMString),
                    Changes = new List<PropertyChange>()
                };

                var serializedCToolWorkflowMessage = JsonConvert.SerializeObject(ctoolWorkflowMessage, Formatting.Indented);

                var shipmentUpdateMessageProducer = new Producer();
                var result = shipmentUpdateMessageProducer.Produce(KafkaTopics.ShipmentsUpdateTopic,
                    KakaMessageTypes.ShipmentUpdate, serializedCToolWorkflowMessage);
                shipmentUpdateMessageProducer.ProducerBuilder.Flush();
                shipmentUpdateMessageProducer.ProducerBuilder.Dispose();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, null, "ProduceShipmentDocumentUpload", null, null);
            }
        }

        public static void ProduceSendEmailMessage(CommunicationLog communicationLog)
        {
            Producer sendEmailProducer = null;
            try
            {
                sendEmailProducer = new Producer();
                var serializedSendEmailMessage = JsonConvert.SerializeObject(communicationLog, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                var result = sendEmailProducer.Produce(KafkaTopics.LookupsTopic, KakaMessageTypes.CommunicationLog, serializedSendEmailMessage);
                sendEmailProducer.ProducerBuilder.Flush();
                sendEmailProducer.ProducerBuilder.Dispose();                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, communicationLog.Tenant, null, "ProduceSendEmailMessage", null, null);
            }
            finally
            {
                if (sendEmailProducer != null)
                {
                    sendEmailProducer.ProducerBuilder.Flush();
                    sendEmailProducer.ProducerBuilder.Dispose();
                }
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

                    var containerCreateMessageProducer = new Producer();
                    var result = containerCreateMessageProducer.Produce(KafkaTopics.ContainerCreateTopic,
                        KakaMessageTypes.ContainerCreate, serializedCToolWorkflowMessage);
                    containerCreateMessageProducer.ProducerBuilder.Flush();
                    containerCreateMessageProducer.ProducerBuilder.Dispose();
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

                    var containerUpdateMessageProducer = new Producer();
                    var result = containerUpdateMessageProducer.Produce(KafkaTopics.ContainerUpdateTopic,
                        KakaMessageTypes.ContainerUpdate, serializedCToolWorkflowMessage);
                    containerUpdateMessageProducer.ProducerBuilder.Flush();
                    containerUpdateMessageProducer.ProducerBuilder.Dispose();
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
