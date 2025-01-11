using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class QueueDefinition
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public bool DuplicateMessagesAutoRemove { get; set; }
    }
}
