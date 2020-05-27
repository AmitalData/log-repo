using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviour;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    public class ShipmentBehaviourFacade
    {
        private IShipmentBehaviour updateShipmentComputedFields;

        public ShipmentBehaviourFacade(ShipmentPM shipmentPM, IShipmentsContext context, ShipmentComputedFields updatedShipmentComputedFields, bool isNewEntity)
        {
            updateShipmentComputedFields = new UpdateShipmentComputedFieldsBehaviour(shipmentPM, context, updatedShipmentComputedFields, isNewEntity);
        }

        public void Handle()
        {
            updateShipmentComputedFields.Handle();
        }
    }
}
