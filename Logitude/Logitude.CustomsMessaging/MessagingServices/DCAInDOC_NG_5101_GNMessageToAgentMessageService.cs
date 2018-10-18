

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
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.MessageLib.Docs;
using UnifreightIIG.Common.TheGateway;


//ResponseHeader MessageToAgentRequestOperation(string ExternalId, string ConsumerID, DOC_NG_5101_GNMessageToAgent myRequest, ref MoreParams myMoreParams, out INF_MSG_Generic myResponse);
namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInDOC_NG_5101_GNMessageToAgentMessageService
     : MessagingServiceBase<
        GenericRequestParams,INF_MSG_GenericResponseData,
        DCAInCustomRequest,DOC_NG_5101_GNMessageToAgent,
        DCAInCustomRequestService, DOC_NG_5101_GNMessageToAgentResponseService, 
        DCAInRequestHeader>

 
    {

        public override string MainInterfaceCode { get { return "5101O_I"; } }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DOC_NG_5101_GNMessageToAgent customsResponse)
        {

            
            var tableName = "Customs.Declaration";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                LoggingEntityId = customsResponse.MessageToAgent.RelatedEntity.entityIdKey1.ToString(),
                RequestName = "Message To Agent dec # " + customsResponse.MessageToAgent.RelatedEntity.entityIdKey1.ToString(),

            };
            return myGenericRequestParams;
        }



        protected override DOC_NG_5101_GNMessageToAgent CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }
    }
}
