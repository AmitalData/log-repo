using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class VendorChargesAnalysisDataProvider : BaseDataProvider
    {
        public string VendorFilter { get; set; }
        public string ChargesTypeFilter { get; set; }

        public List<VendorChargesShipment> Shipments { get; set; }
    }
    
    public class VendorChargesShipment
    {
        public DateTime? CreateDate { get; set; }
        public DateTime? FirstOpCloseDate { get; set; }
        public string ShipmentNumber { get; set; }
        public string CustomerName { get; set; }
        public string OpenedBy { get; set; }
        public string Type { get; set; }
        public string From { get; set; }
        public string FromState { get; set; }
        public string FromCountry { get; set; }
        public string To { get; set; }
        public string ToState { get; set; }
        public string ToCountry { get; set; }
        public string SpecialServices { get; set; }
        public double? ChargeableWeightInKG { get; set; }
        public double? ChargeableWeight { get; set; }
        public double? VolumeInCBM { get; set; }

        public string CarrierId { get; set; }
        public string Carrier { get; set; }
        public string ChargesTypeId { get; set; }
        public string ChargesType { get; set; }
        public string Notes { get; set; }
        public double? AccountedAmount { get; set; }
        public double? OpenAmount { get; set; }
    }    
}