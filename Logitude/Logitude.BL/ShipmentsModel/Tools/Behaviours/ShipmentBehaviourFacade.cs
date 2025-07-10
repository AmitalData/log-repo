using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviour;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.Infrastructure.Data.Models.AuditLog;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Collections.Generic;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    public class ShipmentBehaviourFacade
    {
        private IShipmentBehaviour updateShipmentComputedFields;
        private IShipmentBehaviour updateCrossDocks;
        private IShipmentBehaviour shipmentDigitalFields;
        private IShipmentBehaviour updateShipmentDocsFields;
        public bool ReceivablePricingUpdated_CrossDoc = false;
        public bool DatesUpdated_CrossDoc = false;
        public ShipmentPM shipmentPM = null;
        public IShipmentsContext shipmentsContext = null;
        public bool isNewEntity = false;

        public ShipmentBehaviourFacade(ShipmentPM shipmentPM, IShipmentsContext context, ShipmentComputedFields updatedShipmentComputedFields, bool isNewEntity)
        {
            this.shipmentPM = shipmentPM;
            this.shipmentsContext = context;
            this.isNewEntity = isNewEntity;
            updateShipmentComputedFields = new UpdateShipmentComputedFieldsBehaviour(shipmentPM, context, updatedShipmentComputedFields, isNewEntity);
            updateCrossDocks = new UpdateCrossDockBehaviour(shipmentPM);
        }

        public void Handle(List<FieldChange> fieldChanges = null)
        {
            updateShipmentComputedFields.Handle(fieldChanges);
            if (shipmentPM.DirectionId != "C")
            {
                updateCrossDocks.Handle();
            }
            ReceivablePricingUpdated_CrossDoc = updateCrossDocks.ReceivablePricingUpdated;
            DatesUpdated_CrossDoc = updateCrossDocks.DatesFromCrossDocsUpdated;
        }

        public void HandleShipmentDigitalFields(ShipmentDigitalField shipmentDigitalFields)
        {
            this.shipmentDigitalFields = new ShipmentDigitalFieldsBehaviour(shipmentPM, shipmentsContext, shipmentDigitalFields, isNewEntity);
            this.shipmentDigitalFields.Handle();
        }

        public void HandleShipmentDocsFields(ShipmentDocsField shipmentDocsField)
        {
            this.updateShipmentDocsFields = new UpdateShipmentDocsFieldBehaviour(shipmentPM, shipmentsContext, shipmentDocsField);
            this.updateShipmentDocsFields.Handle();
        }

        public void Trace(ShipmentTracing shipmentTracing)
        {
            updateCrossDocks.Trace(shipmentTracing);
        }

        public void Save()
        {
            updateShipmentComputedFields.Save();
            updateCrossDocks.Save();
            shipmentDigitalFields.Save();

            if (updateShipmentDocsFields != null)
                updateShipmentDocsFields.Save();
        }
    }
}
