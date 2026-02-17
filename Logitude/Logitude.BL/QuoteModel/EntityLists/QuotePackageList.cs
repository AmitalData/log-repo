using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.QuoteModel.EntityLists
{
    public class QuotePackageList
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
        public string PackageTypeName { get; set; }
    }
}