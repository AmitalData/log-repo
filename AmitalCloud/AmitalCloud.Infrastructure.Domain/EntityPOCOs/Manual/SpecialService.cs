using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class SpecialService
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public int Tenant { get; set; }
        public string SpecialServiceEnglishName { get; set; }
        public string SpecialServiceLocalName { get; set; }
        public bool InActive { get; set; }
    }
}