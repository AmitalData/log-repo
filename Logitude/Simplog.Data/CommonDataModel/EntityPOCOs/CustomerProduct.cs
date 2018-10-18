using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CustomerProduct
    {
        [Key]
        [ForeignKey("Customer")]
        public string CustomerId { get; set; }

        [Key]
        [ForeignKey("ProductType")]
        public string ProductTypeCode { get; set; }

        public int Tenant { get; set; }
        public string Notes { get; set; }

        public decimal? PotentialChargeableWeight { get; set; }
        public decimal? CommitmentChargeableWeight { get; set; }

        public decimal? PotentialTEU { get; set; }
        public decimal? CommitmentTEU { get; set; }

        public int? PotentialNumberOfShipments { get; set; }
        public int? CommitmentNumberOfShipments { get; set; }

        public decimal? PotentialRevenue { get; set; }
        public decimal? CommitmentRevenue { get; set; }

        public DateTime? LastShipmentDate { get; set; }

        public string PrepaidCollectId { get; set; }

        //[ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        //ForeignKey("ProductTypeCode")]
        public virtual ProductType ProductType { get; set; }

        public virtual PrepaidCollect PrepaidCollect { get; set; }

        public bool NotesRightToLeft { get; set; }
    }
}
