using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class PackageList
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public bool InActive { get; set; }
        public string FeaturePackageTypeCode { get; set; }
        public string FeaturePackageTypeName { get; set; }
        public string SearchFields { get; set; }
    }
}