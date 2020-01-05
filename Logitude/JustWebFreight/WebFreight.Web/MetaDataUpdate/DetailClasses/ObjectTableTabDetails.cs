namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class ObjectTableTabDetails
    {
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string ControlPath { get; set; }
        public string TabNameTextCodeId { get; set; }
        
        public int IndexOrder { get; set; }
        public string Code { get; set; }
        public string FeatureId { get; set; }

        public string HtmlComponentName { get; set; }
        public string HtmlComponentUrl { get; set; }
        public string TabNameTextCodeCode { get; set; }
    }
}