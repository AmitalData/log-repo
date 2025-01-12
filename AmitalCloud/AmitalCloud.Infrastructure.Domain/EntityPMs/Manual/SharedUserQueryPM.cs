using AmitalCloud.Infrastructure.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class SharedUserQueryPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string UserId { get; set; }
        public string QueryId { get; set; }
        public string QueryCode { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
