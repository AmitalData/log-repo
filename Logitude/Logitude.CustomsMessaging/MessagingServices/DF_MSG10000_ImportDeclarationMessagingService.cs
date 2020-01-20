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
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.ImportDeclarationServiceReference;
using UnifreightIIG.Common.TheGateway;

using System.Diagnostics;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;
using Logitude.CustomsMessaging.FakeMessagingServices;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DF_MSG10000_ImportDeclarationMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DF_MSG10000_ImportDeclaration,
        DF_NG_2754_MSG10004_ImportDeclarationResponse,
        DF_MSG10000_ImportDeclarationRequestService,
        DF_NG_2754_MSG10004_ImportDeclarationResponseService, RequestHeader>
    {

        
        public override string MainInterfaceCode
        {
            get
            {
                return "2750";
            }
        }
        protected override DcaReceivedController GetDcaReceivedController(DF_NG_2754_MSG10004_ImportDeclarationResponse customsResponse, GenericRequestParams RequestParams)
        {
            if (customsResponse==null)
            {
                return null;
            }
            if (customsResponse.Response == null)
            {
                return null;
            }
            if (customsResponse.Response.Declaration == null)
            {
                return null;
            }
            return new DcaReceivedController() { DcaAnalyzeAggregateKey = customsResponse.Response.Declaration.ID.Value };
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DF_NG_2754_MSG10004_ImportDeclarationResponse customsResponse)
        {
            return new GenericRequestParams() {   RequestName="Should not Use !!"};
        }

        protected override INF_MSG_GenericResponseData GetIIGBLExceptionFromReponseHeader(DF_NG_2754_MSG10004_ImportDeclarationResponse customsResponse)
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
                            LogMessagingUtil.Instance.AppendLine("DF_MSG10000_ImportDeclarationMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                                //ITZIK+MIRT _ResponseHeader.ErrorDescription = FormattedMessage;
                                //ITZIK+MIRT (_ResponseService as DF_NG_2754_MSG10004_ImportDeclarationResponseService)._ResponseHeaderExeption = _ResponseHeader;
                            }
                        }
                        break;
                    default:
                        return new INF_MSG_GenericResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }


        protected override void PreCallWS(DF_MSG10000_ImportDeclaration customRequest, GenericRequestParams requestParams)
        {
            //TODO:yUVAL DELETE ...
            base.PreCallWS(customRequest, requestParams);
        }

        
        
        protected override DF_NG_2754_MSG10004_ImportDeclarationResponse CallWSSigned( 
            byte[] customRequestSignedByteArry, GenericRequestParams requestParams, out string exceptionMessage)
        {

            exceptionMessage = null;
            var response = new DF_NG_2754_MSG10004_ImportDeclarationResponse();

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IImportDeclarationSign>()
                    .ImportDeclarationSign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry },
                    ref this._IIGGatewayMoreParams,
                    out response);
            }


            return response;
            
        }
        protected override DF_NG_2754_MSG10004_ImportDeclarationResponse CallWS(
            DF_MSG10000_ImportDeclaration customRequest, 
            GenericRequestParams requestParams, 
            out string exceptionMessage)
        {
            if (requestParams.TestCase != null)
            {
                var myFake1DF_NG_2754_MSG10004_ImportDeclarationResponse = new Fake1DF_NG_2754_MSG10004_ImportDeclarationResponse();
                return myFake1DF_NG_2754_MSG10004_ImportDeclarationResponse
                    .CallWS(customRequest, requestParams, out exceptionMessage);
            }
            exceptionMessage = null;
            var response = new DF_NG_2754_MSG10004_ImportDeclarationResponse();
          
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            BuildRequestContentHeaderB4Sign(customRequest);


            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IImportDeclaration>()
                    .ImportDeclaration(
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
