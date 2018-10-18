using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CustomsBookServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class CBC_NG_8361_MSG01_CustomsBookInRequestService
        : RequestServiceBase<CBC_NG_8361_MSG01_CustomsBookIn, CustomsBookInRequestParams>
    {
        public override CBC_NG_8361_MSG01_CustomsBookIn GetRequest(CustomsBookInRequestParams requestParams)
        {
            var myCBC_NG_8361_MSG01_CustomsBookIn = new CBC_NG_8361_MSG01_CustomsBookIn();
            myCBC_NG_8361_MSG01_CustomsBookIn.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myCBC_NG_8361_MSG01_CustomsBookIn.CustomsBookIn = new CBC_NG_8361_MSG01_CustomsBookInCustomsBookIn()
            {
                fromDate = requestParams.fromDate,
                fromDateSpecified = requestParams.fromDateSpecified,
                isGetHistoricalData = requestParams.isGetHistoricalData,
                toDate = requestParams.toDate,
                toDateSpecified = requestParams.toDateSpecified,
            };

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "עדכון ספר סיווג " + requestParams.fromDate.Value.ToString("dd/MM/yyyy") + "-" + requestParams.toDate.Value.ToString("dd/MM/yyyy");

            return myCBC_NG_8361_MSG01_CustomsBookIn;

        }
    }
}