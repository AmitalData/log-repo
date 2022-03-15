using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviour;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
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
        private IShipmentBehaviour updateCrossDocks;
        public bool ReceivablePricingUpdated_CrossDoc = false;
        public bool DatesUpdated_CrossDoc = false;

        public ShipmentBehaviourFacade(ShipmentPM shipmentPM, IShipmentsContext context, ShipmentComputedFields updatedShipmentComputedFields, bool isNewEntity)
        {
            updateShipmentComputedFields = new UpdateShipmentComputedFieldsBehaviour(shipmentPM, context, updatedShipmentComputedFields, isNewEntity);
            updateCrossDocks = new UpdateCrossDockBehaviour(shipmentPM);
        }

        public void Handle()
        {
            updateShipmentComputedFields.Handle();
            updateCrossDocks.Handle();
            ReceivablePricingUpdated_CrossDoc = updateCrossDocks.ReceivablePricingUpdated;
            DatesUpdated_CrossDoc = updateCrossDocks.DatesFromCrossDocsUpdated;
        }

        public void Trace(ShipmentTracing shipmentTracing)
        {
            updateCrossDocks.Trace(shipmentTracing);
        }

        public void Save()
        {
            updateShipmentComputedFields.Save();
            updateCrossDocks.Save();

        }
    }
}
