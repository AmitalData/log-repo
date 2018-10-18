using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class TarrifCharge
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string TarrifHeaderId { get; set; }
        public string CurrencyId { get; set; }
        public string ChargesTypeId { get; set; }
        public string MeasurementId { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? UnitPrice { get; set; }

        [ForeignKey("TarrifHeaderId")]
        public virtual TarrifHeader TarrifHeader { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }
        [ForeignKey("ChargesTypeId")]
        public virtual ChargesType ChargesType { get; set; }
        [ForeignKey("MeasurementId")]
        public virtual Measurement Measurement { get; set; }


    }
}