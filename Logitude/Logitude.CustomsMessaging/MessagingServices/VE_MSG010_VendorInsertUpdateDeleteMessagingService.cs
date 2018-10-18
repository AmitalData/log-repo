using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;
using UnifreightIIG.Common.VendorInsertUpdateDeleteServiceReference;
using Logitude.Server.Tools.Helpers;
using UnifreightIIG.Common.CommonIIGInterface;
using Simplog.Data.InfrastructureModel.Repositories;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class VE_MSG010_VendorInsertUpdateDeleteMessagingService : MessagingServiceBase<
        VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams,
        VE_MSG010_VendorInsertUpdateDeleteResponseData, 
        VE_MSG010_VendorInsertUpdateDeleteMessage, 
        INF_MSG_Generic, 
        VE_MSG010_VendorInsertUpdateDeleteMessageRequestService,
        VE_MSG010_VendorInsertUpdateDeleteMessageResponseService, RequestHeader>
    {

        public override string MainInterfaceCode { get { return "3670"; } }

        protected override VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams CreateDefaultRequestParamsFromCustomsResponse(INF_MSG_Generic customsResponse)
        {
            //this.MyRequestSheetParam.ObjectTableId1 = ObjectTabelRepository.GetObjectTableByName("");
            var tableName = "Customs.CustomsVendor";

            var myGenericRequestParams = new VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }
        /*protected override RequestSheetParam GetSheetDetailsFromRequestParam(VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams requestParams)
        {
            var localDescription = "";//Eitan H 12/2/15 TASK 11216 -->
            switch (requestParams.OperationType)
            {
                case Logitude.CustomsMessaging.Common.RequestParams.VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams.OperationTypes.Add:
                    localDescription = "הקמת ספק ";
                    break;
                case Logitude.CustomsMessaging.Common.RequestParams.VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams.OperationTypes.Update:
                    localDescription = "עדכון ספק ";
                    break;
                case Logitude.CustomsMessaging.Common.RequestParams.VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams.OperationTypes.Delete:
                    localDescription = "מחיקת ספק ";
                    break;
            }
            return new RequestSheetParam() { RequestDescription = localDescription + requestParams.VendorNumber + " - " + requestParams.VendorName };
        }*/

        protected override INF_MSG_Generic CallWS(VE_MSG010_VendorInsertUpdateDeleteMessage customRequest, VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams requestParams, out string exceptionMessage)
        {
            INF_MSG_Generic customResponse = null;

            //if (requestParams.VendorName.ToLower().Contains("tst"))
            //{
            //    Random randVendorNumber = new Random();
            //    exceptionMessage = null;
            //    customResponse = new INF_MSG_Generic() { ResponseContentHeader = new ResponseContentHeader() { ApplicationID = randVendorNumber.Next(100000), } };
            //}
            //else if (requestParams.VendorName.ToLower().Contains("rej"))
            //{
            //    UnifreightIIG.Common.VendorInsertUpdateDeleteServiceReference.Exception[] exceptions = new UnifreightIIG.Common.VendorInsertUpdateDeleteServiceReference.Exception[1];
            //    exceptions[0] = new UnifreightIIG.Common.VendorInsertUpdateDeleteServiceReference.Exception() { EnglishDescription = "This vendor is not approved", ExeptionDescription = "ספק זה לא אושר" };
            //    exceptionMessage = null;
            //    customResponse = new INF_MSG_Generic() { ResponseContentHeader = new ResponseContentHeader() { Exception = exceptions } };
            //}
            //else
            {
                var reqSoapHeader = new RequestHeader();
                reqSoapHeader.ExternalId = Guid.NewGuid().ToString();
                ///reqSoapHeader.ConsumerId = "038623617";
                //var ExternalId = Guid.NewGuid().ToString();
                MoreParams moreParams = new MoreParams();
                moreParams.MyOption = MoreParams.Options.TestMode;
                moreParams.MyOption = MoreParams.Options.None;

                using (var myUnifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
                {
                    _ResponseHeader = myUnifreightSdkGateway.GetChannel<IVendorRepository>().VendorRepository(
                         this.RequestsSheetExternalId,
                         base.CustomsSetting.CustomsAgentId,
                         customRequest,
                         ref moreParams,
                         out customResponse);
                }

                exceptionMessage = null;

            }
            return customResponse;
        }

        protected override VE_MSG010_VendorInsertUpdateDeleteResponseData GetIIGBLExceptionFromReponseHeader(INF_MSG_Generic customsResponse)
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
                        {
                            LogMessagingUtil.Instance.AppendLine("VE_MSG010_VendorInsertUpdateDeleteMessage: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                                //_ResponseHeader.ErrorDescription = FormattedMessage;
                                //(_ResponseService as VE_MSG010_VendorInsertUpdateDeleteMessageResponseService).MyResponseData.ExceptionMessage = FormattedMessage;
                            }
                        }
                        break;
                    default:
                        return new VE_MSG010_VendorInsertUpdateDeleteResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }

    }
}
