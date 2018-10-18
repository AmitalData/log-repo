using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CustomItemLegalDemandsServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class CB_NG_8316_CustomItemLegalDemandsRequestService : RequestServiceBase<CB_NG_8316_CustomItemLegalDemandsIn, CustomItemLegalDemandsRequestParams>
    {
        public override CB_NG_8316_CustomItemLegalDemandsIn GetRequest(CustomItemLegalDemandsRequestParams requestParams)
        {
            var myCB_NG_8316_CustomItemLegalDemandsIn = new CB_NG_8316_CustomItemLegalDemandsIn();
            myCB_NG_8316_CustomItemLegalDemandsIn.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myCB_NG_8316_CustomItemLegalDemandsIn.CILegalDemandsIn = new CustomsBookItemHeaderIn();

            int customsBookType;
            int.TryParse(requestParams.CustomsBookType, out customsBookType);
            myCB_NG_8316_CustomItemLegalDemandsIn.CILegalDemandsIn.customsBookType = customsBookType;
            myCB_NG_8316_CustomItemLegalDemandsIn.CILegalDemandsIn.customsBookTypeSpecified = customsBookType > 0 ? true : false;
            myCB_NG_8316_CustomItemLegalDemandsIn.CILegalDemandsIn.classification = requestParams.ClassificationCode;
            myCB_NG_8316_CustomItemLegalDemandsIn.CILegalDemandsIn.classification = myCB_NG_8316_CustomItemLegalDemandsIn.CILegalDemandsIn.classification.Insert(10, "/");
            myCB_NG_8316_CustomItemLegalDemandsIn.CILegalDemandsIn.validToDate = (DateTime)requestParams.ValidToDate;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "דרישת חוקיות לפרט " + requestParams.ClassificationCode;

            return myCB_NG_8316_CustomItemLegalDemandsIn;
        }
    }
}
