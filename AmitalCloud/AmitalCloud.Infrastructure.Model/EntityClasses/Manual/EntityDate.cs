using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class EntityDate
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }


    }
}
