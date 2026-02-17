using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class DocumentsMetaDataType
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public bool InActive { get; set; }
        public string CustomsMetaDataCode { get; set; }
        public string Format { get; set; }
    }
}
