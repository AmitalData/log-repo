using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CustomerProductList
    {
        [Key]
        public string CustomerId { get; set; }

        [Key]
        public string ProductTypeCode { get; set; }

        public int Tenant { get; set; }        
        public string Notes { get; set; }                
        public decimal? PotentialChargeableWeight { get; set; }        
        public decimal? CommitmentChargeableWeight { get; set; }        
        public decimal? PotentialTEU { get; set; }        
        public decimal? CommitmentTEU { get; set; }        
        public int? PotentialNumberOfShipments { get; set; }        
        public int? CommitmentNumberOfShipments { get; set; }
        public string CustomerName { get; set; }
        public string ProductTypeName { get; set; }
        public decimal? PotentialRevenue { get; set; }
        public decimal? CommitmentRevenue { get; set; }
        public DateTime? LastShipmentDate { get; set; }

        public bool NotesRightToLeft { get; set; }
    }
}