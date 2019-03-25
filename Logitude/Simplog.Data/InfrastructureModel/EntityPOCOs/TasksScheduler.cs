using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class TasksScheduler

    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; } 
        public DateTime CreateDateTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? NextRunTime { get; set; }
        public DateTime? LastRunTime { get; set; }
        public string LastRunResult { get; set; }
        public bool InActive  { get; set; }
        public string ServiceClassName { get; set; }
        public string TriggerType { get; set; } // Daily,mounthly … 
        public bool Satarday  { get; set; }
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public int MonthlyDay { get; set; }
        public DateTime? StartDateTime { get; set; }

        public int RepeatInMinutes { get; set; }

        public bool IsLastRunError { get; set; }

        public string SchedulerDetailsXML { get; set; }
        public string Type { get; set; }
        public DateTime? NextRunTimeUTC { get; set; }
        public DateTime? LastRunTimeUTC { get; set; }
        public DateTime? StartDateTimeUTC { get; set; }


    }
}
