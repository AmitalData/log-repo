using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.WorkerRole.MultiEntityUpdate
{
    public class MultiEntityUpdateCloner
    {
        public static object CloneEntity(object entityPM)
        {
            object cloneEntity = null;
            if (entityPM.GetType() == typeof(ShipmentPM))
            {
                cloneEntity = CloneShipmentPMEntity(entityPM);
            }

            else if (entityPM.GetType() == typeof(ContainerPM))
            {
                cloneEntity = CloneContainerPMEntity(entityPM);
            }

            return cloneEntity;
        }

        private static object CloneShipmentPMEntity(object entityPM)
        {
            ShipmentPM ShipmentPM = (ShipmentPM)entityPM;
            return new ShipmentPM()
            {
                IsOperationalClosed = ShipmentPM.IsOperationalClosed,
                IsAccountingClosed = ShipmentPM.IsAccountingClosed,
                Tenant = ShipmentPM.Tenant
            };
        }

        private static object CloneContainerPMEntity(object entityPM)
        {
            ContainerPM containerPM = (ContainerPM)entityPM;
            return new ContainerPM()
            {
                Tenant = containerPM.Tenant
            };
        }
    }
}