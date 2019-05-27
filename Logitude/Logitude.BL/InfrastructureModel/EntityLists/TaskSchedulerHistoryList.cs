using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class TaskSchedulerHistoryList
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
        public string LogType { get; set; }
        public string LogFirstLine { get; set; }
        public double Duration { get; set; }

    }
}
