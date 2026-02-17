namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class ObjectTableRuleDetails
    {
        public int Tenant { get; set; }
        public string Condition { get; set; }
        public bool SystemLevel { get; set; }
        public string RuleCode { get; set; }
        public string OutputMessage { get; set; }
        public string Name { get; set; }
        public bool InActive { get; set; }
        public string RuleTypeCode { get; set; }
        public string ObjectTableId { get; set; }
        public bool Internal { get; set; }
        public string TriggerTypeCode { get; set; }
        public string RuleNotificationTypeCode { get; set; }
        public string TriggerFieldId { get; set; }
        public bool ActiveForNew { get; set; }
        public bool ActiveForUpdate { get; set; }
        public bool AdvancedCondition { get; set; }
    }
}