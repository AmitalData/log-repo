using System.ComponentModel.DataAnnotations;
using AmitalCloud.Infrastructure.Domain.Enums;
namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class ObjectTableRuleFieldPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool SystemLevel { get; set; }
        public string ObjectFieldId { get; set; }
        public string ObjectTableRuleId { get; set; }
        public string ObjectFieldName { get; set; }
        public string Expression { get; set; }
        public string RuleNotificationTypeCode { get; set; }
        public string ObjectTableRuleCode { get; set; }
        public string ObjectTableRuleTypeCode { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
        public string ObjectFieldCode { get; set; }


    }
}