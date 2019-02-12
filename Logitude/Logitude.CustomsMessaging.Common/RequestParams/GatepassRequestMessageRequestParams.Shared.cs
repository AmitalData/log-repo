using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class GatepassRequestMessageRequestParams : RequestParamsBase
    {
        public string MasterCourierId { get; set; }
        public string OriginSiteCode { get; set; }
        public string DesignateSiteCode { get; set; }
        public string UpdateCode { get; set; }
        public string TransportationTypeCode { get; set; }
    }
}
