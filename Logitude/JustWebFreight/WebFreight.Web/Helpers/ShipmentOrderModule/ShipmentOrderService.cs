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
        private readonly ShipmentOrderUpdateService shipmentOrderUpdateService;
        public ShipmentOrderService(int tenant)
        {
            IShipmentOrderContext MyContext = ShipmentOrderContext.GetContext(tenant);
            shipmentOrderUpdateService = new ShipmentOrderUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
            this.tenant = tenant;
        }

        public ShipmentOrder GetByOrderNumber(string orderNumber)
        {
            return new ShipmentOrderQueryService(tenant).GetByOrderNumber(orderNumber, tenant);
        }
        public ShipmentOrderPM Create(ShipmentOrder entity)
        {
            ShipmentOrderPM shipmentOrderPM = ShipmentOrderDataMappingAndValidatin(entity, ChangeSetOperation.Insert);
            shipmentOrderUpdateService.Update(shipmentOrderPM,true);
            return shipmentOrderPM;
        }
        public ShipmentOrderPM Update(ShipmentOrder entity)
        {
            ShipmentOrderPM shipmentOrderPM = ShipmentOrderDataMappingAndValidatin(entity, ChangeSetOperation.Update);
            shipmentOrderUpdateService.Update(shipmentOrderPM, true);
            return shipmentOrderPM;
        }
        public ShipmentOrderPM Delete(string orderNumber)
        {
            ShipmentOrderPM shipmentOrderPM = new ShipmentOrderQueryService(tenant).GetSinglePMByOrderNumber(orderNumber, tenant);
            shipmentOrderPM.IsCancelled = true;
            shipmentOrderPM.ChangeSetOp = ChangeSetOperation.Update;
            shipmentOrderUpdateService.Update(shipmentOrderPM, true);
            return shipmentOrderPM;
        }

        private ShipmentOrderPM ShipmentOrderDataMappingAndValidatin(ShipmentOrder entity, ChangeSetOperation changeSetOp)
        {

            if (!IsNewEntity(changeSetOp))
            {
                entity.Id = new ShipmentOrderQueryService(tenant).GetIdByOrderNumber(entity.OrderNumber, tenant);
            }

            var shipmentOrderQueryService = new ShipmentOrderQueryService(tenant);
            ShipmentOrderPM entityPM = shipmentOrderQueryService.ShipmentOrderDataMappingAndValidatin(entity, tenant);
            entityPM.Tenant = tenant;
            entityPM.ChangeSetOp = changeSetOp;
            return entityPM;
        }
        private bool IsNewEntity(ChangeSetOperation changeSetOp)
        {
            return changeSetOp == ChangeSetOperation.Insert;
        }
    }
}