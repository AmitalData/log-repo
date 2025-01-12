using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class BatchServicesDefinitionMods
    {

        [Key]
        public string Code { get; set; }
        public bool InActive { get; set; }
        public int? NumberOfThreads { get; set; }
        //public string Parameter1 { get; set; }
        //public string Parameter2 { get; set; }
        public BatchServicesDefinition BatchServicesDefinition { get; set; }

    }
}
