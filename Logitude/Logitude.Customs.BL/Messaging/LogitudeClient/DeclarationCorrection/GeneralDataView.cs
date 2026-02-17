using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationCorrection
{
   public class GeneralDataView
    {
       public DateTime? CorrectionDate { get; set; }
       public string Version { get; set; }

       public List<AdditionalInformationView> AdditionalInformation { get; set; }
       public List<AmendmentView> AmendmentViews { get; set; }
        public List<error> SystemMessageViews { get; set; }
    }
}
