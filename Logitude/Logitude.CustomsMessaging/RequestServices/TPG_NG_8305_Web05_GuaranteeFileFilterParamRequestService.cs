// moran 5.7.15 - Task 13442
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.GuaranteeFileFilterParamServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class TPG_NG_8305_Web05_GuaranteeFileFilterParamRequestService
        : RequestServiceBase<TPG_NG_8305_Web05_GuaranteeFileFilterParam, GuaranteeRequestParams>
    {
        public override TPG_NG_8305_Web05_GuaranteeFileFilterParam GetRequest(GuaranteeRequestParams requestParams)
        {
            var myGuaranteeFileFilterParam = new TPG_NG_8305_Web05_GuaranteeFileFilterParam();
            myGuaranteeFileFilterParam.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myGuaranteeFileFilterParam.TPGIdentifier = new TPGIdentifier();

            myGuaranteeFileFilterParam.TPGIdentifier.fileNumber = requestParams.FileNumber;
            if (!string.IsNullOrWhiteSpace(requestParams.Numeral))
            {
                int numeral;
                int.TryParse(requestParams.Numeral, out numeral);
                myGuaranteeFileFilterParam.TPGIdentifier.numeral = numeral;
            }
            myGuaranteeFileFilterParam.TPGIdentifier.FillingNumber = null;
            if (!string.IsNullOrWhiteSpace(requestParams.GuranteeType))
            {
                int guranteeType;
                int.TryParse(requestParams.GuranteeType,out guranteeType);
                myGuaranteeFileFilterParam.GuranteeType = guranteeType;
            }
            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "שאילתא לערבויות " + requestParams.FileNumber;

            return myGuaranteeFileFilterParam;
        }
    }
}
