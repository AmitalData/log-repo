using Logitude.Customs.BL.BL;
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
using RequestContentHeader = UnifreightIIG.Common.CustomItemDetailsServiceReference.RequestContentHeader;


namespace Logitude.CustomsMessaging.RequestServices
{
    public class Get_CB_MSG_8323_ClassifGuidanceDetailsRequestService : RequestServiceBase<CB_NG_8323_ClassifGuidanceDetailsIn, GetClassifGuidanceDetailsRequestParams>
    {
        public override CB_NG_8323_ClassifGuidanceDetailsIn GetRequest(GetClassifGuidanceDetailsRequestParams requestParams)
        {
            CB_NG_8323_ClassifGuidanceDetailsIn myMsg = new CB_NG_8323_ClassifGuidanceDetailsIn();
           
            myMsg.ClassifGuidanceDetailsIn = new UnifreightIIG.Common.ClassifGuidanceDetailsServiceReference.CB_NG_8323_ClassifGuidanceDetailsInClassifGuidanceDetailsIn();

            myMsg.ClassifGuidanceDetailsIn.classificationGuidanceNumber = requestParams.ClassificationGuidanceNumber;
            myMsg.ClassifGuidanceDetailsIn.languageType = 1;
            myMsg.ClassifGuidanceDetailsIn.languageTypeSpecified = true;

            
            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "פרטי הנחיות סיווג";
            return myMsg;

        }
    }
}
