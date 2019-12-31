using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Feature
    {
        [Key]
        public string Id { get; set; }
        [Index(IsUnique = true)]
        public string FeatureUniqeCode { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string ObjectTableId { get; set; }
        public string NameTextCodeId { get; set; }
        public string FeatureTypeCode { get; set; }
        public bool Packagable { get; set; }
        public bool IsBusinessUnitEnabled { get; set; }
        public bool IsOld { get; set; }
        public bool IsCoreFeature  { get; set; }
        public string ToggleCode { get; set; }
        

        [ForeignKey("FeatureTypeCode")]
        public FeatureType FeatureType { get; set; }

        [ForeignKey("NameTextCodeId")]
        public TextCode NameTextCode { get; set; }
         
        [ForeignKey("ObjectTableId")]
        public ObjectTable ObjectTable { get; set; }
    }
}