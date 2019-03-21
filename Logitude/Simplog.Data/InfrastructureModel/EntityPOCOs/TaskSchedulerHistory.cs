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
    public class TaskSchedulerHistory
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string RunResult { get; set; }
        public string TaskId { get; set; } 
        public bool IsError { get; set; }
        public DateTime? StartDateTimeUTC { get; set; }
        public DateTime? EndDateTimeUTC { get; set; }
        [ForeignKey("TaskId")]
        public virtual TasksScheduler TaskScheduler { get; set; }
        public string LogType { get; set; }
        public string LogFirstLine { get; set; }

    }
}
