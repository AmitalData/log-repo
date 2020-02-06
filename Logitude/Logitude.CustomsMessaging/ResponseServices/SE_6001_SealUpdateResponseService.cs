using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SealUpdateServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class SE_6001_SealUpdateResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, INF_MSG_Generic, CargoSealsRequestParams>
    {
        public override INF_MSG_GenericResponseData GetResponse(INF_MSG_Generic customResponse, CargoSealsRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(INF_MSG_Generic customResponse, CargoSealsRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var cargoSealIdentifierQueryService = new CargoSealIdentifierQueryService(dbContext);
            var cargoSealIdentifierUpdateService = new CargoSealIdentifierUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;

            CargoSealIdentifierPM cargoSealIdentifierPM = cargoSealIdentifierQueryService.GetSingle(requestParams.CargoSealIdentifierId, false, false);
            if (cargoSealIdentifierPM == null)
            {
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception[0].ExeptionDescription;
                return;
            }

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                cargoSealIdentifierPM.Status = "2";
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception[0].ExeptionDescription;
            }
            else
            {
                cargoSealIdentifierPM.Status = "1";
                this.MyResponseData.UserMessage = "התקבלה תשובה תקינה והסגר עודכן";
            }

            cargoSealIdentifierUpdateService.Update(cargoSealIdentifierPM, true);
        }
    }
}
