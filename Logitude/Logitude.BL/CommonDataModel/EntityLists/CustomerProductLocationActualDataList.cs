using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CustomerProductLocationActualDataList
    {
        [Key]
        public string CustomerId { get; set; }
        [Key]
        public string ProductTypeCode { get; set; }
        [Key]
        public int Month { get; set; }
        [Key]
        public int Year { get; set; }
        [Key]
        public string CountryId { get; set; }

        public int Tenant { get; set; }
        public decimal? TEU { get; set; }
        public int? NumberOfShipments { get; set; }
        public decimal? ChargeableWeight { get; set; }

        public decimal? Revenue { get; set; }
    }
}