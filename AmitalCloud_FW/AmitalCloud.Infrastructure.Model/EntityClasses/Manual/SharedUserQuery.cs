using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class SharedUserQuery
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string UserId { get; set; }
        public string QueryId { get; set; }
        public string QueryCode { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("QueryId")]
        public Query Query { get; set; }
    }
}
