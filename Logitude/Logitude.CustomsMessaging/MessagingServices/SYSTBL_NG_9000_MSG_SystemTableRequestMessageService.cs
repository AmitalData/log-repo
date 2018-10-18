using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.TheGateway;
using System.Threading;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class SYSTBL_NG_9000_MSG_SystemTableRequestMessageService
         : MessagingServiceBase<
        SystemTableRequestParams,
        SystemTableResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        SYSTBL_NG_9001_MSG_SystemTablesResponse,
        SYSTBL_NG_9000_MSG_SystemTableRequestService,
        SYSTBL_NG_9001_MSG_SystemTablesResponseService, RequestHeader>
    {

        public override string MainInterfaceCode
        {
            get { return "9000"; }
        }

        protected override SYSTBL_NG_9001_MSG_SystemTablesResponse CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, SystemTableRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new SYSTBL_NG_9001_MSG_SystemTablesResponse();

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            //var ExternalId = Guid.NewGuid().ToString();

            //customRequest.RequestContentHeader = new RequestContentHeader() { SenderID = 1, RecieverID = new int[] { 1 } };
            //myMP.MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.TestMode;

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IGatewayServiceSystemTableService>()
                    .SystemTablesDetails(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);

            }
            
            return response;
        }

        public static SystemTableResponseData SendIt(string closedTableId, int tenant, bool pseudo = false)
        {
            var messageService = new SYSTBL_NG_9000_MSG_SystemTableRequestMessageService();
            var req = new Logitude.CustomsMessaging.Common.RequestParams.SystemTableRequestParams()
            {
                TableId = closedTableId,
                Tenant = tenant,//_CustomsSetting.Tenant ,
                RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceBatch ,
                
            };
            if (pseudo)
            {
                req.Pseudo = true;
                req.RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive;
            }
            if (closedTableId == "1892")
            {
                req.AsTableData = true;
                var debugIt = false;
                if (debugIt)
                {
                    req.RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive;
                }
            }
            // req.RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive;    
            var res =messageService.Send(req);
            return res;

        }
   
        
    }
}
