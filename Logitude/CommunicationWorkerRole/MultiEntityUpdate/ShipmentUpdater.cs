using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.ShipmentsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.MultiEntityUpdate
{
    class ShipmentUpdater : IMultiEntityUpdater
    {
        private readonly int tenant;
        public ShipmentUpdater(int tenant)
        {
            this.tenant = tenant;
        }
        public void Update(object shipmentPM)
        {
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext((int)tenant);
            ShipmentService shipmentService = new ShipmentService(shipmentsContext, (ShipmentPM)shipmentPM, "");
            shipmentService.Update(true);
        }
    }
}
