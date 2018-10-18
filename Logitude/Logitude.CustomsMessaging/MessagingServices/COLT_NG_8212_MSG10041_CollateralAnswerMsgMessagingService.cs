
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CollateralAnswerMsgServiceReference;
using UnifreightIIG.Common.TheGateway;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class COLT_NG_8212_MSG10041_CollateralAnswerMsgMessagingService:
        MessagingServiceBase<
        CollateralRequestParams,//GenericRequestParams,
        INF_MSG_GenericResponseData,
        COLT_NG_8212_MSG10041_CollateralAnswerMsg,
        INF_MSG_Generic,
        COLT_NG_8212_MSG10041_CollateralAnswerMsgRequestService,
        COLT_NG_8212_MSG10041_CollateralAnswerMsgResponseService, RequestHeader>
    {

        public override string MainInterfaceCode { get { return "8212"; } }

        protected override CollateralRequestParams CreateDefaultRequestParamsFromCustomsResponse(INF_MSG_Generic customsResponse)
        {
            var tableName = "Customs.CustomsCollateral";

            var myGenericRequestParams = new CollateralRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }

        protected override INF_MSG_Generic CallWSSigned(byte[] customRequestSignedByteArry, CollateralRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            //var ExternalId = Guid.NewGuid().ToString();


            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICollateralAnswerMsgOperation>()
                    .CollateralAnswerMsgOperationSign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry },
                    ref this._IIGGatewayMoreParams,
                    out response);

            }

            return response;
        }


        protected override INF_MSG_Generic CallWS(COLT_NG_8212_MSG10041_CollateralAnswerMsg customRequest, CollateralRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            //var ExternalId = Guid.NewGuid().ToString();


            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICollateralAnswerMsgOperation>()
                    .CollateralAnswerMsgOperation(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);

            }

            return response;
        }
    }
}
