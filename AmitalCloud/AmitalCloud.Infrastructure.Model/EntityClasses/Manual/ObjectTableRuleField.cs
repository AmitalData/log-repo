using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ObjectTableRuleField
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool SystemLevel { get; set; }
        public string ObjectFieldId { get; set; }
        public string ObjectTableRuleId { get; set; }
        public string Expression { get; set; }
        public string RuleNotificationTypeCode { get; set; }
        public string ObjectFieldCode { get; set; }

        //[Include]
        //[Association("ObjectTableRuleFieldObjectField", "ObjectFieldId", "Id", IsForeignKey = true)]
        [ForeignKey("ObjectFieldId")]
        public virtual ObjectField ObjectField { get; set; }

        //[Include]
        //[Association("ObjectTableRuleFieldObjectTableRule", "ObjectTableRuleId", "Id", IsForeignKey = true)]

        [ForeignKey("ObjectTableRuleId")]
        public virtual ObjectTableRule ObjectTableRule { get; set; }


        //[Include]
        //[Association("ObjectTableRuleFieldRuleNotificationType", "RuleNotificationTypeCode", "Code", IsForeignKey = true)]
        [ForeignKey("RuleNotificationTypeCode")]
        public virtual RuleNotificationType RuleNotificationType { get; set; }
    }
}