using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class EntityLastActivityType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

        // public List<EntityLastActivity> EntityLastActivities { get; set; }
    }
}
