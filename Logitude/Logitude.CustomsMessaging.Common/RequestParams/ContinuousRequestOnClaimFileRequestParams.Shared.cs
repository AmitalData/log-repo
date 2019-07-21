using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class ContinuousRequestOnClaimFileRequestParams : RequestParamsBase
    {
        public string AppicationId { get; set; }
        public string ClaimRelatedEntityCounterKey { get; set; }
    }
}
