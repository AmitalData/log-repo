
namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class AdvancedFilterDetails
    {
        public int Tenant { get; set; }
        public string QueryId { get; set; }
        public string QueryCode { get; set; }
        public string ObjectFieldId { get; set; }
        public bool IsPredefined { get; set; }
        public string PredefinedValue { get; set; }
        public string PredefinedValue2 { get; set; }
        public string Operator { get; set; }
        public int IndexOrder { get; set; }
        public bool CustomPredefined { get; set; }
        public string ObjectFieldCode { get; set; }
        
    }
}
