
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
using UnifreightIIG.Common.VendorAddCommunicationDeviceServiceReference;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class VE_MSG013_VendorAddCommunicationDeviceMessageService 
        : MessagingServiceBase<
        VE_MSG013_VendorAddCommunicationDeviceRequestParams,
        INF_MSG_GenericResponseData,
        VE_MSG013_VendorAddCommunicationDevice,
        INF_MSG_Generic,
        VE_MSG013_VendorAddCommunicationDeviceRequestService,
        VE_MSG013_VendorAddCommunicationDeviceResponseService, RequestHeader>
    {

        public override string MainInterfaceCode { get { return "3680"; } }

        protected override VE_MSG013_VendorAddCommunicationDeviceRequestParams CreateDefaultRequestParamsFromCustomsResponse(INF_MSG_Generic customsResponse)
        {
         
            //this.MyRequestSheetParam.ObjectTableId1 = ObjectTabelRepository.GetObjectTableByName("");
            var tableName = "Customs.CustomsVendor";

            var myGenericRequestParams = new VE_MSG013_VendorAddCommunicationDeviceRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }
        protected override RequestSheetParam GetSheetDetailsFromRequestParam(VE_MSG013_VendorAddCommunicationDeviceRequestParams requestParams)
        {
            if(requestParams.VendorNumber==null)
            {
                return null;
            }
            return new RequestSheetParam() { RequestDescription = "הוספת תקשורת לספק " + requestParams.VendorNumber };
        }
        
        protected override INF_MSG_Generic CallWS(VE_MSG013_VendorAddCommunicationDevice customRequest, VE_MSG013_VendorAddCommunicationDeviceRequestParams requestParams, out string exceptionMessage)
        {
            ///IResponseHeaderOrFault responseHeader;
            var response = new INF_MSG_Generic();
            if (requestParams.IsFakeResponse)
            {
                Random randVendorNumber = new Random();
                exceptionMessage = null;
                response = new INF_MSG_Generic() { ResponseContentHeader = new UnifreightIIG.Common.VendorAddCommunicationDeviceServiceReference.ResponseContentHeader() { ApplicationID = requestParams.VendorNumber.Value, } };
            }
            else
            {
                exceptionMessage = null;
                // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
                //var ExternalId = Guid.NewGuid().ToString();

                using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
                {
                    _ResponseHeader = uifreightSdkGateway.GetChannel<IVendorAddCommunicationDevice>()
                        .VendorAddCommunicationDevice(
                        this.RequestsSheetExternalId,
                        base.CustomsSetting.CustomsAgentId,
                        customRequest,
                        ref this._IIGGatewayMoreParams,
                        out response);
                }               
            }
            return response;
        }       
    }
}
