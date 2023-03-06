using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class DeploymentPackageExecutionLogPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CreatedByUserId { get; set; }

        public string StatusCode { get; set; }
        public string ExceptionMessage { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? DoneDate { get; set; }
        public string RequestXML { get; set; }

        public int RetryNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public string Logs { get; set; }
        public string Subject { get; set; }
        public string ExecutedByServerName { get; set; }

    }
}
