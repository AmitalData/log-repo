using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class SendALLStorageSiteRequestParams : RequestParamsBase
    {
        public string CourierMasterId { get; set; }
        public string HAWB { get; set; }
        public string StorageSiteCode { get; set; }

        public string UnLoadPortCode { get; set; }
    }
}


