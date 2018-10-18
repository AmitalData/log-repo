//Yuval Chalup 18.06.2015 TASK-13858
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageToAgentServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class INF_MSG_Generic_DOC_NG_5101_GNMessageToAgent_ResponseService
        : ResponseServiceBase<
        MessageToAgentResponseData,
        INF_MSG_Generic,
        MessageToAgentRequestParams>
    {
        public override void Update(INF_MSG_Generic customResponse, MessageToAgentRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var myNotificationQueryService = new NotificationQueryService(dbContext);
            var myNotificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);

            this.MyResponseData = new MessageToAgentResponseData();

            if (string.IsNullOrWhiteSpace(requestParams.NotificationId))
            {
                MyResponseData.Succeeded = false;
                MyResponseData.HasException = true; 
                LogMessagingUtil.Instance.AppendLine("No requestParams.NotificationId");
                return;
            }

            NotificationPM myNotificationPM = myNotificationQueryService.GetSingle(requestParams.NotificationId, true, false);
            if (myNotificationPM == null)
            {
                MyResponseData.Succeeded = false;
                MyResponseData.HasException = true; 
                LogMessagingUtil.Instance.AppendLine("Can not find Notification" + requestParams.NotificationId);
                return;
            }

            myNotificationPM.ChangeSetOp = ChangeSetOperation.Update;
            myNotificationUpdateService.Update(myNotificationPM, true);

            MyResponseData.HasException = false;
            MyResponseData.Succeeded = true;

        }

        public override MessageToAgentResponseData GetResponse(INF_MSG_Generic customResponse, MessageToAgentRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
