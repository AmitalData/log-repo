using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class QuotePackage
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PackageTypeId { get; set; }
        public string QuoteId { get; set; }
        public int? Quantity { get; set; }
        public double? GrossWeight { get; set; }
        public double? Volume { get; set; }
        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? VolumetricWeight { get; set; }

        [ForeignKey("PackageTypeId")]
        public virtual PackageType PackageType { get; set; }

        [ForeignKey("QuoteId")]
        public Quote Quote { get; set; }
    }
}
