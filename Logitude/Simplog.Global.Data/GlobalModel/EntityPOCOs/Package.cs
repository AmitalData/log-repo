using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
    public class Package
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }        
        public bool InActive { get; set; }
        public string FeaturePackageTypeCode { get; set; }

        [ForeignKey("FeaturePackageTypeCode")]
        public FeaturePackageType FeaturePackageType { get; set; }
    }
}