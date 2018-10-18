using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class TarrifStep
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string TarrifHeaderId { get; set; }
        public decimal? Step { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? UnitPrice { get; set; }

        [ForeignKey("TarrifHeaderId")]
        public virtual TarrifHeader TarrifHeader { get; set; }

    }
}