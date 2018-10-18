using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CustomerProductLocationList
    {
        [Key]
        public string CustomerId { get; set; }
        [Key]
        public string ProductTypeCode { get; set; }
        [Key]
        public string CountryId { get; set; }

        public int Tenant { get; set; }

        public decimal? PotentialTEU { get; set; }
        public int? PotentialNumberOfShipments { get; set; }
        public decimal? PotentialChargeableWeight { get; set; }

        public decimal? CommitmentTEU { get; set; }
        public int? CommitmentNumberOfShipments { get; set; }
        public decimal? CommitmentChargeableWeight { get; set; }

        public decimal? PotentialRevenue { get; set; }
        public decimal? CommitmentRevenue { get; set; }
    }
}