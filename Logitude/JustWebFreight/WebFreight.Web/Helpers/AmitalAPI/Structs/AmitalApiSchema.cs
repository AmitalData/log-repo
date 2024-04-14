using System;

namespace WebFreight.Web.Helpers.AmitalAPI.Structs
{
    public class AmitalApiSchema
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SchemaJson { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Ref3 { get; set; }
        public string Ref4 { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Active { get; set; }
        public string Endpoint { get; set; }
        public string SchemaType { get; set; }
        public bool? SaveAsXml { get; set; }
        public int ChunkSize { get; set; }
        public string[] Tenants { get; set; }
    }
}
