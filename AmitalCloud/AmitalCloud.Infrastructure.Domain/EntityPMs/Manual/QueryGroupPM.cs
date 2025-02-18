using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class QueryGroupPM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public int IndexOrder { get; set; }
    }
}