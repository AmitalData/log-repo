using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ColorIndex
    {

        [Key]
        public int IndexNumber { get; set; }

        public string Color { get; set; }
    }
}
