using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CustomerProductLocation
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

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("ProductTypeCode")]
        public virtual ProductType ProductType { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
    }
}
