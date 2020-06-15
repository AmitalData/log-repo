using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DeploymentAgentService.Models
{
    public class ProcessResult
    {
        public int ExitCode { get; set; }
        public string OutputDataReceived { get; set; }
        public string ErrorDataReceived { get; set; }
    }
}