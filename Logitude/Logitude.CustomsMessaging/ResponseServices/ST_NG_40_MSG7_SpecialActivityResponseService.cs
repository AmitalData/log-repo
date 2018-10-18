//Yuval Chalup 25.06.2015 TASK-8907
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SpecialActivityRequestMessageServiceReference;


namespace Logitude.CustomsMessaging.ResponseServices
{
    public class ST_NG_40_MSG7_SpecialActivityResponseService : ResponseServiceBase
       <INF_MSG_GenericResponseData, INF_MSG_Generic, SpecialActivityRequestParams>
    {
        public override void Update(INF_MSG_Generic customResponse, SpecialActivityRequestParams requestParams)
        {
            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "בקשה לפעולה מיוחדת נשלחה בהצלחה";

            //Checking foe Exceptions
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData.HasException = true;
                if (customResponse.ResponseContentHeader.Exception != null)
                {
                    var errMess = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                    LogMessagingUtil.Instance.AppendLine(errMess);
                    this.MyResponseData.UserMessage = errMess;
                }
            }

            LogMessagingUtil.Instance.AppendLine("Special activity request Succeeded");
        }

        public override INF_MSG_GenericResponseData GetResponse(INF_MSG_Generic customResponse, SpecialActivityRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}

