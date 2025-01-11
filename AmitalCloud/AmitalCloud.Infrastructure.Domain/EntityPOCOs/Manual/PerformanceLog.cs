using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class PerformanceLog
    {
        [Key]
        public string Id { get; set; }
        public DateTime LogDateTimeGMT { get; set; }
        public DateTime LogDateTimeLocal { get; set; }
        public string Email { get; set; }
        public string ModelName { get; set; }
        public string MethodName { get; set; }
        public bool MonitoringService { get; set; }
        public int ExecutionTime { get; set; }
        public string UserIP { get; set; }
        public string MethodParameters { get; set; }
        public int Tenant { get; set; }
        public int ServerTime { get; set; }
       
       
        
    }
}