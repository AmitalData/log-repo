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
        private readonly int tenant;

        public ShipmentOrderService(int tenant)
        {
            this.tenant = tenant;
        }

        public ShipmentOrder GetByOrderNumber(int tenant, string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber))
                return new ShipmentOrder();
            return new ShipmentOrderQueryService(tenant).GetByOrderNumber(orderNumber, tenant);
        }


        public ShipmentOrderPM Create(ShipmentOrder entity)
        {

            IShipmentOrderContext MyContext = ShipmentOrderContext.GetContext(tenant);
            ShipmentOrderPM shipmentOrderPM = MapPocoToPM(entity, ChangeSetOperation.Insert);
            ShipmentOrderUpdateService service = new ShipmentOrderUpdateService(MyContext, new Dictionary<string, IContext>(), shipmentOrderPM.Tenant);
            service.Update(shipmentOrderPM, true);

            return shipmentOrderPM;
        }

        public ShipmentOrderPM Update(ShipmentOrder entity)
        {

            IShipmentOrderContext MyContext = ShipmentOrderContext.GetContext(tenant);
            ShipmentOrderPM shipmentOrderPM = MapPocoToPM(entity, ChangeSetOperation.Update);
            ShipmentOrderUpdateService service = new ShipmentOrderUpdateService(MyContext, new Dictionary<string, IContext>(), shipmentOrderPM.Tenant);
            service.Update(shipmentOrderPM, true);

            return shipmentOrderPM;
        }


        private ShipmentOrderPM MapPocoToPM(ShipmentOrder entity, ChangeSetOperation changeSetOp)
        {

            if (changeSetOp == ChangeSetOperation.Update)
            {
                entity.Id = new ShipmentOrderQueryService(tenant).GetByOrderNumber(entity.OrderNumber, tenant)?.Id;
                if (string.IsNullOrEmpty( entity.Id))
                {
                    throw new ApplicationException("ShipmentOrder with orderNumber " + entity.OrderNumber + " doesn't exist");
                }
            }

            var shipmentOrderQueryService = new ShipmentOrderQueryService(tenant);
            ShipmentOrderPM entityPM = shipmentOrderQueryService.ShipmentOrderDataMappingAndValidatin(entity, tenant);
            entityPM.Tenant = tenant;
            entityPM.ChangeSetOp = changeSetOp;
            return entityPM;
        }



    }
}