using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SealUpdateServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class SE_6001_SealUpdateResponseService : ResponseServiceBase<CustomItemLegalDemandsResponseData, INF_MSG_Generic, CargoSealsRequestParams>
    {
        public override CustomItemLegalDemandsResponseData GetResponse(INF_MSG_Generic customResponse, CargoSealsRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(INF_MSG_Generic customResponse, CargoSealsRequestParams requestParams)
        {
            throw new NotImplementedException();
        }
    }
}
