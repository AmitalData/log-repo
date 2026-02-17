using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.EntityPMs
{
   public class BatchServicesDefinitionPM
    {

        [Key]
        public string Code { get; set; }
        public bool InActive { get; set; }
        public int? NumberOfThreads { get; set; }
        public string ClassName { get; set; }
        public string Parameter1 { get; set; }
        public string Parameter2 { get; set; }
        public decimal? CPU { get; set; }
        public int NumberOfDoneItems { get; set; }
        public DateTime? LastActivity { get; set; }
        public int DoneItemsInOneHour { get; set; }

        public int DoneItemsInOneMinute { get; set; }

        public int DoneItemsInFiveMinutes { get; set; }
        public int WaitingItems { get; set; }
        public int FailedItems { get; set; }
        public string QueueDefinitionCode { get; set; }
        public int MaxWorkingTimeInMinutes { get; set; }

    }
}
