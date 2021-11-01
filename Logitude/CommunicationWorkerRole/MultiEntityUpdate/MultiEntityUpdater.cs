using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.MultiEntityUpdate
{
    class MultiEntityUpdater : IMultiEntityUpdater
    {
        private readonly ShipmentUpdater shipmentUpdater;
        public MultiEntityUpdater(int tenant)
        {
            shipmentUpdater = new ShipmentUpdater(tenant);
        }
        public void Update(object entityPM)
        {
            shipmentUpdater.Update(entityPM);
        }
    }
}
