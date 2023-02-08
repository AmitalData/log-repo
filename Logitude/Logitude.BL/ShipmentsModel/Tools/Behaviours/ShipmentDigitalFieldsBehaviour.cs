using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.Infrastructure.Data.Models.AuditLog;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System.Collections.Generic;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    public class ShipmentDigitalFieldsBehaviour : IShipmentBehaviour
    {
        private ShipmentDigitalField entity;
        private ShipmentPM shipmentPM;
        private IShipmentsContext context;
        private ShipmentDigitalFieldRepository shipmentDigitalFieldsRepository;
        private int tenant;
        private bool isNewEntity;

        public bool ReceivablePricingUpdated { get; set; }
        public bool DatesFromCrossDocsUpdated { get; set; }

        public ShipmentDigitalFieldsBehaviour(ShipmentPM shipmentPM, IShipmentsContext context, ShipmentDigitalField shipmentDigitalField, bool isNewEntity)
        {
            this.shipmentPM = shipmentPM;
            this.context = context;
            this.isNewEntity = isNewEntity;
            this.tenant = shipmentPM.Tenant;
            this.entity = shipmentDigitalField;
            shipmentDigitalFieldsRepository = new ShipmentDigitalFieldRepository(context);
        }

        public void Handle(List<FieldChange> fieldChanges = null)
        {
            GetEntity();
            MapEntity();
        }

        private void GetEntity()
        {
            if (isNewEntity)
            {
                entity = new ShipmentDigitalField()
                {
                    Id = shipmentPM.Id,
                    Tenant = tenant,
                };
            }
            else if (entity == null)
            {
                entity = shipmentDigitalFieldsRepository.GetSingleShipmentDigitalFields(shipmentPM.Id, tenant);
            }
        }

        private void MapEntity()
        {
            MapFields();
            
        }

        public void Save()
        {
            if (isNewEntity)
            {
                shipmentDigitalFieldsRepository.Add(entity);
            }
            else
            {
                shipmentDigitalFieldsRepository.Update(entity);
            }

            shipmentDigitalFieldsRepository.SubmitChanges();
        }

        private void MapFields()
        {
            entity.IsCustomerArchived = false;
        }

        public void Trace(ShipmentTracing shipmentTracing)
        {

        }
    }
}
