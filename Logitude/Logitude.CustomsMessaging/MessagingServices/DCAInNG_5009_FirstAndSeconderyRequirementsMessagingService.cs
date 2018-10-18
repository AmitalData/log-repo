using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessageAnalyzer;
using Logitude.CustomsMessaging.RequestServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Deficit;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInNG_5009_FirstAndSeconderyRequirementsMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        DE_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage,
        DCAInCustomRequestService,
        Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessageResponseService, DCAInRequestHeader>
    {

        protected override DE_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "5009"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DE_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage customsResponse)
        {
            var tableName = "Customs.PhysicalCheck";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.ResponseContentHeader.ApplicationID.ToString(), // to check with itzik about array
            };
            return myGenericRequestParams;
        }
    }
}
