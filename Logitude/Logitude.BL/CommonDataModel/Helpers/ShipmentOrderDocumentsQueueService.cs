using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Data.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Collections.Generic;

namespace Logitude.BL.CommonDataModel.Helpers
{
    public class ShipmentOrderDocumentsQueueService
    {

        public void Build(DocumentsFilingPM documentsFiling)
        {
            if (documentsFiling.DontAddToQueue) return;
            if (!documentsFiling.IsSharedWithCustomer) return;
            if (string.IsNullOrEmpty(documentsFiling.ObjectTableId)) return;

            var objectTableName = ObjectTableRepository.GetSingleObjectTableById(documentsFiling.ObjectTableId, documentsFiling.Tenant)?.Name;
            if (objectTableName != "ShipmentOrder") return;

            ShipmentOrder shipmentOrder = GetShipmentOrder(documentsFiling);
            if (shipmentOrder == null) return;
            if (!ValidForLogBoxTransfer(shipmentOrder)) return;

            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("ImportersShipmentDocumentsQueue", 0);
            queueservice.Send(new Dictionary<string, string>() { { "ShipmentOrderId", shipmentOrder.Id }, { "DocumentFilingId", documentsFiling.Id }, { "Tenant", documentsFiling.Tenant.ToString() }, }, documentsFiling.Tenant);
        }

        private ShipmentOrder GetShipmentOrder(DocumentsFilingPM documentsFiling)
        {
            if (!string.IsNullOrEmpty(documentsFiling.EntityId)) return new ShipmentOrderRepository(documentsFiling.Tenant).GetSingleById(documentsFiling.EntityId, documentsFiling.Tenant);
            if (!string.IsNullOrEmpty(documentsFiling.EntityNumber)) return new ShipmentOrderRepository(documentsFiling.Tenant).GetSingleByOrderNumber(documentsFiling.EntityNumber, documentsFiling.Tenant);
            return null;
        }

        private bool ValidForLogBoxTransfer(ShipmentOrder shipmentOrder)
        {
            return shipmentOrder.TransportModeId == "O" && shipmentOrder.DirectionId == "E" && !string.IsNullOrEmpty(shipmentOrder.CustomerTenantNumber?.ToString()) && !string.IsNullOrEmpty(shipmentOrder.CustomerShipmentNumber) && HaveCustomerAccess(shipmentOrder);
        }

        private bool HaveCustomerAccess(ShipmentOrder shipmentOrder)
        {
            CustomerTenantAccessInfo customerTenantAccessInfo = new CustomerTenantAccessQuery(shipmentOrder.Tenant).GetCustomerTenantAccessInfo(shipmentOrder.Tenant, shipmentOrder.ShipperId);
            return customerTenantAccessInfo != null && customerTenantAccessInfo.IsExportActivated && customerTenantAccessInfo.HasAccess && customerTenantAccessInfo.CustomerTenant != 0;
        }
    }
}
