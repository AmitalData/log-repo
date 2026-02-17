using AmitalCloud.Infrastructure.Domain.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class GlobalContact : BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public int GlobalTenantId { get; set; }
        public string Email { get; set; }

        public bool InActive { get; set; }
        public bool IsUser { get; set; }
        public bool InternetAccess { get; set; }
        [Include]
        [Association("GlobalTenantGlobalContact", "GlobalTenantId", "Id", IsForeignKey = true)]
        [ForeignKey("GlobalTenantId")]
        public virtual GlobalTenant GlobalTenant { get; set; }
    }
}