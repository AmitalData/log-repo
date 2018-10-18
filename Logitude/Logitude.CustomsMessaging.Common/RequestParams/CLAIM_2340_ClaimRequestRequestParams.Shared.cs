using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class CLAIM_2340_ClaimRequestRequestParams : RequestParamsBase
    {
        public string AppicationId { get; set; }
        public List<string> ClaimsRelatedEntitiesList { get; set; }
    }
}
