using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
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
        }

        private string GetCustomerIdByDirectionId(string directionId, string consigneeId, string shipperId)
        {
            return directionId == "E" || directionId == "D" ? shipperId : directionId == "I" ? consigneeId : null;
        }

        protected override void OnUpdating(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
            entityPM.ShipmentId = GetShipmentIdByNumber(entityPM.ShipmentNumber, entityPM.Tenant);
            entityPM.CustomerId = GetCustomerIdByDirectionId(entityPM.DirectionId, entityPM.ConsigneeId, entityPM.ShipperId);
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
    }
}
