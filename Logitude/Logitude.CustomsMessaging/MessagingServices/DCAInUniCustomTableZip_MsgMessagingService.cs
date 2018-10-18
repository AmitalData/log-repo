

using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.PhysicalCheck;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInUniCustomTableZip_MsgMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        SYSTBL_NG_9001_MSG_SystemTablesResponse,
        DCAInCustomReturnNullRequestService,
        UniCustomTableZip_MsgResponseService, RequestHeader>
        //Unifreight_L2US01RequestParam,  //DecId + uni.fileNumber 
        //Unifreight_L2US01ResponseData,
        //Unifreight_L2US01RequestParam,
        //LOGISIVUGWithResponseContentHeader,
        //Unifreight_L2US01RequestParamRequestService,
        //UniCustomTableZip_MsgResponseService, DCAInRequestHeader>
    {

        public override string MainInterfaceCode
        {
            get { return "UCTZIP"; }//BuildCustomTableZip
        }

    
        protected override SYSTBL_NG_9001_MSG_SystemTablesResponse CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            //throw new NotImplementedException();
            _ResponseHeader = new DefaultResponseHeaderOrFault() { CorrelationId = Guid.NewGuid().ToString() , Status=""


            };
            exceptionMessage = "";
            return new SYSTBL_NG_9001_MSG_SystemTablesResponse() { ResponseContentHeader = new ResponseContentHeader() };
        }
   

    }
}
