using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class AnalyzeQueueStatus
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

        //public List<AnalyzeQueue> AnalyzeQueues { get; set; }
    }

}
