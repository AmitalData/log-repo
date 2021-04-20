using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.Behaviours.APInvoiceBehaviours
{
    public class APInvoiceShipmentsNumbersBehaviour
    {
        private APInvoicePM aPInvoice;
        public APInvoiceShipmentsNumbersBehaviour(APInvoicePM aPInvoice)
        {
            this.aPInvoice = aPInvoice;
        }

        public void CopmuteShipmentsNumbers()
        {
            if (aPInvoice.IsMultipleEntities)
            {
                aPInvoice.ShipmentsNumbers = this.CopmuteShipmentsNumbersFromMultipleShipments();
            }

            else
            {
                aPInvoice.ShipmentsNumbers = aPInvoice.MainEntityReference;
            }
        }

        private string CopmuteShipmentsNumbersFromMultipleShipments()
        {
            string shipmentsNumbersField = "";

            List<APInvoiceMultipleShipmentPM> allInvoiceShipments = aPInvoice.InvoiceMultipleShipments.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            foreach (APInvoiceMultipleShipmentPM multipleShipment in allInvoiceShipments)
            {
                this.AddNumberToShipmentsNumbersField(ref shipmentsNumbersField, multipleShipment.ShipmentNumber);
            }

            this.TrimShipmentsNumbersTo1000(shipmentsNumbersField);

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
        private void TrimShipmentsNumbersTo1000(string shipmentsNumbersField)
        {
            if (!string.IsNullOrEmpty(shipmentsNumbersField))
            {
                if (shipmentsNumbersField.Length > 1000)
                {
                    shipmentsNumbersField = shipmentsNumbersField.Substring(0, 1000);
                }
            }
        }
    }
}
