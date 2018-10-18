using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.TheGateway;
using UnifreightIIG.Common.VehicleInServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class VP_NG_2690_VehicleInMessagingService : MessagingServiceBase<
        UpdateDeleteVehicleRequestParams,
        INF_MSG_GenericResponseData,
        VP_NG_2690_MSG100_VehicleIn,
        VP_NG_2691_MSG101_VehicleInResponse,
        VP_NG_2690_VehicleInRequestService,
        VP_NG_2691_MSG101_VehicleInResponseService,
        RequestHeader>
    {

        protected override VP_NG_2691_MSG101_VehicleInResponse CallWS(VP_NG_2690_MSG100_VehicleIn customRequest, UpdateDeleteVehicleRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new VP_NG_2691_MSG101_VehicleInResponse();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            BuildRequestContentHeaderB4Sign(customRequest);

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IVehicleInOperation>()
                    .VehicleInOperation(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }

        protected override VP_NG_2691_MSG101_VehicleInResponse CallWSSigned(byte[] customRequestSignedByteArry, UpdateDeleteVehicleRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new VP_NG_2691_MSG101_VehicleInResponse();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IVehicleInOperation>()
                    .VehicleInOperationSign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry },
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }


        public override string MainInterfaceCode
        {
            get { return "2690"; }
        }

        protected override UpdateDeleteVehicleRequestParams CreateDefaultRequestParamsFromCustomsResponse(VP_NG_2691_MSG101_VehicleInResponse customsResponse)
        {
            var tableName = "Customs.Vehicle";

            var myGenericRequestParams = new UpdateDeleteVehicleRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.DebtNotificationMessag.debtNotificationID.ToString()
            };
            return myGenericRequestParams;        }

    }
}
