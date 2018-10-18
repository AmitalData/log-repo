using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{

    public class GuaranteeRequestParams : RequestParamsBase
    {
        public string GuranteeType { get; set; }
        public string FileNumber { get; set; }
        public string Numeral { get; set; }
        
    }
}
