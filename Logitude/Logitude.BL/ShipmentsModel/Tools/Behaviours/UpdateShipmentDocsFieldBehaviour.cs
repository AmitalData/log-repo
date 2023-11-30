using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using System.Collections.Generic;
using Logitude.Infrastructure.Data.Models.AuditLog;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    public class UpdateShipmentDocsFieldBehaviour: IShipmentBehaviour
    {
        private ShipmentDocsFieldRepository shipmentDocsFieldRepository;
        private ShipmentDocsField shipmentDocsFieldFromWorkerRole;
        private ShipmentDocsField myShipmentDocsField;
        private ShipmentPM shipmentPM;
        private bool isNew;

        public bool ReceivablePricingUpdated { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool DatesFromCrossDocsUpdated { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public UpdateShipmentDocsFieldBehaviour(ShipmentPM shipmentPM, IShipmentsContext context, ShipmentDocsField shipmentDocsField)
        {
            this.shipmentDocsFieldRepository = new ShipmentDocsFieldRepository(context);
            this.shipmentDocsFieldFromWorkerRole = shipmentDocsField;
            this.shipmentPM = shipmentPM;
            this.isNew = false;
        }

        public void Handle(List<FieldChange> fieldChanges = null)
        {
            GetEntity();
            MapEntity();            
        }

        private void GetEntity()
        {
            if (string.IsNullOrEmpty(shipmentDocsFieldFromWorkerRole.Id))
            {
                isNew = true;
                myShipmentDocsField = new ShipmentDocsField()
                {
                    Id = shipmentPM.Id,
                    Tenant = shipmentPM.Tenant,
                };
            }

            else
            {
                myShipmentDocsField = shipmentDocsFieldRepository.GetSingleShipmentDocsField(shipmentPM.Id, shipmentPM.Tenant);
            }
        }

        private void MapEntity()
        {
            myShipmentDocsField.IsPODReceived = shipmentDocsFieldFromWorkerRole.IsPODReceived;
            myShipmentDocsField.PODReceivedDate = shipmentDocsFieldFromWorkerRole.PODReceivedDate;
            myShipmentDocsField.IsCommercialInvoiceReceived = shipmentDocsFieldFromWorkerRole.IsCommercialInvoiceReceived;
            myShipmentDocsField.CommercialInvoiceReceivedDate = shipmentDocsFieldFromWorkerRole.CommercialInvoiceReceivedDate;
            myShipmentDocsField.IsPackingListReceived = shipmentDocsFieldFromWorkerRole.IsPackingListReceived;
            myShipmentDocsField.PackingListReceivedDate = shipmentDocsFieldFromWorkerRole.PackingListReceivedDate;
            myShipmentDocsField.IsBOLReceived = shipmentDocsFieldFromWorkerRole.IsBOLReceived;
            myShipmentDocsField.BOLReceivedDate = shipmentDocsFieldFromWorkerRole.BOLReceivedDate;
            myShipmentDocsField.IsMasterBOLReceived = shipmentDocsFieldFromWorkerRole.IsMasterBOLReceived;
            myShipmentDocsField.MasterBOLReceivedDate = shipmentDocsFieldFromWorkerRole.MasterBOLReceivedDate;
            myShipmentDocsField.IsArrivalNoticeReceived = shipmentDocsFieldFromWorkerRole.IsArrivalNoticeReceived;
            myShipmentDocsField.ArrivalNoticeReceivedDate = shipmentDocsFieldFromWorkerRole.ArrivalNoticeReceivedDate;
        }
        public void Save()
        {
            if (isNew)            
                shipmentDocsFieldRepository.Add(myShipmentDocsField);            

            else            
                shipmentDocsFieldRepository.Update(myShipmentDocsField);

            shipmentDocsFieldRepository.SubmitChanges();
        }

        public void Trace(ShipmentTracing shipmentTracing)
        {
            
        }
    }
}
