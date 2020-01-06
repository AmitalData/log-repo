using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class ObjectTableRule
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Condition { get; set; }
        public bool SystemLevel { get; set; }
        public string RuleCode { get; set; }
        public string OutputMessage { get; set; }
        public string Name { get; set; }
        public bool InActive { get; set; }
        public string RuleTypeCode { get; set; }
        public string ObjectTableId { get; set; }

        public string TriggerTypeCode { get; set; }
        public string RuleNotificationTypeCode { get; set; }
        public string TriggerFieldId { get; set; }
        public bool ActiveForNew { get; set; }
        public bool ActiveForUpdate { get; set; }
        public bool Internal { get; set; }
        public bool AdvancedCondition { get; set; }
        public string TriggerFieldCode { get; set; }
        //[Include]
        //[Association("ObjectTableRuleObjectTable", "ObjectTableId", "Id", IsForeignKey = true)]

        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }

        //[Include]
        //[Association("ObjectTableRuleObjectField", "TriggerFieldId", "Id", IsForeignKey = true)]
        [ForeignKey("TriggerFieldId")]
        public virtual ObjectField ObjectField { get; set; }

        //[Include]
        //[Association("ObjectTableRuleRuleType", "RuleTypeCode", "Code", IsForeignKey = true)]
        [ForeignKey("RuleTypeCode")]
        public virtual RuleType RuleType { get; set; }

        //[Include]
        //[Association("TriggerTypeObjectTableRule", "TriggerTypeCode", "Code", IsForeignKey = true)]
        [ForeignKey("TriggerTypeCode")]
        public virtual TriggerType TriggerType { get; set; }

        //[Include]
        //[Association("ObjectTableRuleRuleNotificationType", "RuleNotificationTypeCode", "Code", IsForeignKey = true)]
        [ForeignKey("RuleNotificationTypeCode")]
        public virtual RuleNotificationType RuleNotificationType { get; set; }

       // public virtual List<ObjectTableRuleField> ObjectTableRuleFields { get; set; }


 //       public  List<RuleConditionField> RuleConditionFields { get; set; }


    }
}