using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using Logitude.BL.ShipmentsModel.Tools.Initializers;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    public class UpdateShipmentDocsFieldBehaviour
    {
        private ShipmentServiceInitializer shipmentServiceInitializer;
        private ShipmentDocsFieldRepository shipmentDocsFieldRepository;
        private ShipmentDocsField myShipmentDocsField;
        private bool isNew;
        public UpdateShipmentDocsFieldBehaviour(ShipmentServiceInitializer initializer)
        {
            this.shipmentServiceInitializer = initializer;
            this.shipmentDocsFieldRepository = new ShipmentDocsFieldRepository(initializer.ShipmentContext);
            this.isNew = false;
        }

        public void Handle()
        {
            GetEntity();
            MapEntity();
            Save();
        }

        private void GetEntity()
        {
            if (string.IsNullOrEmpty(shipmentServiceInitializer.ShipmentDocsFieldFromWorkerRole.Id))
            {
                isNew = true;
                myShipmentDocsField = new ShipmentDocsField()
                {
                    Id = shipmentServiceInitializer.EntityPM.Id,
                    Tenant = shipmentServiceInitializer.EntityPM.Tenant,
                };
            }

            else
            {
                myShipmentDocsField = shipmentDocsFieldRepository.GetSingleShipmentDocsField(shipmentServiceInitializer.EntityPM.Id, shipmentServiceInitializer.EntityPM.Tenant);
            }
        }

        private void MapEntity()
        {
            myShipmentDocsField.IsPODReceived = shipmentServiceInitializer.ShipmentDocsFieldFromWorkerRole.IsPODReceived;
            myShipmentDocsField.PODReceivedDate = shipmentServiceInitializer.ShipmentDocsFieldFromWorkerRole.PODReceivedDate;
            myShipmentDocsField.IsCommercialInvoiceReceived = shipmentServiceInitializer.ShipmentDocsFieldFromWorkerRole.IsCommercialInvoiceReceived;
            myShipmentDocsField.CommercialInvoiceReceivedDate = shipmentServiceInitializer.ShipmentDocsFieldFromWorkerRole.CommercialInvoiceReceivedDate;
            myShipmentDocsField.IsPackingListReceived = shipmentServiceInitializer.ShipmentDocsFieldFromWorkerRole.IsPackingListReceived;
            myShipmentDocsField.PackingListReceivedDate = shipmentServiceInitializer.ShipmentDocsFieldFromWorkerRole.PackingListReceivedDate;
            myShipmentDocsField.IsBOLReceived = shipmentServiceInitializer.ShipmentDocsFieldFromWorkerRole.IsBOLReceived;
            myShipmentDocsField.BOLReceivedDate = shipmentServiceInitializer.ShipmentDocsFieldFromWorkerRole.BOLReceivedDate;
            myShipmentDocsField.IsMasterBOLReceived = shipmentServiceInitializer.ShipmentDocsFieldFromWorkerRole.IsMasterBOLReceived;
            myShipmentDocsField.MasterBOLReceivedDate = shipmentServiceInitializer.ShipmentDocsFieldFromWorkerRole.MasterBOLReceivedDate;
            myShipmentDocsField.IsArrivalNoticeReceived = shipmentServiceInitializer.ShipmentDocsFieldFromWorkerRole.IsArrivalNoticeReceived;
            myShipmentDocsField.ArrivalNoticeReceivedDate = shipmentServiceInitializer.ShipmentDocsFieldFromWorkerRole.ArrivalNoticeReceivedDate;
        }
        private void Save()
        {
            if (isNew)
            {
                shipmentDocsFieldRepository.Add(myShipmentDocsField);
            }

            else
            {
                shipmentDocsFieldRepository.Update(myShipmentDocsField);
            }
        }
    }
}
