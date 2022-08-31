using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.BatchPrint
{
    public class ShipmentBatchPrinter : BatchPrinter
    {
        private IShipmentsContext shipmentsContext;
        public ShipmentBatchPrinter(BatchPrinterArgs batchPrinterArgs) : base(batchPrinterArgs)
        {
            this.shipmentsContext = ShipmentsContext.GetContext(batchPrinterArgs.Tenant);
        }

        public override void CustomeValidation(PrintEntityKeys item)
        {
            Shipment shipment = this.GetShipment(item);
            if (shipment == null)
                throw new Exception($"Shipment not found");

            this.Validate_TransportMode(shipment);
            this.Validate_ShipmentLevel(shipment);
        }
        private void Validate_TransportMode(Shipment shipment)
        {
            if(shipment.TransportModeId == "A" && !documentType.IsAir)
            {
                throw new Exception($"This shipment transportmode does not support the selected document type. Please print Air document");
            }

            if (shipment.TransportModeId == "O" && !documentType.IsOcean)
            {
                throw new Exception($"This shipment transportmode does not support the selected document type. Please print Ocean document");
            }

            if (shipment.TransportModeId == "I" && !documentType.IsInland)
            {
                throw new Exception($"This shipment transportmode does not support the selected document type. Please print Inland document");
            }
        }
        private void Validate_ShipmentLevel(Shipment shipment)
        {
            if (shipment.ShipmentLevelCode == "D" && !documentType.IsDirect)
            {
                throw new Exception($"This shipment level does not support the selected document type. Please print Direct document");
            }

            if (shipment.ShipmentLevelCode == "H" && !documentType.IsHouse)
            {
                throw new Exception($"This shipment level does not support the selected document type. Please print House document");
            }

            if (shipment.ShipmentLevelCode == "C" && !documentType.IsMaster)
            {
                throw new Exception($"This shipment level does not support the selected document type. Please print Console document");
            }
        }

        private Shipment GetShipment(PrintEntityKeys item)
        {
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            return shipmentRepository.GetSingleShipment(item.EntityId, _batchPrinterArgs.Tenant);
        }
        public override void AfterPrint(PrintEntityKeys item)
        {
            
        }
    }
}