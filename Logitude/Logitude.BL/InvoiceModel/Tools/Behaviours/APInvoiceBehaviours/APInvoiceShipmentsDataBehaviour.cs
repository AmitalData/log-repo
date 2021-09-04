using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.BL.InvoiceModel.Tools.Behaviours.APInvoiceBehaviours
{
    public class APInvoiceShipmentsDataBehaviour
    {
        private APInvoicePM aPInvoice;
        private List<APInvoiceMultipleShipmentPM> allInvoiceShipments;
        private IShipmentsContext iShipmentsContext;
        private List<ShipmentMasterData> invoiceConsoleShipments;
        public APInvoiceShipmentsDataBehaviour(APInvoicePM aPInvoice)
        {
            this.aPInvoice = aPInvoice;
            this.iShipmentsContext = ShipmentsContext.GetContext(aPInvoice.Tenant);
        }
        public void CopmuteShipmentsData()
        {
            if (aPInvoice.IsMultipleEntities)
            {
                this.GetAllInvoicesShipments();
                aPInvoice.ShipmentsNumbers = this.CopmuteShipmentsNumbersFromMultipleShipments();
                aPInvoice.MasterShipmentNumbers = this.CopmuteMasterShipmentNumbersFromMultipleShipments();
                aPInvoice.HouseNumbers = this.CopmuteHouseNumbersFromMultipleShipments();
                aPInvoice.MasterNumbers = this.CopmuteMasterNumbersFromMultipleShipments();
            }
            else
            {
                aPInvoice.ShipmentsNumbers = aPInvoice.MainEntityReference;
                aPInvoice.MasterShipmentNumbers = this.GetMainEntityMasterShipmentNumbers();
                aPInvoice.HouseNumbers = aPInvoice.HouseNumber;
                aPInvoice.MasterNumbers = aPInvoice.MasterNumber;
            }
        }
        private void GetAllInvoicesShipments()
        {
            this.allInvoiceShipments = aPInvoice.InvoiceMultipleShipments.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
        }
        private string CopmuteShipmentsNumbersFromMultipleShipments()
        {
            string shipmentsNumbersField = "";
            foreach (APInvoiceMultipleShipmentPM multipleShipment in allInvoiceShipments)
            {
                this.AddNumberToShipmentsNumbersField(ref shipmentsNumbersField, multipleShipment.ShipmentNumber);
            }

            this.TrimLengthTo1000(ref shipmentsNumbersField);

            return shipmentsNumbersField;
        }

        private void AddNumberToShipmentsNumbersField(ref string shipmentsNumbersField, string shipmentNumber)
        {
            if (!string.IsNullOrEmpty(shipmentNumber))
            {
                if (!shipmentsNumbersField.Contains(shipmentNumber))
                {
                    shipmentsNumbersField = string.IsNullOrEmpty(shipmentsNumbersField) ? shipmentNumber : shipmentsNumbersField + ", " + shipmentNumber;
                }
            }
        }

        private string CopmuteMasterShipmentNumbersFromMultipleShipments()
        {
            string masterShipmentNumbers = "";
            this.GetInvoiceConsoleShipments();

            foreach (var multipleShipment in allInvoiceShipments)
            {
                this.AddNumberToMasterShipmentNumbersField(ref masterShipmentNumbers, multipleShipment);
            }
            this.TrimLengthTo1000(ref masterShipmentNumbers);
            return masterShipmentNumbers;
        }

        private void GetInvoiceConsoleShipments()
        {
            var shipmentsIds = allInvoiceShipments.Where(a => a.ShipmentLevelCode == "C").Select(a => a.ShipmentId).ToList();
            this.invoiceConsoleShipments = (from d in iShipmentsContext.ShipmentMasterDatas where shipmentsIds.Contains(d.Id) select d).ToList();
        }

        private void AddNumberToMasterShipmentNumbersField(ref string masterShipmentNumbers, APInvoiceMultipleShipmentPM shipment)
        {
            if (shipment.ShipmentLevelCode == "C")
            {
                var console = this.invoiceConsoleShipments.Where(a => a.Id == shipment.ShipmentId).FirstOrDefault();
                if (!masterShipmentNumbers.Contains(console.MasterShipmentNumber))
                {
                    masterShipmentNumbers = string.IsNullOrEmpty(masterShipmentNumbers) ? console.MasterShipmentNumber : masterShipmentNumbers + ", " + console.MasterShipmentNumber;
                }
            }
            else
            {
                if (!masterShipmentNumbers.Contains(shipment.ShipmentNumber))
                {
                    masterShipmentNumbers = string.IsNullOrEmpty(masterShipmentNumbers) ? shipment.ShipmentNumber : masterShipmentNumbers + ", " + shipment.ShipmentNumber;
                }
            }
        }

        private string CopmuteHouseNumbersFromMultipleShipments()
        {
            string houseNumbers = "";
            foreach (APInvoiceMultipleShipmentPM multipleShipment in allInvoiceShipments)
            {
                this.AddNumberToHouseNumbersField(ref houseNumbers, multipleShipment.House);
            }

            this.TrimLengthTo1000(ref houseNumbers);
            return houseNumbers;
        }

        private void AddNumberToHouseNumbersField(ref string houseNumbers, string house)
        {
            if (!string.IsNullOrEmpty(house))
            {
                if (!houseNumbers.Contains(house))
                {
                    houseNumbers = string.IsNullOrEmpty(houseNumbers) ? house : houseNumbers + ", " + house;
                }
            }
        }

        private string CopmuteMasterNumbersFromMultipleShipments()
        {
            string masterNumbers = "";
            foreach (APInvoiceMultipleShipmentPM multipleShipment in allInvoiceShipments)
            {
                this.AddNumberToMasterNumbersField(ref masterNumbers, multipleShipment.Master);
            }

            this.TrimLengthTo1000(ref masterNumbers);
            return masterNumbers;
        }

        private void AddNumberToMasterNumbersField(ref string masterNumbers, string master)
        {
            if (!string.IsNullOrEmpty(master))
            {
                if (!masterNumbers.Contains(master))
                {
                    masterNumbers = string.IsNullOrEmpty(masterNumbers) ? master : masterNumbers + ", " + master;
                }
            }
        }

        private void TrimLengthTo1000(ref string field)
        {
            if (!string.IsNullOrEmpty(field))
            {
                if (field.Length > 1000)
                {
                    field = field.Substring(0, 1000);
                }
            }
        }
        
        private string GetMainEntityMasterShipmentNumbers()
        {
            var shipment = (from d in iShipmentsContext.Shipments where d.Id == aPInvoice.MainEntityId select d).FirstOrDefault();
            var masterShipmentNumbers = shipment?.ShipmentNumber;
            if (shipment?.ShipmentLevelCode == "C")
            {
                var masterShipment = (from d in iShipmentsContext.Shipments where d.Id == shipment.MasterShipmentDataId select d).FirstOrDefault();
                masterShipmentNumbers = masterShipment.ShipmentNumber;
            }
            return masterShipmentNumbers;
        }
    }
}
