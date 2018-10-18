using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.StorageEntranceUnloadingServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class ST_MSG05_StorageEntranceUnloadingResponseService : ResponseServiceBase<StorageEntranceUnloadingResponseData, INF_MSG_Generic, StorageEntranceUnloadingRequestParams>
    {
        public override StorageEntranceUnloadingResponseData GetResponse(INF_MSG_Generic customResponse, StorageEntranceUnloadingRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(INF_MSG_Generic customResponse, StorageEntranceUnloadingRequestParams requestParams)
        {
            this.MyResponseData = new StorageEntranceUnloadingResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "שליחת מסר זמיניות בוצעה בהצלחה";
        }
    }
}
