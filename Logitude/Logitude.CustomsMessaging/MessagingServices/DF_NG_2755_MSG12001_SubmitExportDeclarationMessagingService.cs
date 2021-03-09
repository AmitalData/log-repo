
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
using UnifreightIIG.Common.TheGateway;
using Logitude.Server.Tools.Helpers;
using Logitude.CustomsMessaging.FakeMessagingServices;
using UnifreightIIG.Common.SubmitExportDeclarationRequestServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DF_NG_2755_MSG12001_SubmitExportDeclarationMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DF_NG_2755_MSG12001_SubmitDeclaration,
        DF_NG_2757_MSG10004_ExportDeclarationResponse,
        DF_NG_2755_MSG12001_SubmitExportDeclarationRequestService,
        DF_NG_2754_MSG10004_SubmitExportDeclarationResponseService, RequestHeader>
    {

        public override string MainInterfaceCode { get { return "2755E"; } }



        
        protected override void BuildRequestContentHeaderB4Sign(DF_NG_2755_MSG12001_SubmitDeclaration customRequest)
        {
            customRequest.RequestContentHeader = new RequestContentHeader() { SenderID = 1, RecieverID = new int[] { 1 }  };
        }

       /* protected override DF_NG_2757_MSG10004_ExportDeclarationResponse CallWSSigned(byte[] customRequestSignedByteArry, GenericRequestParams requestParams, out string exceptionMessage)
        {
         exceptionMessage = null;
            var response = new DF_NG_2757_MSG10004_ExportDeclarationResponse();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            
            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IImportDeclarationSubmitRequestSignOperation>()
                    .IImportDeclarationSubmitSignRequest (
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new   ESBRequestSigned () { SignedByteArry = customRequestSignedByteArry}  ,
                    ref this._IIGGatewayMoreParams,
                    out response);                        
            }

            return response;
        }
        */

        protected override DF_NG_2757_MSG10004_ExportDeclarationResponse CallWS(DF_NG_2755_MSG12001_SubmitDeclaration customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new DF_NG_2757_MSG10004_ExportDeclarationResponse();
            //IResponseHeaderOrFault responseHeader;
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            if (requestParams.TestCase != null)
            {
                BuildRequestContentHeaderB4Sign(customRequest);


                var Fake2755 = new Fake_2754_MSG10004_SumbitPayment(requestParams);
               // _ResponseHeader = Fake2755.CallWS(out response, requestParams);


                exceptionMessage = null;
                return response;

            }

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                try
                {
                    _ResponseHeader = uifreightSdkGateway.GetChannel<ISubmitExportDeclarationRequestOperation>()
                        .SubmitExportDeclarationRequest(
                        this.RequestsSheetExternalId,
                        base.CustomsSetting.CustomsAgentId,
                        customRequest,
                        ref this._IIGGatewayMoreParams,
                        out response);
                }
                catch (System.Exception e)
                {
                    throw;
                }
            }

            return response;
        }

        protected override INF_MSG_GenericResponseData GetIIGBLExceptionFromReponseHeader(DF_NG_2757_MSG10004_ExportDeclarationResponse customsResponse)
        {
            try
            {
                var responseContentHeader = customsResponse.GetResponseContentHeader() as IResponseContentHeader;
                ThrowIIGBLException(_ResponseHeader, responseContentHeader);

            }
            catch (System.ServiceModel.FaultException<UnifreightIIGFault> myUnifreightIIGFault)
            {
                var defaultMessage = "Sending request to IIG Server Failed ";
                var FormattedMessage = UnifreightIIG.Common.Utils.ErrorHandlerUtil.CreateNew().ToFormattedMessage(myUnifreightIIGFault);
                if (String.IsNullOrWhiteSpace(FormattedMessage))
                {
                    FormattedMessage = defaultMessage;
                }
                LogMessagingUtil.Instance.AppendLine(FormattedMessage);

                switch (myUnifreightIIGFault.Detail.PlaceFault)
                {

                    case UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError:
                    case UnifreightIIGFault.PlaceFaultEnum.IIGFatalException:
                    case UnifreightIIGFault.PlaceFaultEnum.IIGTechnicalError:
                        {
                            LogMessagingUtil.Instance.AppendLine("DF_NG_2755_MSG12001_SubmitDeclarationMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                                //ITZIK+MIRT  _ResponseHeader.ErrorDescription = FormattedMessage;
                                //ITZIK+MIRT (_ResponseService as DF_NG_2754_MSG10004_SubmitDeclarationResponseService)._ResponseHeaderExeption = _ResponseHeader;
                            }
                        }
                        break;
                    default:
                        return new INF_MSG_GenericResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }

    }
}
