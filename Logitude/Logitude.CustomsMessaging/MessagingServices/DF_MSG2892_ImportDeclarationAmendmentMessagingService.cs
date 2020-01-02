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
using UnifreightIIG.Common.ImportDeclarationAmendmentServiceReference;
using RequestHeader = UnifreightIIG.Common.ImportDeclarationAmendmentServiceReference.RequestHeader;
using ESBRequestSigned = UnifreightIIG.Common.ImportDeclarationAmendmentServiceReference.ESBRequestSigned;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DF_MSG2892_ImportDeclarationAmendmentMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DF_NG_2892_MSG14000_ImportDeclarationAmendmentRequestMsg,
        INF_MSG_Generic,
        DF_MSG2892_ImportDeclarationAmendmentRequestService,
        DF_NG_2892_MSG14000_ImportDeclarationResponseService, RequestHeader>
    {

        
        public override string MainInterfaceCode
        {
            get
            {
                return "2892";
            }
        }
        protected override DcaReceivedController GetDcaReceivedController(INF_MSG_Generic customsResponse, GenericRequestParams RequestParams)
        {
            if (customsResponse==null)
            {
                return null;
            }
            //if (customsResponse.Response == null)
            //{
            //    return null;
            //}
            //if (customsResponse.Response.Declaration == null)
            //{
            //    return null;
            //}
            return new DcaReceivedController();// { DcaAnalyzeAggregateKey = customsResponse.Response.Declaration.ID.Value };
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(INF_MSG_Generic customsResponse)
        {
            return new GenericRequestParams() {   RequestName="Should not Use !!"};
        }

        protected override INF_MSG_GenericResponseData GetIIGBLExceptionFromReponseHeader(INF_MSG_Generic customsResponse)
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


        protected override void PreCallWS(DF_NG_2892_MSG14000_ImportDeclarationAmendmentRequestMsg customRequest, GenericRequestParams requestParams)
        {
            //TODO:yUVAL DELETE ...
            base.PreCallWS(customRequest, requestParams);
        }

        
        
        protected override INF_MSG_Generic CallWSSigned( 
            byte[] customRequestSignedByteArry, GenericRequestParams requestParams, out string exceptionMessage)
        {

            exceptionMessage = null;
            var response = new INF_MSG_Generic();

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            

            //using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            //{
            //    _ResponseHeader = uifreightSdkGateway.GetChannel<IImportDeclarationSign>()
            //        .ImportDeclarationSign(
            //        this.RequestsSheetExternalId,
            //        base.CustomsSetting.CustomsAgentId,
            //        //new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry ,  },
            //        ref this._IIGGatewayMoreParams,
            //        out response);
            //}


            return response;
            
        }
        protected override INF_MSG_Generic CallWS(
            DF_NG_2892_MSG14000_ImportDeclarationAmendmentRequestMsg customRequest, 
            GenericRequestParams requestParams, 
            out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();
          
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            BuildRequestContentHeaderB4Sign(customRequest);


            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IImportDeclarationAmendmentOperation>()
                    .ImportDeclarationAmendment(
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
