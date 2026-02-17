using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class GlobalDB
    {
        [Key]
        public string Id { get; set; }
        public string DBConnection { get; set; }
        public bool IsUpgrading { get; set; }
        public bool IsActive { get; set; }
        public string SharedDWConnection { get; set; }
        public string SecondaryAzureDBConnection { get; set; }
        public bool IsBlocking { get; set; }
    }
}
