using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitude.Customs.BL.Messaging.Customs
{
    public class SendSheetSignModel
    {

        public byte[] CustomRequestSignedByteArryPasiveSign { get; set; }

        public string CurrentSignCertificateName { get; set; }

        public int Tenant { get; set; }

        public string CustomsRequestsSheetId { get; set; }

        
    }
}
