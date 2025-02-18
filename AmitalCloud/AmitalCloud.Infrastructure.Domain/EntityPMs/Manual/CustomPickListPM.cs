using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class CustomPickListPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public bool IsMultipleChoice { get; set; }
        public bool IsDirty { get; set; }

    }
}