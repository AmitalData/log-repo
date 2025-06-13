using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class QueryGroup
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public int IndexOrder { get; set; }

        //public List<Query> Queries { get; set; }
    }
}