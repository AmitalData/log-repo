using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class CASSDataProvider : BaseDataProvider
    {
        [Key]
        public int Id { get; set; }
        public byte[] Logo { get; set; }

        public string AirlineName { get; set; }
        public string TenantName { get; set; }
        public string TenantIATACode { get; set; }
        public string TenantAddress { get; set; }
        public string TenantLocalCurrencyCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public double? TotalDue { get; set; }
        public double? TotalVAT { get; set; }
        public double? AdvancedPayment { get; set; }
        public double? FinalPayableNetSale { get; set; }

        public List<ShipmentDataRecord> ShipmentsData { get; set; }
    }

    public class ShipmentDataRecord
    {
        public string Id { get; set; }
        public string AWBNumber { get; set; }
        public string DestinationCode { get; set; }
        public string CommodityNumber { get; set; }
        public double? ChargeableWeight { get; set; }
        public string INCENTIVE { get; set; }
        public double? PayablesFrieghtRate { get; set; }

        public int CrossTabIndex { get; set; }
        public string CrossTabHeader { get; set; }
        public double? CrossTabValue { get; set; }        
    }
}