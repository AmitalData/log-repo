using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
