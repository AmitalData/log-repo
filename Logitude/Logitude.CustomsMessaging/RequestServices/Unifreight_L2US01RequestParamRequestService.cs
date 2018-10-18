using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class Unifreight_L2US01RequestParamRequestService
        :RequestServiceBase
        <Unifreight_L2US01RequestParam, Unifreight_L2US01RequestParam>
    {
        public override Unifreight_L2US01RequestParam GetRequest(Unifreight_L2US01RequestParam requestParams)
        {
            return requestParams;
        }
    }
}
