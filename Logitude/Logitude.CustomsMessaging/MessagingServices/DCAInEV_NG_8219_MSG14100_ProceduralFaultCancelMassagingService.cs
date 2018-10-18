using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Fault;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInEV_NG_8219_MSG14100_ProceduralFaultCancelMassagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        EV_NG_8219_MSG14100_ProceduralFaultCancelMsg,
        DCAInCustomRequestService,
        EV_NG_8219_MSG14100_ProceduralFaultCancelMsgResponseService, DCAInRequestHeader>
    {

        //protected override RequestSheetParam GetSheetDetailsFromRequestParam(GenericRequestParams requestParams)
        //{
        //    return base.GetSheetDetailsFromRequestParam(requestParams);
        //}
        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(EV_NG_8219_MSG14100_ProceduralFaultCancelMsg customsResponse)
        {
            var tableName = "Customs.ProceduralFault";
            

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.ResponseContentHeader.ApplicationID.ToString(),
                //RequestName = "Acceptance of IMport dec # " + customsResponse.ResponseContentHeader.ApplicationID.ToString(),

            };
            return myGenericRequestParams;
        }
        protected override EV_NG_8219_MSG14100_ProceduralFaultCancelMsg CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "8219"; }
        }
    }
}
