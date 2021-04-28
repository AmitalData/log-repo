using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationCorrection
{
   public class General
    {

       public DateTime? IssueDateTime { get; set; }
       public string VersionId { get; set; }

       public List<Additional> AdditionalInformation { get; set; }

       public List<Entity> Amendments { get; set; }

        public List<error> SystemMessages { get; set; }

        public List<Reference> References { get; set; }

    }
}
