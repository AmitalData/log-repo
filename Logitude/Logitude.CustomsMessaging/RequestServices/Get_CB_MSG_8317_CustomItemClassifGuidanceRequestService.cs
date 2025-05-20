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
    public class Get_CB_MSG_8317_CustomItemClassifGuidanceRequestService : RequestServiceBase<CB_NG_8317_CustomItemClassifGuidanceIn, GetCustomItemClassifGuidanceRequestParams>
    {
        public override CB_NG_8317_CustomItemClassifGuidanceIn GetRequest(GetCustomItemClassifGuidanceRequestParams requestParams)
        {
            CB_NG_8317_CustomItemClassifGuidanceIn myMsg = new CB_NG_8317_CustomItemClassifGuidanceIn();
           
            myMsg.CIClassifGuidanceIn = new UnifreightIIG.Common.CustomItemClassifGuidanceServiceReference.CustomsBookItemHeaderIn();
           
            myMsg.CIClassifGuidanceIn.customsItemId = requestParams.CustomItemId;
            myMsg.CIClassifGuidanceIn.customsItemIdSpecified = true;
            myMsg.CIClassifGuidanceIn.validToDate= requestParams.ValidToDate;


            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "הנחיות סיווג";
            return myMsg;

        }
    }
}
