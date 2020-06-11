using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DeploymentAgentService.Models
{
    public class ProcessRunResult
    {
        public ProcessResult ProcessResult { get; set; }
        public string Exception { get; set; }
        public string ExceptionMessage { get; set; }
    }
}