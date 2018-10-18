using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class EAWBsDataProvider : BaseDataProvider
    {
        [Key]
        public int Id { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public List<AWBRecord> AWBsRecordList { get; set; }
    }

    public class AWBRecord
    {
        [Key]
        public int Id { get; set; }
        public string CarrierCode { get; set; }
        public string MessageType { get; set; }
        public string Prefix { get; set; }
        public string AWBNumber { get; set; }
        public string HouseNumber { get; set; }
        public DateTime? SentDate { get; set; }
        public string Participant { get; set; }
        public string User { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int? NumberOfPieces { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? ChargeableWeight { get; set; }
        public string WeightUnitCode { get; set; }
        public decimal? Volume { get; set; }
        public string NatureOfGoods { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string Flight1 { get; set; }
        public DateTime? FlightDate1 { get; set; }
        public string Flight2 { get; set; }
        public DateTime? FlightDate2 { get; set; }
        public string Flight3 { get; set; }
        public DateTime? FlightDate3 { get; set; }
    }
}