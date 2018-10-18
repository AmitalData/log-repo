using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;
using UnifreightIIG.Common.DeclarationPrintServiceReference;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.CustomsMessaging.MessagingServices
{   
    public class DF_NG_8302_Web03_DeclarationPrintMessagingService : MessagingServiceBase<
        DF_NG_8302_Web03_DeclarationPrintRequestParams,
        DeclarationPrintResponseData,
        DF_NG_8302_Web03_DeclarationPrint_Request, 
        DF_NG_8303_Web04_DeclarationPrint_Response,
        DF_NG_8302_Web03_DeclarationPrintRequestService, 
        DF_NG_8303_Web04_DeclarationPrintResponseService, 
        RequestHeader>
    {

        public override string MainInterfaceCode
        {
            get { return "8302"; }
        }

        protected override DF_NG_8303_Web04_DeclarationPrint_Response CallWS(DF_NG_8302_Web03_DeclarationPrint_Request customRequest, DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new DF_NG_8303_Web04_DeclarationPrint_Response();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            
            customRequest.RequestContentHeader = new RequestContentHeader() { SenderID = 1, RecieverID = new int[] { 1 } };
 
            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IDeclarationPrintOperation>()
                    .DeclarationPrintOperation(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            return response;
        }

        protected override DeclarationPrintResponseData GetIIGBLExceptionFromReponseHeader(DF_NG_8303_Web04_DeclarationPrint_Response customsResponse)
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
                            LogMessagingUtil.Instance.AppendLine("DF_NG_8302_Web03_DeclarationPrintMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                               /// _ResponseHeader.ErrorDescription = FormattedMessage;
                                /// (_ResponseService as DF_NG_8303_Web04_DeclarationPrintResponseService)._ResponseHeaderExeption = _ResponseHeader;
                            }
                        }
                        break;
                    default:
                        return new DeclarationPrintResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }

        protected override RequestSheetParam GetSheetDetailsFromRequestParam(DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams)
        {
            var myRequestSheetParam = new RequestSheetParam();
            myRequestSheetParam.RequestDescription = "בקשה לטופס הצהרה";
            myRequestSheetParam.EntityId1 = requestParams.LoggingEntityId;
            myRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            return myRequestSheetParam;
        }

        protected override DF_NG_8302_Web03_DeclarationPrintRequestParams CreateDefaultRequestParamsFromCustomsResponse(DF_NG_8303_Web04_DeclarationPrint_Response customsResponse)
        {
            var tableName = "Customs.Declaration";

            var myGenericRequestParams = new DF_NG_8302_Web03_DeclarationPrintRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.DebtNotificationMessag.debtNotificationID.ToString()
            };
            return myGenericRequestParams;
        }
    }
}

