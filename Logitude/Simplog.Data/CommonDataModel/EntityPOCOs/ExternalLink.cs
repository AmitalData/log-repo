using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class ExternalLink
    {
        [Key]
        public string Id { get; set; }
        public string Ref { get; set; }
        public string Link { get; set; }
        public int ExpirationDate { get; set; }
        public bool ActivityLog { get; set; }
        public string Params { get; set; }
        public int Tenant { get; set; }
    }
}
