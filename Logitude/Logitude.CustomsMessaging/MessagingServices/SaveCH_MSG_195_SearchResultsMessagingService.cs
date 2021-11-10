using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.TheGateway;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using UnifreightIIG.Common.SearchResultsServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class SaveCH_MSG_195_SearchResultsMessagingService : MessagingServiceBase<
        GenericRequestParams, INF_MSG_GenericResponseData,
        CH_NG_195_MSG6_SearchResults, INF_MSG_Generic,
        SaveCH_MSG_195_SearchResultsRequestService, SaveCH_MSG_195_SearchResultsResponseService,
        RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "195"; }
        }

        protected override INF_MSG_GenericResponseData GetIIGBLExceptionFromReponseHeader(INF_MSG_Generic customsResponse)
        {

            return base.GetIIGBLExceptionFromReponseHeader(customsResponse);
        }

        protected override INF_MSG_Generic CallWS(CH_NG_195_MSG6_SearchResults customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();
       
            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ISearchResultsOperation>()
                    .SearchResults(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);

            }
        
           //using (TransactionScope scope = TransactionFactory.GetTransaction())
           // {
           //     if (response.ResponseContentHeader.Exception != null)
           //     {
           //         ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);

           //         var cargoSealIdentifierQueryService = new CargoSealIdentifierQueryService(dbContext);
           //         var cargoSealIdentifierUpdateService = new CargoSealIdentifierUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
           //         CargoSealIdentifierPM cargoSealIdentifierPM = cargoSealIdentifierQueryService.GetSingle(requestParams.CargoSealIdentifierId, false, false);
           //         cargoSealIdentifierPM.Status = "2";
           //         cargoSealIdentifierPM.ChangeSetOp = ChangeSetOperation.Update;
           //         cargoSealIdentifierUpdateService.Update(cargoSealIdentifierPM, true);

           //     }

           //     scope.Complete();
           // }
            return response;
        }
    }
}

