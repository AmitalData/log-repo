using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class ObjectTableTypePM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}