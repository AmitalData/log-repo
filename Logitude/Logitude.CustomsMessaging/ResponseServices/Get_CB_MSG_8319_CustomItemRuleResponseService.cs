using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClassifGuidanceDetailsServiceReference;
using UnifreightIIG.Common.CustomItemClassifGuidanceServiceReference;
using UnifreightIIG.Common.CustomItemLegalDemandsServiceReference;
using UnifreightIIG.Common.CustomItemRuleServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class Get_CB_MSG_8319_CustomItemRuleResponseService : ResponseServiceBase<CustomItemRuleResponseData, CB_NG_8319_CustomItemRuleOut, CustomItemRuleRequestParams>
    {
        public override void Update(CB_NG_8319_CustomItemRuleOut customResponse, CustomItemRuleRequestParams requestParams)
        {

            //if(customResponse?.CIClassifGuidanceOut == null )
            //{
            //    this.MyResponseData = new CustomItemRuleResponseData();
            //    this.MyResponseData.Succeeded = true;
            //    this.MyResponseData.HasException = false;
            //    this.MyResponseData.UserMessage = "לא התקבלו הנחיות סיווג";
            //    return;
            //}
            //this.MyResponseData = new CustomItemRuleResponseData();

            //foreach (var x in customResponse.CIClassifGuidanceOut)
            //{
            //    var item = new CustomItemClassifGuidanceResult
            //    {
            //        classificationGuidanceNumber = x?.classificationGuidanceNumber,
            //        title = x?.title,
            //        classificationGuidanceTypeName = x?.classificationGuidanceTypeName,
            //        fullClassification = x?.fullClassification,
            //        publicationDate = x?.publicationDate,
            //        customsItemId = x?.CustomsItemID
            //    };

            //    this.MyResponseData.CustomItemClassifGuidanceList.Add(item);
            //}

            //this.MyResponseData.Succeeded = true;
            //this.MyResponseData.HasException = false;
            //this.MyResponseData.UserMessage = "התקבלו הנחיות סיווג בהצלחה";

        }

        public override CustomItemRuleResponseData GetResponse(CB_NG_8319_CustomItemRuleOut customResponse, CustomItemRuleRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
