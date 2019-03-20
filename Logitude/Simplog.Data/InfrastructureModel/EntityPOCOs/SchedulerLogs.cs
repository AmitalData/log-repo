using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class SchedulerLogs

    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; } 
        public DateTime CreateDate { get; set; }
        public string Log { get; set; }
        public string HistoryId { get; set; }

        [ForeignKey("HistoryId")]
        public virtual TaskSchedulerHistory TaskSchedulerHistory { get; set; }

    }
}
