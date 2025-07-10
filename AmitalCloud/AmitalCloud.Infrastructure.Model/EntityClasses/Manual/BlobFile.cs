using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class BlobFile
    {
        [Key]
        public string Id { get; set; }
        public byte[] Blob { get; set; }
        public bool IsCompressed { get; set; }
    }
}
