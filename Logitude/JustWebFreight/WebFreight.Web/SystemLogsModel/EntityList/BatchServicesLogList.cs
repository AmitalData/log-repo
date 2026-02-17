using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.SystemLogsModel.EntityList
{
    public class BatchServicesLogList
    {

        [Key]
        public string Id { get; set; }
        public string BatchServiceCode { get; set; }
        public DateTime? LastActivity { get; set; }
        public decimal? CPU { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? NumberOfDoneItems { get; set; }

        public int DoneItemsInOneHour { get; set; }

        public int DoneItemsInOneMinute { get; set; }

        public int DoneItemsInFiveMinutes { get; set; }
    }
}