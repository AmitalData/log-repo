using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DeploymentAgentService.Models
{
    public class SaveAgent
    {
        public string ServiceTypeCode { get; set; }

        public int CurrentVersion { get; set; }

        public int NewVersion { get; set; }

        public int NewVersionArtifactId { get; set; }
    }
}
