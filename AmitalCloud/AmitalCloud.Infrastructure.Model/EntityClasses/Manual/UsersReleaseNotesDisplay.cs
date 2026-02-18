using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class UsersReleaseNotesDisplay
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
