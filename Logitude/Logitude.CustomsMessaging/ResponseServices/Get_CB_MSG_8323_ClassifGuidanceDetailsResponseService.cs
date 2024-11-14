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

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class Get_CB_MSG_8323_ClassifGuidanceDetailsResponseService : ResponseServiceBase<GetClassifGuidanceDetailsResponseData, CB_NG_8323_ClassifGuidanceDetailsOut, GetClassifGuidanceDetailsRequestParams>
    {
        public override void Update(CB_NG_8323_ClassifGuidanceDetailsOut customResponse, GetClassifGuidanceDetailsRequestParams requestParams)
        {

            //if(customResponse?.CIClassifGuidanceOut == null )
            //{
            //    this.MyResponseData = new GetClassifGuidanceDetailsResponseData();
            //    this.MyResponseData.Succeeded = true;
            //    this.MyResponseData.HasException = false;
            //    this.MyResponseData.UserMessage = "לא התקבלו הנחיות סיווג";
            //    return;
            //}
            //this.MyResponseData = new GetClassifGuidanceDetailsResponseData();

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

        public override GetClassifGuidanceDetailsResponseData GetResponse(CB_NG_8323_ClassifGuidanceDetailsOut customResponse, GetClassifGuidanceDetailsRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
