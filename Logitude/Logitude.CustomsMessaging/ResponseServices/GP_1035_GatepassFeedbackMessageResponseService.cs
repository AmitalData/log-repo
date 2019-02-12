using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.GatepassFeedbackMServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class GP_1035_GatepassFeedbackMessageResponseService : ResponseServiceBase<GatepassFeedbackMessageResponseData, GP_NG_1035_MSG2_GatepassFeedbackMessage, GatepassRequestMessageRequestParams>
    {
        public override GatepassFeedbackMessageResponseData GetResponse(GP_NG_1035_MSG2_GatepassFeedbackMessage customResponse, GatepassRequestMessageRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(GP_NG_1035_MSG2_GatepassFeedbackMessage customResponse, GatepassRequestMessageRequestParams requestParams)
        {
            this.MyResponseData = new GatepassFeedbackMessageResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "שאילתא לדרישת חוקיות לפרט מכס בוצעה בהצלחה";

            if (customResponse.GatepassFeedbackMessage == null || (customResponse.GatepassFeedbackMessage != null && customResponse.GatepassFeedbackMessage.Count() == 0))
            {
                LogMessagingUtil.Instance.AppendLine("GatepassFeedbackMessage is empty");
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "לא התקבלו נתונים מהמכס";
                return;
            }


        }
    }
}
