using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.FakeMessagingServices;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.ExportStorage.MN2791;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInMN_MSG2791_ExportDeliveryAnswerMessageMessagingServices
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        MN_MSG2791_ExportDeliveryAnswerMessage,
        DCAInCustomRequestService,
        MN_MSG2791_ExportDeliveryAnswerMessageResponseService, DCAInRequestHeader>
    {

        public override string MainInterfaceCode
        {
            get { return "2791"; }
        }


        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(MN_MSG2791_ExportDeliveryAnswerMessage customsResponse)
        {
            int contextTenant = 0;
            if (RequestParams?.Tenant == null)
            {
                contextTenant = SettingUtil.GetCurrentTenant();
            }
            else
            {
                contextTenant = RequestParams.Tenant;
            }

            ExportStoragePM entity = new ExportStorageQueryService(CustomContext.GetContext(contextTenant)).GetByCargoKeys(
                customsResponse.CargoIdentifier.cargoIdentifierKey1,
                customsResponse.CargoIdentifier.cargoIdentifierKey2,
                customsResponse.CargoIdentifier.cargoIdentifierKey3,
                customsResponse.CargoIdentifier.cargoIdentifierType);

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                LoggingEntityId = entity?.DeclarationId,
                LoggingObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.ExportStorage"),
                LoggingEntityId2 = entity?.Id,                
            };
            
            return myGenericRequestParams;
        }

        
        protected override MN_MSG2791_ExportDeliveryAnswerMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }
    }
}
