
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.CustomsMessaging.FakeMessagingServices;
using UnifreightIIG.Common.ContainerizationMessageServiceReference;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.TheGateway;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityUpdateServices;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class SaveCC_MSG2450_ContainerizationMessageMessagingService
         : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        AV_MSG2_ContainerizationMessage,
        INF_MSG_Generic,
        SaveCC_MSG2450_ContainerizationMessageRequestService,
        SaveCC_MSG2450_ContainerizationMessageResponseService,
        RequestHeader>
    {


   

        public override string MainInterfaceCode { get { return "2450"; } }
       
        public static INF_MSG_GenericResponseData SendInteractive(GenericRequestParams searchParams) // moran 13.1.14 - Task 10236 - change to send from service - reuse send code
        {
            
            searchParams.RequestVIA = SendRequestVIA.WebServiceInteractive;

            var saveCC_MSG2450_ContainerizationMessageMessagingService = new Logitude.CustomsMessaging.MessagingServices.SaveCC_MSG2450_ContainerizationMessageMessagingService();
            var resData = saveCC_MSG2450_ContainerizationMessageMessagingService.Send(searchParams);
            return resData;

        }


        protected override INF_MSG_Generic CallWS(
            AV_MSG2_ContainerizationMessage customRequest,
            GenericRequestParams requestParams, 
            out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();

            customRequest.RequestContentHeader = new  RequestContentHeader() { SenderID = 1, RecieverID = new int[] { 1 } };

            if (requestParams.TestCase != null)
            {
                BuildRequestContentHeaderB4Sign(customRequest);
                switch (requestParams.TestCase.Code)
                {
                    case "2450NotFound":
                        var Fake2450NotFoundMsg = new Fake_2450_NotFound(requestParams);
                        _ResponseHeader = Fake2450NotFoundMsg.CallWS(requestParams, out response);
                        break;

                    case "2450Found":
                        var Fake2450FoundMsg = new Fake_2450_Found(requestParams);
                        _ResponseHeader = Fake2450FoundMsg.CallWS(requestParams, out response);
                        break;

                    case "2450UpdateContainerWithError":
                        var Fake2450UpdateContainerWithErrorMsg = new Fake_2450_UpdateContainerWithError(requestParams);
                        _ResponseHeader = Fake2450UpdateContainerWithErrorMsg.CallWS(requestParams, out response);
                        break;

                    case "2450UpdateContainer":
                        var Fake2450UpdateContainerMsg = new Fake_2450_UpdateContainer(requestParams);
                        _ResponseHeader = Fake2450UpdateContainerMsg.CallWS(requestParams, out response);
                        break;

                    case "2450CancelContainerization":
                        var Fake2450CancelContainerizationMsg = new Fake_2450_CancelContainerization(requestParams);
                        _ResponseHeader = Fake2450CancelContainerizationMsg.CallWS(requestParams, out response);
                        break;

                }
                exceptionMessage = null;
            }
            else
            {
                using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
                {
                    _ResponseHeader = uifreightSdkGateway.GetChannel<IContainerizationMessageOperation>()
                        .ContainerizationMessage(
                        this.RequestsSheetExternalId,
                        base.CustomsSetting.CustomsAgentId,
                        customRequest,
                        ref this._IIGGatewayMoreParams,
                        out response);
                }
            }
            
            if (response.ResponseContentHeader.Exception != null && response.ResponseContentHeader.Exception.Any(x => x.ExceptionLevel == 3))
            {
                ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
                var myContainerizationQueryService = new ContainerizationQueryService(dbContext);
                string containerizationID = requestParams.LoggingEntityId;
                var _ContainerizationPM = myContainerizationQueryService.GetSingle(containerizationID, true, false);
                if (string.IsNullOrEmpty(_ContainerizationPM.ContainerizationStatus))
                    _ContainerizationPM.ContainerizationStatus = "4";

                if (_ContainerizationPM.ContainerizationStatus == "1")
                    _ContainerizationPM.ContainerizationStatus = "2";

                _ContainerizationPM.ChangeSetOp = ChangeSetOperation.Update;
                var containerizationUpdateService = new ContainerizationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                containerizationUpdateService.Update(_ContainerizationPM, true);
            }

            return response;
        }

       
    }
}
