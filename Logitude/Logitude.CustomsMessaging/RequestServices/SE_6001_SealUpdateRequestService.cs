using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SealUpdateServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class SE_6001_SealUpdateRequestService : RequestServiceBase<SE_NG_6001_MSG01_SealUpdateMessage, CargoSealsRequestParams>
    {
        public override SE_NG_6001_MSG01_SealUpdateMessage GetRequest(CargoSealsRequestParams requestParams)
        {
            var mySE_NG_6001_MSG01_SealUpdateMessage = new SE_NG_6001_MSG01_SealUpdateMessage();
            mySE_NG_6001_MSG01_SealUpdateMessage.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            //this._DbContext = CustomContext.GetContext(requestParams.Tenant);
            //ClaimQueryService myClaimQueryService = new ClaimQueryService(this._DbContext);
            //_ClaimPM = myClaimQueryService.GetSingle(requestParams.AppicationId, true, false);

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            //this.MyRequestSheetParam.EntityId1 = requestParams.AppicationId;
            this.MyRequestSheetParam.RequestDescription = "בקשה לעדכון סגרים";




            return mySE_NG_6001_MSG01_SealUpdateMessage;
        }
    }
}
