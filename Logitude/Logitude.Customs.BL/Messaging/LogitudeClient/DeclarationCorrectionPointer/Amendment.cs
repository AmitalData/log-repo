using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationCorrectionPointer
{
    public class Amendment
    {
        public string ChangeReasonCode { get; set; }
        public string ChangeReasonName { get; set; }
        public Entity Pointer { get; set; }
    }
}
