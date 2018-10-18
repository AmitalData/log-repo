using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class GuaranteeCertificateRequestParams : GenericRequestParams
    {
        //public string CustomsFile { get; set; }
        //public string DeclarationNumber { get; set; }
        //public string DeclarationId { get; set; }
        public int? certificateID { get; set; }
        public bool certificateIDSpecified { get; set; }
        public int? guaranteeCertificateType { get; set; }
        public bool guaranteeCertificateTypeSpecified { get; set; }
        public string guaranteeExternalCertificateNumber { get; set; }
        public int? guarantorID { get; set; }
        public bool guarantorIDSpecified { get; set; }        
    }
}
