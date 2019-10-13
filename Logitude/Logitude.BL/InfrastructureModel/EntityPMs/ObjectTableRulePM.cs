using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class ObjectTableRulePM
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
        public string RuleTypeName { get; set; }
        public string RuleNotificationTypeCode { get; set; }
        public string TriggerTypeCode { get; set; }
        public string TriggerFieldId { get; set; }
        public bool ActiveForNew { get; set; }
        public bool ActiveForUpdate { get; set; }
        public bool Internal { get; set; }
        public bool AdvancedCondition { get; set; }

        public bool IsCreatedFromSystemRule { get; set; }


        private List<RuleConditionFieldPM> ruleConditionFields;
        [Include]
        [Composition]
        [Association("ObjectTableRuleRuleConditionFieldPM", "Id", "ObjectTableRuleId")]
        public virtual List<RuleConditionFieldPM> RuleConditionFields
        {
            get
            {
                if (ruleConditionFields == null)
                {
                    ruleConditionFields = new List<RuleConditionFieldPM>();
                }
                return ruleConditionFields;
            }
            set
            {
                if (value != null)
                {
                    ruleConditionFields = value;
                }
            }
        }
    }
}