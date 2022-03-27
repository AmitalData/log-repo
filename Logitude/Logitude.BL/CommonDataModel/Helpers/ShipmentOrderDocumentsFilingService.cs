using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Data.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Collections.Generic;

namespace Logitude.BL.CommonDataModel.Helpers
{
    public class ShipmentOrderDocumentsFilingService
    {
        public string GetDocumentTypeId(DocumentsFiling documentsFiling, int tenant)
        {
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
            var shipmentOrderObjectTableId = ObjectTableRepository.GetObjectTableByName(documentsFiling.EntityType.Name);
            var documentTypeId = documentTypeRepository.GetDocumentTypeIdByCodeAndObjectTable(documentsFiling.DocumentType.Code, shipmentOrderObjectTableId, tenant);
            if (!string.IsNullOrEmpty(documentTypeId)) return documentTypeId;

            var shipmentObjectTableId = ObjectTableRepository.GetObjectTableByName("shipment");
            var documentType = documentTypeRepository.GetByCodeAndObjectTable(documentsFiling.DocumentType.Code, shipmentObjectTableId, tenant);
            if (documentType == null) return null;

            documentType.ObjectTableId = shipmentOrderObjectTableId;
            documentType.Id = IdCounter.GetNumber("DocumentType", tenant).ToString();
            documentType.Code = "SO" + documentType.Code;

            documentTypeRepository.Add(documentType);
            documentTypeRepository.SubmitChanges();

            return documentType.Id;
        }
        public void BuildDocumentQueue(DocumentsFilingPM documentsFiling)
        {
            if (string.IsNullOrEmpty(documentsFiling.EntityId)) return;

            var shipmentOrder = new ShipmentOrderRepository(documentsFiling.Tenant).GetSinglesById(documentsFiling.EntityId, documentsFiling.Tenant);  
            if (!ValidForLogBoxTransfer(shipmentOrder))
                return;

            //IQueueService queueservice = new DbQueueService();
            //queueservice.InitializeQueue("ImporterShipmentOrderQueue", 0);
            //queueservice.Send(
            //    new Dictionary<string, string>() { { "ShipmentOrderId", shipmentOrder.Id }, { "Tenant", shipmentOrder.Tenant.ToString() } }, shipmentOrder.Tenant);
        }
        private bool ValidForLogBoxTransfer(ShipmentOrder shipmentOrder)
        {
            CustomerTenantAccessInfo customerTenantAccessInfo = new CustomerTenantAccessQuery(shipmentOrder.Tenant).GetCustomerTenantAccessInfo(shipmentOrder.Tenant, shipmentOrder.ShipperId);
            return shipmentOrder.TransportModeId == "O" && shipmentOrder.DirectionId == "E" && customerTenantAccessInfo != null && customerTenantAccessInfo.IsExportActivated && customerTenantAccessInfo.HasAccess && customerTenantAccessInfo.CustomerTenant != 0 && !string.IsNullOrEmpty(shipmentOrder.CustomerTenantNumber?.ToString()) && !string.IsNullOrEmpty(shipmentOrder.CustomerShipmentNumber);
        }
    }
}
