using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CargoQueryMessageServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class MN_NG_8240_CargoQueryRequestService
        : RequestServiceBase<MN_NG_8240_CargoQuery_Message, CargoQueryRequestParams>
    {
        public override MN_NG_8240_CargoQuery_Message GetRequest(CargoQueryRequestParams requestParams)
        {
            var myMN_NG_8240_CargoQuery_Message = new MN_NG_8240_CargoQuery_Message();
            int result;
            if (!int.TryParse(requestParams.CargoTypeCode, out result))
            {
                return null;
            }

            myMN_NG_8240_CargoQuery_Message.CargoIdentifier = new cargoIdentifier()
            {
                cargoIdentifierType = result,
                cargoIdentifierKey1 = requestParams.ManifestNumber,
                cargoIdentifierKey2 = requestParams.SecondCargoID,
                cargoIdentifierKey3 = requestParams.ThirdCargoID
            };

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = requestParams.LoggingEntityId;
            if (!string.IsNullOrEmpty(requestParams.DeclarationId))
            {
                this.MyRequestSheetParam.EntityId1 = requestParams.DeclarationId;
            }
            this.MyRequestSheetParam.RequestDescription = "שאילתא למצהר";

            return myMN_NG_8240_CargoQuery_Message;

        }
    }
}