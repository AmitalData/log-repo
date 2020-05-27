namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class QueryFilterDetails
    {
        public int Tenant { get; set; }
        public string QueryId { get; set; }
        public string QueryCode { get; set; }
        public string ObjectFieldId { get; set; }
        public string ObjectFieldCode { get; set; }
        public int IndexOrder { get; set; }
    }
}