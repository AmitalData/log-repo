using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CargoQueryMessageServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    
    public class MN_NG_8240_CargoQuery_MessageRequestService
        : RequestServiceBase<MN_NG_8240_CargoQuery_Message, GenericRequestParams>
    {
        public override MN_NG_8240_CargoQuery_Message GetRequest(GenericRequestParams requestParams)
        {

            var myMN_NG_8240_CargoQuery_Message = new MN_NG_8240_CargoQuery_Message();
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var declarationQueryService = new DeclarationQueryService(dbContext);

            var declarationPM = declarationQueryService.GetSingle(requestParams.AppicationId, false, false);
            if (declarationPM == null)
            {
                throw new System.Exception("CargoQuery_MessageRequest: \nAppicationId:" + requestParams.AppicationId + " is missing");
            }

            myMN_NG_8240_CargoQuery_Message.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            if (declarationPM.Consignments.Count > 0)
            {
                int type;
                int.TryParse(declarationPM.Consignments[0].CargoTypeCode, out type);
                myMN_NG_8240_CargoQuery_Message.CargoIdentifier = new cargoIdentifier()
                {
                    cargoIdentifierType = type,
                    cargoIdentifierKey1 = declarationPM.Consignments[0].ManifestNumber,
                    cargoIdentifierKey2 = declarationPM.Consignments[0].SecondCargoID,
                    cargoIdentifierKey3 = declarationPM.Consignments[0].ThirdCargoID
                };
            }

            return myMN_NG_8240_CargoQuery_Message;
        }



    }
}
