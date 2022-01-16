using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentOrderModule.BL.EntityUpdateServices
{
    public partial class ShipmentOrderUpdateService
    {
        protected override void OnCreating(ShipmentOrderPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.SecurityKey = Guid.NewGuid().ToString("N");
            }

            entityPM.ShipmentId = GetShipmentIdByNumber(entityPM.ShipmentNumber, entityPM.Tenant);
            entityPM.CustomerId = GetCustomerIdByDirectionId(entityPM.DirectionId, entityPM.ConsigneeId, entityPM.ShipperId);

            BuildShipmentOrderQueue(entityPM);
        }

        private string GetCustomerIdByDirectionId(string directionId, string consigneeId, string shipperId)
        {
            return directionId == "E" || directionId == "D" ? shipperId : directionId == "I" ? consigneeId : null;
        }

        protected override void OnUpdating(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
            entityPM.ShipmentId = GetShipmentIdByNumber(entityPM.ShipmentNumber, entityPM.Tenant);
            entityPM.CustomerId = GetCustomerIdByDirectionId(entityPM.DirectionId, entityPM.ConsigneeId, entityPM.ShipperId);

            BuildShipmentOrderQueue(entityPM);
        }

        private string GetShipmentIdByNumber(string shipmentNumber, int tenant)
        {
            if (string.IsNullOrEmpty(shipmentNumber))
                return null;

            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            string shipmentId = shipmentQuery.GetShipmentIdByShipmentNumber(shipmentNumber, tenant);
            if (string.IsNullOrEmpty(shipmentId))
                throw new ApplicationException("Shipment with Shipment Number " + shipmentNumber + " doesn't exist");
            return shipmentId;

        }

        private void BuildShipmentOrderQueue(ShipmentOrderPM entityPM)
        {
            if (!ValidForLogBoxTransfer(entityPM))
                return;

            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("ImporterShipmentOrderQueue", 0);
            queueservice.Send(
                new Dictionary<string, string>() { { "ShipmentOrderId", entityPM.Id }, { "Tenant", entityPM.Tenant.ToString() } }, entityPM.Tenant);
        }

        private bool ValidForLogBoxTransfer(ShipmentOrderPM entityPM)
        {
            CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(entityPM.Tenant);
            CustomerTenantAccessInfo customerTenantAccessInfo = customerTenantAccessQuery.GetCustomerTenantAccessInfo(entityPM.Tenant, entityPM.ShipperId);
            //customerTenantAccessInfo != null && customerTenantAccessInfo.HasAccess && customerTenantAccessInfo.CustomerTenant != 0  &&
            return !string.IsNullOrEmpty(entityPM.CustomerTenantNumber?.ToString()) && !string.IsNullOrEmpty(entityPM.CustomerShipmentNumber);
        }
    }
}
