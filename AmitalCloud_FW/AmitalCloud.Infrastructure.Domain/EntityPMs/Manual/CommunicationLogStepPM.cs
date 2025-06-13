using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class CommunicationLogStepPM
    {
        [Key]
        public string CommunicationLogId { get; set; }
        [Key]
        public int StepNumber { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public int Retries { get; set; }
        public string Status { get; set; }
        public string Log { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StatusName { get; set; }
        public string DocumentId { get; set; }
    }
}
