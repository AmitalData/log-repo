using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityLists
{
    public partial class CommunicationLogStepList
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
        public string DocumentId { get; set; }
        public string StatusName { get; set; }
        public string DocumentData { get; set; }
    }
}
