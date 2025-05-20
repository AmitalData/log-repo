using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class GetCustomItemClassifGuidanceRequestParams : RequestParamsBase
    {
        public int CustomItemId { get; set; }
        public DateTime ValidToDate { get; set; } 
    }
}
