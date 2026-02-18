using AmitalCloud.Infrastructure.Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class RuleConditionFieldPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableRuleId { get; set; }
        public string ObjectFieldId { get; set; }
        public string Value { get; set; }
        public string Operator { get; set; }
        public string ObjectFieldCode { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }

        public string ObjectFieldName { get; set; }
    }
}
