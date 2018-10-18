namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class EntityStatusDetails
    {
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string ObjectTableId { get; set; }
        public int StatusWeight { get; set; }
        public string Code { get; set; }
        public string OldCode { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public string DisplayName { get; internal set; }
    }
}
