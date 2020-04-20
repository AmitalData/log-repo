using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DeploymentAgentService.Models
{
    public class Artifact
    {
        public int Id { get; set; }

        public string FtpUrl { get; set; }

        public string FtpUsername { get; set; }

        public string FtpPassword { get; set; }

        public string FolderName { get; set; }

        public string FileName { get; set; }
    }
}