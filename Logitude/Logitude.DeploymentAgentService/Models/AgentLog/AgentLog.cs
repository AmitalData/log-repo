using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DeploymentAgentService.Models
{
    public class AgentLog
    {
        public int Id { get; set; }

        public string LogMessage { get; set; }

        public bool IsException { get; set; }

        public DateTime LogDatetime { get; set; }

        public Agent Agent { get; set; }
    }
}
