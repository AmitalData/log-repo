using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class DataProvider
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

    }
}
