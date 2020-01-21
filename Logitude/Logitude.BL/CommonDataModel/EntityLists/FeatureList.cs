using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class FeatureList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string ObjectTableId { get; set; }
        public string NameTextCodeId { get; set; }
        public string NameTextCodeCode { get; set; }
        public string FeatureTypeCode { get; set; }
        public bool Packagable { get; set; }
        public bool IsBusinessUnitEnabled { get; set; }
        public bool IsOld { get; set; }
        public bool IsCoreFeature { get; set; }
        public string FeatureUniqeCode { get; set; }

        // Dummy
        public string Name { get; set; }
        public string ObjectTableName { get; set; }
        public string FeatureTypeName { get; set; }
    }
}