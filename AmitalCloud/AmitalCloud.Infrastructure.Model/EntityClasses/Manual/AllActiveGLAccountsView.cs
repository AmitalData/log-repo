using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class AllActiveGLAccountsView
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
    }
}