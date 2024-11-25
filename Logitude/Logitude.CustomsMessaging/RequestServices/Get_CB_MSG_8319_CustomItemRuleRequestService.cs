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
using UnifreightIIG.Common.ClassifGuidanceDetailsServiceReference;
using UnifreightIIG.Common.CurrencyRateServiceReference;
using UnifreightIIG.Common.CustomItemClassifGuidanceServiceReference;
using UnifreightIIG.Common.CustomItemDetailsServiceReference;
using UnifreightIIG.Common.CustomItemRuleServiceReference;
using RequestContentHeader = UnifreightIIG.Common.CustomItemDetailsServiceReference.RequestContentHeader;


namespace Logitude.CustomsMessaging.RequestServices
{
    public class Get_CB_MSG_8319_CustomItemRuleRequestService : RequestServiceBase<CB_NG_8319_CustomItemRuleIn, CustomItemRuleRequestParams>
    {
        public override CB_NG_8319_CustomItemRuleIn GetRequest(CustomItemRuleRequestParams requestParams)
        {
            CB_NG_8319_CustomItemRuleIn myMsg = new CB_NG_8319_CustomItemRuleIn();

            myMsg.CIRuleIn = new UnifreightIIG.Common.CustomItemRuleServiceReference.CustomsBookItemHeaderIn() 
            {
                customsItemId = requestParams.customsItemId,
                customsItemIdSpecified = true,
                validToDate = requestParams.validToDate
            };

            
            
            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "כללים - ספר סיווג";
            return myMsg;

        }
    }
}
