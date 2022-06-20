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
                if (FeatureToggleHelper.HasFeatureToggle("CTL", entityPM.Tenant))
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
                if (FeatureToggleHelper.HasFeatureToggle("CTL", entityPM.Tenant))
                {
                    var shipmentPMString = JsonConvert.SerializeObject(entityPM, Formatting.Indented);
                    Dictionary<string, object> shipmentPMDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(shipmentPMString);
                    shipmentPMDictionary.Add("DocumentsFilingPM", GetShipmentDocumentsFilingPM(entityPM.ShipmentNumber, entityPM.Tenant));
                    shipmentPMString =  JsonConvert.SerializeObject(shipmentPMDictionary, Formatting.Indented);

                    CToolWorkflowMessage ctoolWorkflowMessage = new CToolWorkflowMessage()
                    {
                        Entity = JsonConvert.DeserializeObject(shipmentPMString),
                        Changes = GetShipmentUpdateChanges(entityPoco, entityPM)
                    };

                    var serializedCToolWorkflowMessage = JsonConvert.SerializeObject(ctoolWorkflowMessage, Formatting.Indented);

                    var shipmentCreateMessageProducer = new Producer();
                    var result = shipmentCreateMessageProducer.Produce(KafkaTopics.ShipmentsCreateTopic,
                        KakaMessageTypes.ShipmentCreate, serializedCToolWorkflowMessage);
                    shipmentCreateMessageProducer.ProducerBuilder.Flush();
                    shipmentCreateMessageProducer.ProducerBuilder.Dispose();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, entityPM.Tenant, null, "ProduceShipmentUpdateMessage", null, null);
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
            return shipmentChanges.Union(ShipmentMasterDataChanges).ToList();
        }
        #endregion

    }
}
