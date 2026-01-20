using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
   public class BatchServicesDefinition
    {

        [Key]
        public string Code { get; set; }
        //public bool InActive { get; set; }
        //public int? NumberOfThreads { get; set; }
        public string ClassName { get; set; }
        public string Parameter1 { get; set; }
        public string Parameter2 { get; set; }
        public string QueueDefinitionCode { get; set; }
        public string QueueBase { get; set; }

        public BatchServicesDefinitionMods BatchServicesDefinitionMods { get; set; }

        [Column("UseRabbitMQ")]
        public bool UseRabbitMQ { get; set; }


    }
}
