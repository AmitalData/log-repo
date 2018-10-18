using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.SystemLogs.POCOs
{
   public class BatchServicesLog
    {

       [Key]
       public string Id { get; set; }
       public string BatchServiceCode { get; set; }
       public DateTime? LastActivity { get; set; }
       public decimal? CPU { get; set; }
       public DateTime? CreateDate { get; set; }
       public int? NumberOfDoneItems { get; set; }
       public int DoneItemsInOneMinute { get; set; }
       public int DoneItemsInFiveMinutes { get; set; }
       public int DoneItemsInOneHour { get; set; }
    }
}
