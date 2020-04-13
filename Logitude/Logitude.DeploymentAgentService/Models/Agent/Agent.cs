using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DeploymentAgentService.Models
{
    public class Agent
    {
        public string Id { get; set; }

        public int CurrentVersion { get; set; }

        public int NewVersion { get; set; }

        public ServiceType ServiceType { get; set; }

        public Artifact NewVersionArtifact { get; set; }

        public Customer Customer { get; set; }

        public DeploymentStatus DeploymentStatus { get; set; }
    }
}