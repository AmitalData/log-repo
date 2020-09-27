using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Deficit.NG5108;

namespace Logitude.CustomsMessaging
{
    public class DCAInDE_NG_5108_DecisionMessageMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        DE_NG_5108_MSG12_DecisionMessage,
        DCAInCustomRequestService,
        DE_NG_5108_DecisionMessageResponseService, DCAInRequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "5108"; }
        }

        protected override DE_NG_5108_MSG12_DecisionMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DE_NG_5108_MSG12_DecisionMessage customsResponse)
        {
            var tableName = "Customs.Deficit";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.DeficitFile.FirstOrDefault().TapagIdentifier.fileNumber;
            };
            if (customsResponse.DeficitFile != null)
            {
                myGenericRequestParams.RequestName = @" החלטה בתיק גרעון" + customsResponse.DeficitFile.FirstOrDefault().TapagIdentifier.fileNumber;
            }
            return myGenericRequestParams;
        }
    }
}
