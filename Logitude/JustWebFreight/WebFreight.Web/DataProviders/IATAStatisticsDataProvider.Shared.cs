using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class IATAStatisticsDataProvider : BaseDataProvider
    {
        public string Name { get; set; }

        public DateTime? FromPeriod { get; set; }
        public DateTime? ToPeriod { get; set; }
        public string SelectedAirline { get; set; }
        public string IataCASSCode { get; set; }
        public string Prefix { get; set; }
        public string CASSCode { get; set; }

        public string TenantName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Signature { get; set; }
        public string TenantPhone { get; set; }
        public string TenantFax { get; set; }

        public string TotalFreightCharges { get; set; }
        public double? TotalChargeableWeight { get; set; }

        public string FreightChargesCurrency { get; set; }
        public string ChargeableWeightUnit { get; set; }

        public List<IATAStatisticsRecord> RecordList { get; set; }
        public List<IATAStatisticsGroup> GroupList { get; set; }

        //Classes
        public class IATAStatisticsRecord
        {
            public string Master { get; set; }
            public string ShipperOrConsignee { get; set; }
            public DateTime? DepartureDate { get; set; }
            public string Departure { get; set; }
            public string Destination { get; set; }
            public double? Weight { get; set; }
            public double? ChargeableWeight { get; set; }
            public string AirlineName { get; set; }
            public string Prefix { get; set; }
            public string FreightChargeCurrency { get; set; }
            public double? FreightChargeAmount { get; set; }
            public string WeightUnit { get; set; }
            public string CassCode { get; set; }
            public string ShipperName { get; set; }
            public string ConsigneeName { get; set; }
            public string AirlineId { get; set; }
            public string FreightChargeString { get; set; }
            public string PortOfOriginCode { get; set; }
            public DateTime? MainCarriageETA { get; set; }
            public Double? GrossWeight { get; set; }
            public string ShipmentStatus { get; set; }
        }

        public class IATAStatisticsGroup
        {
            public string AirlineId { get; set; }
            public string AirlineName { get; set; }
            public string TotalFreight { get; set; }

            public List<IATAStatisticsRecord> InsideGroupList { get; set; }
        }
    }
}