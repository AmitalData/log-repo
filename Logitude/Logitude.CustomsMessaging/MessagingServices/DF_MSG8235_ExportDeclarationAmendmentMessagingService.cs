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
using RequestHeader = UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.RequestHeader;
using Logitude.CustomsMessaging.FakeMessagingServices;
using UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DF_MSG8235_ExportDeclarationAmendmentMessagingService
        : MessagingServiceBase<
        AmendmentRequestParams,
        ExportDeclarationAmendmentResponseData,
        DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg,
        DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg,
        DF_MSG8235_ExportDeclarationAmendmentRequestService,
        DF_NG_8237_ExportDeclerationAmendmentReplyResponseService, RequestHeader>
    {

        
        public override string MainInterfaceCode
        {
            get
            {
                return "8235";
            }
        }
        protected override DcaReceivedController GetDcaReceivedController(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customsResponse, AmendmentRequestParams RequestParams)
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

        protected override AmendmentRequestParams CreateDefaultRequestParamsFromCustomsResponse(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customsResponse)
        {
            return new AmendmentRequestParams() {   RequestName="Should not Use !!"};
        }

        protected override ExportDeclarationAmendmentResponseData GetIIGBLExceptionFromReponseHeader(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customsResponse)
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
                            LogMessagingUtil.Instance.AppendLine("DF_MSG10000_ExportDeclarationMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                                //ITZIK+MIRT _ResponseHeader.ErrorDescription = FormattedMessage;
                                //ITZIK+MIRT (_ResponseService as DF_NG_2754_MSG10004_ImportDeclarationResponseService)._ResponseHeaderExeption = _ResponseHeader;
                            }
                        }
                        break;
                    default:
                        return new ExportDeclarationAmendmentResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }


        protected override void PreCallWS(DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg customRequest, AmendmentRequestParams requestParams)
        {
            //TODO:yUVAL DELETE ...
            base.PreCallWS(customRequest, requestParams);
        }

        
        
        protected override DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg CallWSSigned( 
            byte[] customRequestSignedByteArry, AmendmentRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;

            var response = new DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg();
            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<UnifreightIIG.Common.TheGateway.IExportDeclarationAmendmentRequestMsgRequestOperation>()
                    .ExportDeclarationAmendmentRequestMsgRequestSign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry ,  },
                    ref this._IIGGatewayMoreParams,
                    out response);
            }


            return response;
            
        }
        protected override DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg CallWS(
            DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg customRequest,
            AmendmentRequestParams requestParams,
            out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg();
            if (requestParams.TestCase != null)
            {
                BuildRequestContentHeaderB4Sign(customRequest);
                 

                        //var Fake2892 = new Fake_DF_NG_2892_MSG14000_ImportDeclarationResponseService(requestParams);
                //_ResponseHeader= Fake2892.CallWS(out response);

           
            exceptionMessage = null;
            return response;
         
            }
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            BuildRequestContentHeaderB4Sign(customRequest);


            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<UnifreightIIG.Common.TheGateway.IExportDeclarationAmendmentRequestMsgRequestOperation>()
                    .ExportDeclarationAmendmentRequestMsgRequest(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
                
            }
            return response;
        }


    }

   public class  AmendmentRequestParams: GenericRequestParams
    {
        public bool IsExportClose { get; set; }
        public bool IsTransShipment { get; set; }
    }

}
