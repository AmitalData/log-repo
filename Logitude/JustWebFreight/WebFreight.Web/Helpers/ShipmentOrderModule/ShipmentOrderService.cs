using Logitude.Server.Tools;
using Logitude.ShipmentOrderModule.BL.APIDataContract.ApiV1;
using Logitude.ShipmentOrderModule.BL.EntityUpdateServices;
using Logitude.ShipmentOrderModule.Data;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers.ExternalAPIHelpers;

namespace WebFreight.Web.Helpers.ShipmentOrderModule
{
    public class ShipmentOrderService
    {
        public ShipmentOrder GetByOrderNumber(int tenant, string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber))
                return new ShipmentOrder();
            return new ShipmentOrderQueryService(tenant).GetByOrderNumber(orderNumber, tenant);
        }


        public ShipmentOrderPM Create(ShipmentOrder entity)
        {

            IShipmentOrderContext MyContext = ShipmentOrderContext.GetContext(entity.Tenant);
            ShipmentOrderPM shipmentOrderPM = MapPocoToPM(entity, ChangeSetOperation.Insert);
            ShipmentOrderUpdateService service = new ShipmentOrderUpdateService(MyContext, new Dictionary<string, IContext>(), shipmentOrderPM.Tenant);
            service.Update(shipmentOrderPM, true);

            return shipmentOrderPM;
        }

        public ShipmentOrderPM Update(ShipmentOrder entity)
        {

            IShipmentOrderContext MyContext = ShipmentOrderContext.GetContext(entity.Tenant);
            ShipmentOrderPM shipmentOrderPM = MapPocoToPM(entity, ChangeSetOperation.Update);
            ShipmentOrderUpdateService service = new ShipmentOrderUpdateService(MyContext, new Dictionary<string, IContext>(), shipmentOrderPM.Tenant);
            service.Update(shipmentOrderPM, true);

            return shipmentOrderPM;
        }


        private ShipmentOrderPM MapPocoToPM(ShipmentOrder entity, ChangeSetOperation changeSetOp)
        {

            if (changeSetOp == ChangeSetOperation.Update)
            {
                entity.Id = new ShipmentOrderQueryService(entity.Tenant).GetByOrderNumber(entity.OrderNumber, entity.Tenant)?.Id;
                if (entity.Id == null)
                {
                    throw new ApplicationException("ShipmentOrder with orderNumber " + entity.OrderNumber + " doesn't exist");
                }
            }

            var shipmentOrderQueryService = new ShipmentOrderQueryService(entity.Tenant);
            ShipmentOrderPM entityPM = shipmentOrderQueryService.ShipmentOrderDataMappingAndValidatin(entity, entity.Tenant);
            entityPM.Tenant = entity.Tenant;
            entityPM.ChangeSetOp = changeSetOp;
            return entityPM;
        }



    }
}