

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
using UnifreightIIG.Common.GlobalScannedAttachmentToEntityServiceReference;
using UnifreightIIG.Common.TheGateway;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService
        : MessagingServiceBase<
        D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam,
        AddAttachmentResponseData,
        D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity,
        D_NG_2716_MSG22001_AddAttachmentResponse,
        D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestService,
        D_NG_2716_MSG22001_AddAttachmentResponseService, RequestHeader>
    {

        public override string MainInterfaceCode
        {
            get
            {
                return "2715";
            }
        }


        protected override void BuildRequestContentHeaderB4Sign(D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity customRequest)
        {
            customRequest.RequestContentHeader = new RequestContentHeader() { SenderID = 1, RecieverID = new int[] { 1 }, TransmitionDateTime = DateTime.Now };
        }

        protected override void PreCallWS(D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity customRequest, D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam requestParams)
        {
            base.PreCallWS(customRequest, requestParams);
        }

        protected override D_NG_2716_MSG22001_AddAttachmentResponse CallWS(D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity customRequest, D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new D_NG_2716_MSG22001_AddAttachmentResponse();
            //IResponseHeaderOrFault responseHeader;
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            //var ExternalId = Guid.NewGuid().ToString();


            BuildRequestContentHeaderB4Sign(customRequest);
            //myMP.MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.TestMode;


            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IGlobalScannedAttachmentToEntityOperation>()
                    .AddAGlobalScannedAttachmentToEntity(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);

            }


            return response;

        }

        protected override D_NG_2716_MSG22001_AddAttachmentResponse CallWSSigned(byte[] customRequestSignedByteArry, D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new D_NG_2716_MSG22001_AddAttachmentResponse();
            //IResponseHeaderOrFault responseHeader;
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            //var ExternalId = Guid.NewGuid().ToString();



            //myMP.MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.TestMode;


            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IGlobalScannedAttachmentToEntitySignOperation>()
                    .GlobalScannedAttachmentToEntitySign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry },
                    ref this._IIGGatewayMoreParams,
                    out response);

            }


            return response;
        }

        protected override AddAttachmentResponseData GetIIGBLExceptionFromReponseHeader(D_NG_2716_MSG22001_AddAttachmentResponse customsResponse)
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
                            LogMessagingUtil.Instance.AppendLine("D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                                //_ResponseHeader.ErrorDescription = FormattedMessage;
                                //(_ResponseService as D_NG_2716_MSG22001_AddAttachmentResponseService)._ResponseHeaderExeption = _ResponseHeader;
                            }
                        }
                        break;
                    default:
                        return new AddAttachmentResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }

        protected override D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam CreateDefaultRequestParamsFromCustomsResponse(D_NG_2716_MSG22001_AddAttachmentResponse customsResponse)
        {
            var tableName = "Customs.CustomsDocument";
            var myGenericRequestParams = new D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }
        public static RequestSheetParam GetReqSheetParam(string DocumentsFilingId, string DeclaretionId, string requestDescription)
        {
            var myRequestSheetParam = new RequestSheetParam();
            if (!string.IsNullOrWhiteSpace(DeclaretionId))
            {
                myRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                myRequestSheetParam.EntityId1 = DeclaretionId;
            }
            myRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsDocument");
            myRequestSheetParam.EntityId2 = DocumentsFilingId;
            myRequestSheetParam.RequestDescription = requestDescription;
            return myRequestSheetParam;
        }
    }
}
