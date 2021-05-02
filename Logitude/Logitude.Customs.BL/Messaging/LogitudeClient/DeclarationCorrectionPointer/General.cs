using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationCorrectionPointer
{
    public class General
    {
        public string IssueDateTime { get; set; }
        public string VersionId { get; set; }
        public List<Additional> AdditionalInformation { get; set; }
        public List<Entity> Amendments { get; set; }
        public List<error> SystemMessages { get; set; }
        public List<Reference> References { get; set; }
    }
}
