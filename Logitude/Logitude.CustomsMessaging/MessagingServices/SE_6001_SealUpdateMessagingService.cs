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
using UnifreightIIG.Common.SealUpdateServiceReference;
using UnifreightIIG.Common.TheGateway;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class SE_6001_SealUpdateMessagingService : MessagingServiceBase<
        CargoSealsRequestParams, INF_MSG_GenericResponseData,
        SE_NG_6001_MSG01_SealUpdateMessage, INF_MSG_Generic,
        SE_6001_SealUpdateRequestService, SE_6001_SealUpdateResponseService,
        RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "6001"; }
        }

        protected override INF_MSG_GenericResponseData GetIIGBLExceptionFromReponseHeader(INF_MSG_Generic customsResponse)
        {

            return base.GetIIGBLExceptionFromReponseHeader(customsResponse);
        }

        protected override INF_MSG_Generic CallWS(SE_NG_6001_MSG01_SealUpdateMessage customRequest, CargoSealsRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();
       
            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ISealUpdateOperation>()
                    .SealUpdate(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);

            }
        
           using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                if (response.ResponseContentHeader.Exception != null)
                {
                    ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);

                    var cargoSealIdentifierQueryService = new CargoSealIdentifierQueryService(dbContext);
                    var cargoSealIdentifierUpdateService = new CargoSealIdentifierUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
                    CargoSealIdentifierPM cargoSealIdentifierPM = cargoSealIdentifierQueryService.GetSingle(requestParams.CargoSealIdentifierId, false, false);
                    cargoSealIdentifierPM.Status = "2";
                    cargoSealIdentifierPM.ChangeSetOp = ChangeSetOperation.Update;
                    cargoSealIdentifierUpdateService.Update(cargoSealIdentifierPM, true);

                }

                scope.Complete();
            }
            return response;
        }
    }
}

