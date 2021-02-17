using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
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
            if (string.IsNullOrEmpty(initializer.EntityPM.ShipmentTypeId) && initializer.EntityPM.TransportModeId == "A")
            {
                initializer.EntityPM.ShipmentTypeId = "Air";
            }

            initializer.EntityPM.House = MethodHelper.Trim(initializer.EntityPM.House);

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
                initializer.EntityPM.CreateDateTime = initializer.TodayDateTime.Value;
            }

            if (initializer.EntityPM.ShipmentLevelCode == "C")
            {
                initializer.EntityPM.ProrateReceivables = initializer.LoggedTenant.ProrateMasterReceivables;
            }


        }

        private void InitializeOnUpdating()
        {
            initializer.EntityPM.OldStatusValue = initializer.EntityPOCO.StatusId;


        }
    }
}
