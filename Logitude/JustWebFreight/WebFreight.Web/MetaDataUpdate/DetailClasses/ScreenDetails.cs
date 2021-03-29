namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class ScreenDetails
    {
        
        public string Code { get; set; }

        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }
        public string Name { get; set; }
        public string ObjectTableId { get; set; }
        // public virtual List<User> Users { get; set; }
        public int Tenant { get; set; }

        public bool IsReadOnly { get; set; }

        
    }
}