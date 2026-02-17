using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class RuleConditionField
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableRuleId { get; set; }
        public string ObjectFieldId { get; set; }
        public string Value { get; set; }
        public string Operator { get; set; }
       

        ////[Include]
        ////[Association("RuleConditionFieldsObjectTableRule", "ObjectTableRuleId", "Id", IsForeignKey = true)]

        [ForeignKey("ObjectTableRuleId")]
        public virtual ObjectTableRule ObjectTableRule { get; set; }

        ////[Include]
        ////[Association("RuleConditionFieldsObjectField", "ObjectFieldId", "Id", IsForeignKey = true)]

        [ForeignKey("ObjectFieldId")]
        public virtual ObjectField ObjectField { get; set; }
    }
}