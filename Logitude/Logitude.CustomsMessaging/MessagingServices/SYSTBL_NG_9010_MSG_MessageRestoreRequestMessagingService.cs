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
using UnifreightIIG.Common.MessageRestoreServiceReference;
using UnifreightIIG.Common.TheGateway;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class SYSTBL_NG_9010_MSG_MessageRestoreRequestMessagingService
        : MessagingServiceBase<
        MessageRestoreRequestParams, MessageRestoreResponseData,
        SYSTBL_NG_9010_MSG_MessageRestoreRequest, SYSTBL_NG_9011_MSG_MessageRestoreResponse,
        SYSTBL_NG_9010_MSG_MessageRestore_RequestService, SYSTBL_NG_9011_MSG_MessageRestore_ResponseService,
        RequestHeader>
    {
        public override string MainInterfaceCode { get { return "9010"; } }


        protected override SYSTBL_NG_9011_MSG_MessageRestoreResponse CallWS(SYSTBL_NG_9010_MSG_MessageRestoreRequest customRequest, MessageRestoreRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new SYSTBL_NG_9011_MSG_MessageRestoreResponse();

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };


            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IMessageRestoreOperation>()
                    .MessageRestoreOperation(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }

        protected override MessageRestoreRequestParams CreateDefaultRequestParamsFromCustomsResponse(SYSTBL_NG_9011_MSG_MessageRestoreResponse customsResponse)
        {
            //var tableName = "Customs.Declaration";
            var myMessageRestoreRequestParams = new MessageRestoreRequestParams()
            {
                //LoggingObjectTableId = ObjectTabelRepository.GetObjectTableByName(tableName),
                RequestName = "Resotre Messages Request",
            };
            return myMessageRestoreRequestParams;
        }
    }
}
