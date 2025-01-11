using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
