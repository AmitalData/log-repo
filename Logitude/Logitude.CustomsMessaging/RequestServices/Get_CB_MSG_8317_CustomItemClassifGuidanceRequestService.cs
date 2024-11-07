using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CurrencyRateServiceReference;
using UnifreightIIG.Common.CustomItemClassifGuidanceServiceReference;
using UnifreightIIG.Common.CustomItemDetailsServiceReference;
using RequestContentHeader = UnifreightIIG.Common.CustomItemDetailsServiceReference.RequestContentHeader;


namespace Logitude.CustomsMessaging.RequestServices
{
    public class Get_CB_MSG_8317_CustomItemClassifGuidanceRequestService : RequestServiceBase<CB_NG_8317_CustomItemClassifGuidanceIn, GenericRequestParams>
    {
        public override CB_NG_8317_CustomItemClassifGuidanceIn GetRequest(GenericRequestParams requestParams)
        {
            var myMsg = new CB_NG_8317_CustomItemClassifGuidanceIn();
            //myMsg.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            //myMsg.CIDetailsHeaderIn = new CustomsBookItemHeaderIn();
            //if (requestParams.Classification.Length > 10)
            //{
            //    requestParams.Classification = requestParams.Classification.Substring(0, 10);
            //}
            //myMsg.CIDetailsHeaderIn.classification = requestParams.Classification;
            //myMsg.CIDetailsHeaderIn.customsBookType = requestParams.CustomsBookType;
            //myMsg.CIDetailsHeaderIn.validToDate = requestParams.ValidToDate;
            //myMsg.CIDetailsHeaderIn.customsBookTypeSpecified = true;
            //this.MyRequestSheetParam = new RequestSheetParam();
            //this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsItem");
            //this.MyRequestSheetParam.EntityId1 = requestParams.LoggingEntityId2;
            ////this.MyRequestSheetParam.CustomFileNo= requestParams.c
            //this.MyRequestSheetParam.RequestDescription = "נתוני פרט מכס";
            return myMsg;

        }
    }
}
