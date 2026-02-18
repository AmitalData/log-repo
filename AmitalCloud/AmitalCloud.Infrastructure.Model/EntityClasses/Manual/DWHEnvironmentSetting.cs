using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class DWHEnvironmentSetting
    {
        [Key]
        public int Id { get; set; }
        public string FactCodes { get; set; }
    }
}
