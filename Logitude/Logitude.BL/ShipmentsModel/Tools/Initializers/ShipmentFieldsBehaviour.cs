using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Initializers
{
    public class ShipmentFieldsBehaviour : IServiceBehaviour
    {
        private ShipmentServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;

            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            if (initializer.IsNewEntity)
            {
                InitializeOnCreating();
            }

            else
            {
                InitializeOnUpdating();
            }
        }

        private void InitializeOnCreating()
        {
            initializer.EntityPM.FWBStatusCode = "NSEN";
            initializer.EntityPM.FHLStatusCode = "NSEN";
            initializer.EntityPM.CargonautFHLStatusCode = "NSEN";
            initializer.EntityPM.CargonautFWBStatusCode = "NSEN";
            initializer.EntityPM.ShipmentReceivableStatusCode = "NORE";
            initializer.EntityPM.ShipmentPayableStatusCode = "NOPA";
            initializer.EntityPM.INTTRASIStatusCode = "NSEN";
            initializer.EntityPM.INTTRABookingStatusCode = "NS";
            initializer.EntityPM.INTTRABookingTransStatusCode = "NST";

            if (string.IsNullOrEmpty(initializer.EntityPM.CreatedByUserId))
            {
                initializer.EntityPM.CreatedByUserId = initializer.LoggedContact.Id;
            }

            if (!initializer.EntityPM.IsHybrid)
            {
                initializer.EntityPM.CreateDateTime = TenantServerConfigration.GetCurrentDateTime(initializer.Tenant);
            }
        }

        private void InitializeOnUpdating()
        {

        }
    }
}
