using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.PaymentFilterParamServiceReference;
using UnifreightIIG.Common.WarehouseBlockBalanceServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class TSH_NG_8286_Web02_PaymentListResponseService : ResponseServiceBase<
        TSH_NG_8285_Web01_PaymentResponseData,
        TSH_NG_8286_Web02_PaymentList,
        TSH_NG_8285_Web01_PaymentRequestParams>
    {
        
        public override void Update(TSH_NG_8286_Web02_PaymentList customResponse, TSH_NG_8285_Web01_PaymentRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData = new TSH_NG_8285_Web01_PaymentResponseData();
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception[0].ExeptionDescription;
                return;
            }

            this.MyResponseData = new TSH_NG_8285_Web01_PaymentResponseData();

            if (customResponse.PaymentsDetails != null)
            {
                List<PaymentsDetailsResult> paymentList = new List<PaymentsDetailsResult>();
                foreach (var paymentItem in customResponse.PaymentsDetails)
                {
                    var paymentDetails = new PaymentsDetailsResult();
                    paymentDetails.Agent = paymentItem.Agent;
                    paymentDetails.Importer = paymentItem.Importer;
                    paymentDetails.PaymentAmount = paymentItem.PaymentAmount;
                    paymentDetails.PaymentID = paymentItem.PaymentID;
                    paymentDetails.PaymentMethodType = paymentItem.paymentMethodName;
                    paymentDetails.PaymentType = paymentItem.PaymentTypeName;
                    paymentDetails.PaymentStatus = paymentItem.PaymentStatusName;
                    paymentDetails.EntityExternalID = paymentItem.EntityExternalID;
                    paymentDetails.EntityType = paymentItem.EntityType;
                    paymentList.Add(paymentDetails);
                }
                this.MyResponseData.PaymentsDetailsList = paymentList;
            }

            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "שליחת מסר שאילתא להוראות תשלום הצליחה";

        }

        public override TSH_NG_8285_Web01_PaymentResponseData GetResponse(TSH_NG_8286_Web02_PaymentList customResponse, TSH_NG_8285_Web01_PaymentRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
