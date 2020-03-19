namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class ObjectTableRuleFieldDetails
    {
        public int Tenant { get; set; }
        public bool SystemLevel { get; set; }
        public string ObjectFieldId { get; set; }
        public string ObjectTableRuleId { get; set; }
        public string Expression { get; set; }
        public string RuleNotificationTypeCode { get; set; }
        public string ObjectFieldCode { get; set; }
    }
}