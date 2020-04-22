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
using Logitude.CustomsMessaging.FakeMessagingServices;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DF_MSG2892_ImportDeclarationAmendmentMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DF_NG_2892_MSG14000_ImportDeclarationAmendmentRequestMsg,
        DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg,
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
        protected override DcaReceivedController GetDcaReceivedController(DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg customsResponse, GenericRequestParams RequestParams)
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

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg customsResponse)
        {
            return new GenericRequestParams() {   RequestName="Should not Use !!"};
        }

        protected override INF_MSG_GenericResponseData GetIIGBLExceptionFromReponseHeader(DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg customsResponse)
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

        
        
        protected override DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg CallWSSigned( 
            byte[] customRequestSignedByteArry, GenericRequestParams requestParams, out string exceptionMessage)
        {

            exceptionMessage = null;
            var response = new DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg();

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
        protected override DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg CallWS(
            DF_NG_2892_MSG14000_ImportDeclarationAmendmentRequestMsg customRequest,
            GenericRequestParams requestParams,
            out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg();
            if (requestParams.TestCase != null)
            {
                BuildRequestContentHeaderB4Sign(customRequest);
                 

                        var Fake2892 = new Fake_DF_NG_2892_MSG14000_ImportDeclarationResponseService(requestParams);
                _ResponseHeader= Fake2892.CallWS(out response);

           
            exceptionMessage = null;
            return response;
         
            }
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
