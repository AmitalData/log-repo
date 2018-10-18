namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class RuleConditionFieldDetails
    {
        public int Tenant { get; set; }
        public string ObjectTableRuleId { get; set; }
        public string ObjectFieldId { get; set; }
        public string Value { get; set; }
        public string Operator { get; set; }
    }
}