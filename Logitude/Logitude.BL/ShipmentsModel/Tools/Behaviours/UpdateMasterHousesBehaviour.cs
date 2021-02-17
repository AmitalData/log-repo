using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    public class UpdateMasterHousesBehaviour
    {
        private int Tenant;
        private Shipment entityPOCO;
        private ShipmentPM entityPM;
        public UpdateMasterHousesBehaviour(ShipmentPM entityPM, Shipment entityPOCO)
        {
            this.Tenant = entityPM.Tenant;
            this.entityPM = entityPM;
        }

        public void Handle()
        {
            if (entityPM.ShipmentLevelCode == "C")
            {

            }
        }
    }
}
