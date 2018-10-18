using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class TarrifFromTo
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string TarrifHeaderId { get; set; }
        public string PortId { get; set; }
        public string CountryId { get; set; }
        public string TarrifFromToTypeCode { get; set; }

        [ForeignKey("TarrifHeaderId")]
        public virtual TarrifHeader TarrifHeader { get; set; }
        [ForeignKey("PortId")]
        public virtual Port Port { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
        [ForeignKey("TarrifFromToTypeCode")]
        public virtual TarrifFromToType TarrifFromToType { get; set; }
    }
}