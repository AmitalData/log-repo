namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class ScreenFieldDetails
    {
        
        public int Tenant { get; set; }

        public int Column { get; set; }
        public int Row { get; set; }

        public string ScreenId { get; set; }

        // public string FieldName { get; set; }
        public string ObjectFieldId { get; set; }
        public string ObjectFieldCode { get; set; }


    }
}