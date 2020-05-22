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
 using UnifreightIIG.Common.TheGateway;

using System.Diagnostics;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;
using Logitude.CustomsMessaging.FakeMessagingServices;
using UnifreightIIG.Common.ExportDeclarationServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DF_NG_2751_MSG10000_ExportDeclarationMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DF_NG_2751_MSG10000_ExportDeclaration,
        DF_NG_2757_MSG10004_ExportDeclarationResponse,
        DF_NG_2751_MSG10000_ExportDeclarationRequestService,
        DF_NG_2757_MSG10004_ExportDeclarationResponseService, RequestHeader>
    {

        
        public override string MainInterfaceCode
        {
            get
            {
                return "2751";
            }
        }
        protected override DcaReceivedController GetDcaReceivedController(DF_NG_2757_MSG10004_ExportDeclarationResponse customsResponse, GenericRequestParams RequestParams)
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

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DF_NG_2757_MSG10004_ExportDeclarationResponse customsResponse)
        {
            return new GenericRequestParams() {   RequestName="Should not Use !!"};
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


        protected override void PreCallWS(DF_NG_2751_MSG10000_ExportDeclaration customRequest, GenericRequestParams requestParams)
        {
            //TODO:yUVAL DELETE ...
            base.PreCallWS(customRequest, requestParams);
        }

        
        
        protected override DF_NG_2757_MSG10004_ExportDeclarationResponse CallWSSigned( 
            byte[] customRequestSignedByteArry, GenericRequestParams requestParams, out string exceptionMessage)
        {

            exceptionMessage = null;
            var response = new DF_NG_2757_MSG10004_ExportDeclarationResponse();

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IExportDeclarationOperation>()
                    .ExportDeclarationSign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry },
                    ref this._IIGGatewayMoreParams,
                    out response);
            }


            return response;
            
        }
        protected override DF_NG_2757_MSG10004_ExportDeclarationResponse CallWS(
            DF_NG_2751_MSG10000_ExportDeclaration customRequest, 
            GenericRequestParams requestParams, 
            out string exceptionMessage)
        {
            var response = new DF_NG_2757_MSG10004_ExportDeclarationResponse();
            //if (requestParams.TestCase != null)
            //{
            //    BuildRequestContentHeaderB4Sign(customRequest);
            //    switch (requestParams.TestCase.Code)
            //    {
            //        case "2754Valid":
            //            var Fake2754ValidMsg = new Fake_2754_MSG10004_ImportDeclarationResponse(requestParams);
            //            _ResponseHeader = Fake2754ValidMsg.CallWS(out response);
            //            break;
            //        case "2754Constraint":
            //            var Fake2754WithConstraintMsg = new Fake_2754_MSG10004_ImportDeclarationResponseWithConstraint(requestParams);
            //            _ResponseHeader = Fake2754WithConstraintMsg.CallWS(out response);
            //            break;
            //        case "2754Payment":
            //            var Fake2754SumbitPayment = new Fake_2754_MSG10004_SumbitPayment(requestParams);
            //            _ResponseHeader = Fake2754SumbitPayment.CallWS(out response);
            //            break;
            //    }
            //    exceptionMessage = null;
            //    return response;
            //}
            exceptionMessage = null;

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            BuildRequestContentHeaderB4Sign(customRequest);


            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IExportDeclarationOperation>()
                    .ExportDeclaration(
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
