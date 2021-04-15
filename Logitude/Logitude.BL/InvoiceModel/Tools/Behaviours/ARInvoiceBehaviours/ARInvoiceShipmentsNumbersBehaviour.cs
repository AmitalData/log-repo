using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.Behaviours.ARInvoiceBehaviours
{
    public class ARInvoiceShipmentsNumbersBehaviour
    {
        private ARInvoicePM aRInvoice;
        private ARInvoiceRepository aRInvoiceRepository;
        public ARInvoiceShipmentsNumbersBehaviour(ARInvoicePM aRInvoice)
        {
            this.aRInvoice = aRInvoice;
            this.aRInvoiceRepository = new ARInvoiceRepository(aRInvoice.Tenant);
        }

        public void CopmuteShipmentsNumbers()
        {
            if (aRInvoice.IsConsolidationInvoice)
            {
                aRInvoice.ShipmentsNumbers = this.CopmuteShipmentsNumbersFromConnectedShipments();
            }

            else
            {
                aRInvoice.ShipmentsNumbers = aRInvoice.MainEntityReference;
            }
        }

        private string CopmuteShipmentsNumbersFromConnectedShipments()
        {          
            string shipmentsNumbersField = "";

            List<ConstituentPM> allConstituents = aRInvoice.ConstituentInvoices.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            foreach (ConstituentPM constituent in allConstituents)
            {
                string constituentShipmentNumber = this.GetConstituentShipmentNumber(constituent);
                this.AddNumberToShipmentsNumbersField(ref shipmentsNumbersField, constituentShipmentNumber);
            }

            this.TrimShipmentsNumbersTo1000(shipmentsNumbersField);

            return shipmentsNumbersField;
        }
        private string GetConstituentShipmentNumber(ConstituentPM constituent)
        {
            string constituentShipmentNumber = "";

            ARInvoice constituentInvoice = aRInvoiceRepository.GetSingleARInvoice(constituent.Id, constituent.Tenant);
            if(constituentInvoice!= null)
            {
                constituentShipmentNumber = constituentInvoice.MainEntityReference;
            }

            return constituentShipmentNumber;
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
            if(!string.IsNullOrEmpty(shipmentsNumbersField))
            {
                if (shipmentsNumbersField.Length > 1000)
                {
                    shipmentsNumbersField = shipmentsNumbersField.Substring(0, 1000);
                }
            }
        }
    }
}
