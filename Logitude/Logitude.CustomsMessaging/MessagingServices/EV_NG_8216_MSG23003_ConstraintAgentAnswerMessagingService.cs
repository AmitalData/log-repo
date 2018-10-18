using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;
using UnifreightIIG.Common.ConstraintAgentAnswerServiceReference;
using Simplog.Data.InfrastructureModel.Repositories;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class EV_NG_8216_MSG23003_ConstraintAgentAnswerMessagingService
        : MessagingServiceBase<
        ConstraintAgentObjectionRequestParams, ConstraintAgentAnswerResponseData,
        EV_NG_8216_MSG23003_ConstraintAgentAnswer, INF_MSG_Generic,
        EV_NG_8216_MSG23003_ConstraintAgentAnswerRequestService, INF_MSG_Generic_EV_NG_8216_MSG23003_ConstraintAgentAnswerResponseService, 
        RequestHeader>
    {

        protected override INF_MSG_Generic CallWS(EV_NG_8216_MSG23003_ConstraintAgentAnswer customRequest, ConstraintAgentObjectionRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IConstraintAgentAnswerOperation>()
                    .ConstraintAgentAnswerOperation(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }

        protected override ConstraintAgentObjectionRequestParams CreateDefaultRequestParamsFromCustomsResponse(INF_MSG_Generic customsResponse)
        {
             var tableName = "Customs.Declaration";

             var myGenericRequestParams = new ConstraintAgentObjectionRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }
        public override string MainInterfaceCode
        {
            get { return "8216"; }
        }
    }
}
