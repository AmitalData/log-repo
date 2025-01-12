using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class SessionPolicy
    {
        [Key]
        public string Id { get; set; }
        public int WebTokenLifeTimeInMinutes { get; set; }
        public int WebTokenExpirationWarningInMinutes { get; set; }
    }
}
