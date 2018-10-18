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
using UnifreightIIG.Common.ExportDeclarationDataRequestServiceReference;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DF_9070_ExportDeclarationDataMessagingService : MessagingServiceBase<
    ExportDeclarationDataRequestParams, ExportDeclarationDataResponseData,
    DF_MSG_9070_ExportDeclarationDataRequst, DF_MSG_9071_ExportDeclarationDataResponse,
    DF_9070_ExportDeclarationDataRequestService, DF_9071_ExportDeclarationDataResponseService, RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "9070"; }
        }

        protected override DF_MSG_9071_ExportDeclarationDataResponse CallWS(DF_MSG_9070_ExportDeclarationDataRequst customRequest, ExportDeclarationDataRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new DF_MSG_9071_ExportDeclarationDataResponse();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IExportDeclarationDataRequestOperation>()
                    .ExportDeclarationDataRequest(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }

        protected override ExportDeclarationDataRequestParams CreateDefaultRequestParamsFromCustomsResponse(DF_MSG_9071_ExportDeclarationDataResponse customsResponse)
        {
            var tableName = "Customs.Declaration";
            var myGenericRequestParams = new ExportDeclarationDataRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }
    }
}

