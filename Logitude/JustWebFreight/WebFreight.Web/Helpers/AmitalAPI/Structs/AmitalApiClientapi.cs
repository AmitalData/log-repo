using System;

namespace WebFreight.Web.Helpers.AmitalAPI.Structs
{
    public class AmitalApiClientapi
    {
        public string Id { get; set; }
        public string PartnerToken { get; set; }
        public string PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string SchemaId { get; set; }
        public string SchemaName { get; set; }
        public int ChunkSize { get; set; }
        public object Priority { get; set; }
        public bool SaveAsXml { get; set; }
        public object WebhookAddress { get; set; }
        public object WebhookAuthType { get; set; }
        public object WebhookHeaderParams { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Active { get; set; }
        public object MoreParams { get; set; }
    }
}
