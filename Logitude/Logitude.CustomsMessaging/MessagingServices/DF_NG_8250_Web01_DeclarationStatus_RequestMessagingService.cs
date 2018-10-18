
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
using UnifreightIIG.Common.DeclarationStatusQueryRequestServiceReference;
using UnifreightIIG.Common.TheGateway;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService
         : MessagingServiceBase<
        DeclarationStatusRequestParams,
        DeclarationStatusResponseData,
        DF_NG_8250_Web01_DeclarationStatus_Request,
        DF_NG_8251_Web02_DeclarationStatus_Response,
        DF_NG_8250_Web01_DeclarationStatus_RequestService,
        DF_NG_8251_Web02_DeclarationStatus_ResponseService, RequestHeader>
    {


        protected override DeclarationStatusRequestParams CreateDefaultRequestParamsFromCustomsResponse(DF_NG_8251_Web02_DeclarationStatus_Response customsResponse)
        {
            var tableName = "Customs.Declaration";

            var myGenericRequestParams = new DeclarationStatusRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };

            return myGenericRequestParams;
        }

        public override string MainInterfaceCode { get { return "8250"; } }

        public static DeclarationStatusResponseData SendInteractive(DeclarationStatusRequestParams searchParams) // moran 13.1.14 - Task 10236 - change to send from service - reuse send code
        {
            
            searchParams.RequestVIA = SendRequestVIA.WebServiceInteractive;

            var myDeclarationStatus_RequestMessagingService = new Logitude.CustomsMessaging.MessagingServices.DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService();
            var resData = myDeclarationStatus_RequestMessagingService.Send(searchParams);
            return resData;

        }


        protected override DF_NG_8251_Web02_DeclarationStatus_Response CallWS(
            DF_NG_8250_Web01_DeclarationStatus_Request customRequest, 
            DeclarationStatusRequestParams requestParams, 
            out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new DF_NG_8251_Web02_DeclarationStatus_Response();

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            //var ExternalId = Guid.NewGuid().ToString();



            customRequest.RequestContentHeader = new  RequestContentHeader() { SenderID = 1, RecieverID = new int[] { 1 } };
            //myMP.MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.TestMode;



            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IDeclarationStatusQueryRequestOperation>()
                    .DeclarationStatusQueryRequestOperation(
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
