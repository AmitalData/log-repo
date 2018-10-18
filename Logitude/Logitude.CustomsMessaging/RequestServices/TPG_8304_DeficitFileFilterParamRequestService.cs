using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.DeficitFileServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class TPG_8304_DeficitFileFilterParamRequestService
        : RequestServiceBase<TPG_NG_8304_Web03_DeficitFileFilterParam, DeficitFileFilterRequestParams>
    {
        public override TPG_NG_8304_Web03_DeficitFileFilterParam GetRequest(DeficitFileFilterRequestParams requestParams)
        {
            var myDeficitFileFilterParam = new TPG_NG_8304_Web03_DeficitFileFilterParam();
            myDeficitFileFilterParam.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myDeficitFileFilterParam.TPGIdentifier = new TPGIdentifier();

            myDeficitFileFilterParam.TPGIdentifier.fileNumber = requestParams.FileNumber;
            int numeral;
            int.TryParse(requestParams.Numeral, out numeral);
            myDeficitFileFilterParam.TPGIdentifier.numeral = numeral;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "שאילתא לגרעונות נתוני קלט " + requestParams.FileNumber;

            return myDeficitFileFilterParam;
        }
    }
}
