using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
    public class RoleFeature :ICloneable
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string RoleId { get; set; }
        public string FeatureId { get; set; }
        public string FeatureAccessLevelCode  { get; set; }
        public bool IsDeleted { get; set; }
        public string FeatureUniqeCode { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        //[ForeignKey("FeatureId")]
        public Feature Feature { get; set; }

        [ForeignKey("FeatureAccessLevelCode")]
        public FeatureAccessLevel FeatureAccessLevel { get; set; }

        public object Clone() => new RoleFeature { Id = this.Id, Tenant = Tenant, Feature = Feature, FeatureAccessLevel = FeatureAccessLevel, FeatureAccessLevelCode =FeatureAccessLevelCode, FeatureId =FeatureId , FeatureUniqeCode =FeatureUniqeCode , IsDeleted =IsDeleted , Role =Role , RoleId=RoleId};


    }
}