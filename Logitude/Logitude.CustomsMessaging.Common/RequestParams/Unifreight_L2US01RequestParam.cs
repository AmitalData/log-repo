using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    [XmlRoot(Namespace = "http://amital.com/customs/Prod/LOGISIVUGWithResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/LOGISIVUGWithResponseContentHeader")]
    public class Unifreight_L2US01RequestParam : RequestParamsBase
    {
        
        public string DeclarationId { get; set; }
        public string CFIFILEMFileNo { get; set; }
        public string CCUFILEmFileNo { get; set; }
    }

    
}