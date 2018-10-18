

using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.ConstraintApprovalRequestServiceReference;
using UnifreightIIG.Common.TheGateway;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class EV_NG_8214_MSG23001_ConstraintApprovalRequestMessagingService : MessagingServiceBase<
        ConstraintApprovalRequestParams,
        //ConstraintApprovalListRequestParams, // to check- if getting a list or only one constraint
        INF_MSG_GenericResponseData,
        EV_NG_8214_MSG23001_ConstraintApprovalRequest,
        INF_MSG_Generic,
        EV_NG_8214_MSG23001_ConstraintApprovalRequestRequestService,
        EV_NG_8214_MSG23001_ConstraintApprovalResponseService, RequestHeader>
    {

        public override string MainInterfaceCode { get { return "8214"; } }

        protected override ConstraintApprovalRequestParams CreateDefaultRequestParamsFromCustomsResponse(INF_MSG_Generic customsResponse)
        {
            var tableName = "Customs.Declaration";

            var myGenericRequestParams = new ConstraintApprovalRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }
        protected override INF_MSG_Generic CallWS(EV_NG_8214_MSG23001_ConstraintApprovalRequest customRequest, ConstraintApprovalRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IConstraintApprovalRequestOperation>()
                    .ConstraintApprovalRequestOperation(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            return response;
        }

        protected override INF_MSG_Generic CallWSSigned(byte[] customRequestSignedByteArry, ConstraintApprovalRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IConstraintApprovalRequestOperation>()
                    .ConstraintApprovalRequestOperationSign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry },
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            return response;
        }
    }
}
