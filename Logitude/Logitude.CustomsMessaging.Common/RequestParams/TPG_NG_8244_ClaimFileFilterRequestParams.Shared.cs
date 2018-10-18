using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class TPG_NG_8244_ClaimFileFilterRequestParams : RequestParamsBase
    {
        public string FileNumber { get; set; }
        public int? Numeral { get; set; }
        public string FillingNumber { get; set; }
    }
}
