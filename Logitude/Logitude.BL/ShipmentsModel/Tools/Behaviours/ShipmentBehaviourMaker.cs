using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviour;
using Simplog.Data.ShipmentsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    public class ShipmentBehaviourMaker
    {
        private IShipmentBehaviour updateShipmentComputedFields;
        public ShipmentBehaviourMaker(ShipmentPM shipmentPM, IShipmentsContext context, bool isNewEntity)
        {
            updateShipmentComputedFields = new UpdateShipmentComputedFieldsBehaviour(shipmentPM, context, isNewEntity);
        }
        public void HandleShipmentComputedFields()
        {
            updateShipmentComputedFields.Handle();
        }
    }
}
