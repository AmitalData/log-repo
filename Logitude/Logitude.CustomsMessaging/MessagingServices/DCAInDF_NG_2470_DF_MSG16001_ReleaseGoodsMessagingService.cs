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
using UnifreightIIG.Common.MessageLib.DeclarationDeal;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInDF_NG_2470_DF_MSG16001_ReleaseGoodsMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        ReleaseGoodsResponseData,
        DCAInCustomRequest,
        DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage,
        DCAInCustomRequestService,
        DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseService, DCAInRequestHeader>
    {

        public override string MainInterfaceCode
        {
            get { return "2470"; }
        }


        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage customsResponse)
        {
            var tableName = "Customs.Declaration";
            //ResolveTenant() ==CustomsAgentToTenant(_CustomResponse.NoticeToClient.customsAgent);

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.ResponseContentHeader.ApplicationID.ToString(),
                //RequestName = "Acceptance of IMport dec # " + customsResponse.ResponseContentHeader.ApplicationID.ToString(),

            };
            return myGenericRequestParams;
        }

        
        protected override DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }
    }
}
