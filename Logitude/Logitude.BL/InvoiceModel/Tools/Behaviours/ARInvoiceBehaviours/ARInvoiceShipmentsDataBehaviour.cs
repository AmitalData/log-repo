using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.BL.InvoiceModel.Tools.Behaviours.ARInvoiceBehaviours
{
    public class ARInvoiceShipmentsDataBehaviour
    {
        private ARInvoicePM aRInvoice;
        private ARInvoiceRepository aRInvoiceRepository;
        private List<ConstituentPM> allConstituents;
        private IShipmentsContext iShipmentsContext;
        private IInvoiceContext iInvoiceContext;
        string constituentShipmentNumber = "";
        string constituentMasterNumber = "";
        string constituentHouseNumbers = "";
        string shipmentsNumbersField = "";
        string shipmentsMasterField = "";
        string shipmentsHouseField = "";
        private List<ARInvoice> constituentInvoice;
        private List<Shipment> invoiceShipments;

        public ARInvoiceShipmentsDataBehaviour(ARInvoicePM aRInvoice)
        {
            this.aRInvoice = aRInvoice;
            this.aRInvoiceRepository = new ARInvoiceRepository(aRInvoice.Tenant);
            this.iShipmentsContext = ShipmentsContext.GetContext(aRInvoice.Tenant);
            this.iInvoiceContext = InvoiceContext.GetContext(aRInvoice.Tenant);
        }

        public void CopmuteShipmentsData()
        {
            if (aRInvoice.IsConsolidationInvoice)
            {
                this.GetAllInvoicesShipments();
                this.CopmuteShipmentsDataFromConnectedShipments();
                aRInvoice.ShipmentsNumbers = shipmentsNumbersField;
                aRInvoice.HouseNumbers = shipmentsHouseField;
                aRInvoice.MasterNumbers = shipmentsMasterField;
                aRInvoice.MasterShipmentNumbers = this.CopmuteMasterShipmentNumbersFromMultipleShipments();

            }

            else
            {
                aRInvoice.ShipmentsNumbers = aRInvoice.MainEntityReference;
                aRInvoice.MasterShipmentNumbers = this.GetMainEntityMasterShipmentNumbers();
                aRInvoice.HouseNumbers = aRInvoice.HouseNumber;
                aRInvoice.MasterNumbers = aRInvoice.MasterNumber;
            }
        }
        private void GetAllInvoicesShipments()
        {
            this.allConstituents = aRInvoice.ConstituentInvoices.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            var invoicesIds = allConstituents.Select(a => a.Id).ToList();
            this.constituentInvoice = (from d in iInvoiceContext.ARInvoices where invoicesIds.Contains(d.Id) select d).ToList();

            var shipmentsIds = constituentInvoice.Select(a => a.MainEntityId).ToList();
            this.invoiceShipments = (from d in iShipmentsContext.Shipments where shipmentsIds.Contains(d.Id) select d).ToList();

        }

        private void  CopmuteShipmentsDataFromConnectedShipments()
        {         
            foreach (ConstituentPM constituent in allConstituents)
            {
                this.GetConstituentShipmentNumber(constituent);
                this.AddNumberToShipmentsNumbersField(ref shipmentsNumbersField, constituentShipmentNumber);
                this.AddNumberToMasterNumbersField(ref shipmentsMasterField, constituentMasterNumber);
                this.AddNumberToHouseNumbersField(ref shipmentsHouseField, constituentHouseNumbers);
            }

            this.TrimLengthTo1000(ref shipmentsNumbersField);
            this.TrimLengthTo1000(ref shipmentsMasterField);
            this.TrimLengthTo1000(ref shipmentsHouseField);
        }

        private void GetConstituentShipmentNumber(ConstituentPM constituent)
        {
            ARInvoice constituentInvoice = this.constituentInvoice.Where(a => a.Id == constituent.Id).FirstOrDefault();
            if(constituentInvoice!= null)
            {
                this.constituentShipmentNumber = constituentInvoice.MainEntityReference;
                this.constituentMasterNumber = constituentInvoice.MasterNumber;
                this.constituentHouseNumbers = constituentInvoice.HouseNumber;
            }
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

        private void AddNumberToMasterNumbersField(ref string masterNumbersField, string masterNumber)
        {
            if (!string.IsNullOrEmpty(masterNumber))
            {
                if (!masterNumbersField.Contains(masterNumber))
                {
                    masterNumbersField = string.IsNullOrEmpty(masterNumbersField) ? masterNumber : masterNumbersField + ", " + masterNumber;
                }
            }
        }

        private void AddNumberToHouseNumbersField(ref string shipmentsHouseField, string house)
        {
            if (!string.IsNullOrEmpty(house))
            {
                if (!shipmentsHouseField.Contains(house))
                {
                    shipmentsHouseField = string.IsNullOrEmpty(shipmentsHouseField) ? house : shipmentsHouseField + ", " + house;
                }
            }
        }

        private void TrimLengthTo1000(ref string shipmentsNumbersField)
        {
            if(!string.IsNullOrEmpty(shipmentsNumbersField))
            {
                if (shipmentsNumbersField.Length > 1000)
                {
                    shipmentsNumbersField = shipmentsNumbersField.Substring(0, 1000);
                }
            }
        }

        private string CopmuteMasterShipmentNumbersFromMultipleShipments()
        {
            string masterShipmentNumbers = "";
            foreach (var shipment in invoiceShipments)
            {
                this.AddNumberToMasterShipmentNumbersField(ref masterShipmentNumbers, shipment);
            }
            this.TrimLengthTo1000(ref masterShipmentNumbers);
            return masterShipmentNumbers;
        }

        private void AddNumberToMasterShipmentNumbersField(ref string masterShipmentNumbers, Shipment shipment)
        {
            if (shipment.ShipmentLevelCode == "H")
            {
                var console = (from d in iShipmentsContext.ShipmentMasterDatas where d.Id == shipment.Id select d).FirstOrDefault();
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

        private string GetMainEntityMasterShipmentNumbers()
        {
            var shipment = (from d in iShipmentsContext.Shipments where d.Id == aRInvoice.MainEntityId select d).FirstOrDefault();
            var masterShipmentNumbers = shipment?.ShipmentNumber;
            if (shipment?.ShipmentLevelCode == "H")
            {
                var masterShipment = (from d in iShipmentsContext.Shipments where d.Id == shipment.MasterShipmentDataId select d).FirstOrDefault();
                masterShipmentNumbers = masterShipment.ShipmentNumber;
            }
            return masterShipmentNumbers;
        }
    }
}
