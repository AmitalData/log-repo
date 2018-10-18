using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ConstraintAgentAnswerServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class INF_MSG_Generic_EV_NG_8216_MSG23003_ConstraintAgentAnswerResponseService
        : ResponseServiceBase<
        ConstraintAgentAnswerResponseData,
        INF_MSG_Generic,
        ConstraintAgentObjectionRequestParams>
    {
        public override void Update(INF_MSG_Generic customResponse, ConstraintAgentObjectionRequestParams requestParams)
        {

            this.MyResponseData = new ConstraintAgentAnswerResponseData();
            this.MyResponseData.ApplicationID = requestParams.ConstraintNumber;
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;

            return;

        }

        public override ConstraintAgentAnswerResponseData GetResponse(INF_MSG_Generic customResponse, ConstraintAgentObjectionRequestParams requestParams)
        {
            if (this.MyResponseData.Succeeded)
            {
                SendDeclarationStatus(customResponse, requestParams); // moran 30.12.14 - Task 9793
            }
            return this.MyResponseData;
        }

        public void SendDeclarationStatus(INF_MSG_Generic customResponse, ConstraintAgentObjectionRequestParams requestParams) // moran 30.12.14 - Task 9793
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var declarationQueryService = new DeclarationQueryService(dbContext);
            DeclarationPM declarationPM = declarationQueryService.GetSingle(requestParams.DeclarationId, false, false);
            if (declarationPM != null)
            {
                DeclarationStatusRequestParams searchParams = new DeclarationStatusRequestParams()
                {
                    LoggingEnabled = true,

                    CustomFileNo = declarationPM.CustomFileNo,
                    DeclarationNumber = declarationPM.DeclarationNumber,
                    Tenant = declarationPM.Tenant,
                    RequestName = "Declaration Status Search",
                    ResponseName = "Declaration Status Search",
                    SuppressSplitWR = true
                };

                searchParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
                var resData = Logitude.CustomsMessaging.MessagingServices.DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService.SendInteractive(searchParams);
                if (!resData.Succeeded)
                {
                    LogMessagingUtil.Instance.AppendLine("Request Failed " + resData.CustomsRequestsSheetId + ", Message: " + resData.UserMessage);
                    return;
                }
                LogMessagingUtil.Instance.AppendLine("Request Succeeded " + resData.CustomsRequestsSheetId);

            }


        }
    }
}
