
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
            return response;
        }

       
    }
}
