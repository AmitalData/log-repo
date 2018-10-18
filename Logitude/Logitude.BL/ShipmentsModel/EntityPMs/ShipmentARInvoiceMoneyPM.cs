using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ShipmentARInvoiceMoneyPM
    {
        public string Id { get; set; }

        private List<ShipmentARInvoicePM> aRInvoices;
        [Include]
        [Association("ShipmentARInvoiceShipmentARMoney", "Id", "ShipmentARInvoiceMoneyId")]
        public List<ShipmentARInvoicePM> ARInvoices
        {
            get
            {
                if (aRInvoices == null)
                {
                    aRInvoices = new List<ShipmentARInvoicePM>();
                }

                return aRInvoices;
            }

            set
            {
                if (value != null)
                {
                    aRInvoices = value;
                }
            }
        }

        private List<ARInvoiceChargePM> aRCharges;
        [Include]
        [Association("ShipmentARChargeShipmentARMoney", "Id", "ShipmentARChargeMoneyId")]
        public List<ARInvoiceChargePM> ARCharges
        {
            get
            {
                if (aRCharges == null)
                {
                    aRCharges = new List<ARInvoiceChargePM>();
                }

                return aRCharges;
            }

            set
            {
                if (value != null)
                {
                    aRCharges = value;
                }
            }
        }

    }
}