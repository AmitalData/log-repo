namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class TextCodeDetails
    {
        public string Code { get; set; }
        public string DefaultText { get; set; }
        public string ObjectTableId { get; set; }
        public string TextCodeTypeCode { get; set; }
      
        public int Tenant { get; set; }
        public string DefaultTextPlural { get; set; }
        public bool InActive { get; set; }
        public string LocalDefaultText { get; set; }
        public string LocalDefaultTextBack_up { get; set; }

        public bool IsMemoryAdded { get; set; } // dont fill
        public bool IsSpellChecked { get; set; }
    }
}