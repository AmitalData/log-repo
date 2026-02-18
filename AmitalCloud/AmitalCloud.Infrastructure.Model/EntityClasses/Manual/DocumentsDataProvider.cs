using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class DocumentsDataProvider
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
