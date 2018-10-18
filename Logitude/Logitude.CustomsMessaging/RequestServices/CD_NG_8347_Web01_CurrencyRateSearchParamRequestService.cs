using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using UnifreightIIG.Common.CurrencyRateServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class CD_NG_8347_Web01_CurrencyRateSearchParamRequestService : RequestServiceBase<CD_NG_8347_Web01_CurrencyRateSearchParam, CD_NG_8347_Web01_CurrencyRateSearchRequestParams>
    {

        public override void ManipulateRequestParams(CD_NG_8347_Web01_CurrencyRateSearchRequestParams requestParams)
        {

            return;

            var srverTime = (new DualQueryService(AmitalContext.GetContext(requestParams.Tenant))).GetServerDateTime();
            var FuturePaymentDateTime = srverTime.GetValueOrDefault().AddMinutes(5);
            if (FuturePaymentDateTime.Subtract( srverTime.GetValueOrDefault()) > TimeSpan.FromMinutes(1))
            {
                if (requestParams.RequestVIA == SendRequestVIA.Default) {
                    requestParams.RequestVIA = DefaultMessageController.Via(requestParams.Tenant, requestParams.MainInterfaceCode, requestParams.RequestVIA);
                }

                switch (requestParams.RequestVIA)
                {
                    case SendRequestVIA.WebServiceInteractive:
                        requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                        break;
                    case SendRequestVIA.WebServiceBatch:
                        break;
                    case SendRequestVIA.DCABatch:
                        break;
                    case SendRequestVIA.Default:
                    default:
                        
                        throw new System.Exception("should not be SendRequestVIA.Default !!!!");
                        break;
                }
                requestParams.RequestVIAChangeDue = "הבקשה תשלח בעתיד";
                requestParams.FutureSendDateTime = FuturePaymentDateTime;
            }


            base.ManipulateRequestParams(requestParams);
        }

        public override CD_NG_8347_Web01_CurrencyRateSearchParam GetRequest(CD_NG_8347_Web01_CurrencyRateSearchRequestParams requestParams)
        {
            //Build request 8327 - Currency Rate Search Param
            var myCD_NG_8347_Web01_CurrencyRateSearchParam = new CD_NG_8347_Web01_CurrencyRateSearchParam();
            myCD_NG_8347_Web01_CurrencyRateSearchParam.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myCD_NG_8347_Web01_CurrencyRateSearchParam.CurrencyRate = new CD_NG_8347_Web01_CurrencyRateSearchParamCurrencyRate();
            myCD_NG_8347_Web01_CurrencyRateSearchParam.CurrencyRate.currencyTypeID = requestParams.CurrencyTypeId;
            myCD_NG_8347_Web01_CurrencyRateSearchParam.CurrencyRate.fromDate = (DateTime)requestParams.FromDate;
            myCD_NG_8347_Web01_CurrencyRateSearchParam.CurrencyRate.toDate = (DateTime)requestParams.ToDate;

            DateTime fromDateOnly = (DateTime)requestParams.FromDate;
            DateTime toDateOnly = (DateTime)requestParams.ToDate;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "שערי מטבע " + fromDateOnly.Date.ToString("dd/MM/yyyy") + "-" + toDateOnly.Date.ToString("dd/MM/yyyy");

            return myCD_NG_8347_Web01_CurrencyRateSearchParam;
        }
    }
}
