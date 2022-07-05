using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using Logitude.BL.ShipmentsModel.Tools.Initializers;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    public class UpdateShipmentDocsFieldBehaviour : IServiceBehaviour
    {
        private ShipmentServiceInitializer initializer;
        private ShipmentDocsField shipmentDocsField;
        private ShipmentPM shipmentPM;
        private ShipmentDocsFieldRepository shipmentDocsFieldRepository;
        private bool isNewEntity;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.shipmentPM = this.initializer.EntityPM;
            this.shipmentDocsFieldRepository = new ShipmentDocsFieldRepository(this.initializer.ShipmentContext);

            this.HandleBehaviour();
        }

        public void HandleBehaviour()
        {
            GetEntity();
            MapEntity();
        }

        private void GetEntity()
        {
            if (isNewEntity)
            {
                shipmentDocsField = new ShipmentDocsField()
                {
                    Id = shipmentPM.Id,
                    Tenant = this.initializer.Tenant,
                };
            }

            else if (shipmentDocsField == null)
            {
                shipmentDocsField = shipmentDocsFieldRepository.GetSingleShipmentDocsField(shipmentPM.Id, this.initializer.Tenant);
            }
        }

        private void MapEntity()
        {

        }
    }
}
