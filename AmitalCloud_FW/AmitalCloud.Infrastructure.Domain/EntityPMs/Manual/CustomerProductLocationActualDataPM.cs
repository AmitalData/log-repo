using AmitalCloud.Infrastructure.Domain.BaseClasses;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class CustomerProductLocationActualDataPM : BaseEntityPM
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


        public override int Tenant { get; set; }

        public decimal? TEU { get; set; }

        public int? NumberOfShipments { get; set; }

        public decimal? ChargeableWeight { get; set; }


        public decimal? Revenue { get; set; }

        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}