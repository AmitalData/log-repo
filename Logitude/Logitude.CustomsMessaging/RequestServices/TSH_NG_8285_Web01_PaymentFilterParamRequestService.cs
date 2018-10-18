using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.PaymentFilterParamServiceReference;


namespace Logitude.CustomsMessaging.RequestServices
{
    public class TSH_NG_8285_Web01_PaymentFilterParamRequestService
        : RequestServiceBase
        <TSH_NG_8285_Web01_PaymentFilterParam, TSH_NG_8285_Web01_PaymentRequestParams>
    {
        public override TSH_NG_8285_Web01_PaymentFilterParam GetRequest(TSH_NG_8285_Web01_PaymentRequestParams requestParams)
        {
            //Build request TSH_NG_8285_Web01_PaymentFilterParam 
            var myTSH_NG_8285_Web01_PaymentFilterParam = new TSH_NG_8285_Web01_PaymentFilterParam();
            myTSH_NG_8285_Web01_PaymentFilterParam.TSHPaymentParams = QueryDetails(requestParams);
            myTSH_NG_8285_Web01_PaymentFilterParam.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            myTSH_NG_8285_Web01_PaymentFilterParam.TSHPaymentParams.PaymentID = null;
            if (!string.IsNullOrWhiteSpace(requestParams.PaymentID))
            {
                myTSH_NG_8285_Web01_PaymentFilterParam.TSHPaymentParams.PaymentID = requestParams.PaymentID;
            }


            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "שאילתא להוראות תשלום " + requestParams.PaymentID;

            return myTSH_NG_8285_Web01_PaymentFilterParam;
        }

        private TSH_NG_8285_Web01_PaymentFilterParamTSHPaymentParams QueryDetails(TSH_NG_8285_Web01_PaymentRequestParams requestParams)
        {
            ICustomContext customContext = CustomContext.GetContext(requestParams.Tenant);
            PaymentOrderQueryService paymentQueryService = new PaymentOrderQueryService(customContext);
            var paymentFilterParamTSHPaymentParams = new TSH_NG_8285_Web01_PaymentFilterParamTSHPaymentParams();
            int tempInt;
            if (!string.IsNullOrWhiteSpace(requestParams.AgentExternalId))
            {
                int.TryParse(requestParams.AgentExternalId, out tempInt);
                paymentFilterParamTSHPaymentParams.AgentExternalID = tempInt;
                paymentFilterParamTSHPaymentParams.AgentExternalIDSpecified = true;
            }

            if (!string.IsNullOrWhiteSpace(requestParams.AgentID))
            {
                int.TryParse(requestParams.AgentID, out tempInt);
                paymentFilterParamTSHPaymentParams.AgentID = tempInt;
                paymentFilterParamTSHPaymentParams.AgentIDSpecified = true;
            }

            if (!string.IsNullOrWhiteSpace(requestParams.BankID))
            {
                int.TryParse(requestParams.BankID, out tempInt);
                paymentFilterParamTSHPaymentParams.BankID = tempInt;
                paymentFilterParamTSHPaymentParams.BankIDSpecified = true;
            }
            if (!string.IsNullOrWhiteSpace(requestParams.BranchID))
            {
                int.TryParse(requestParams.BranchID, out tempInt);
                paymentFilterParamTSHPaymentParams.BranchID = tempInt;
                paymentFilterParamTSHPaymentParams.BranchIDSpecified = true;
            }

            paymentFilterParamTSHPaymentParams.EffectiveDateFrom = requestParams.EffectiveDateFrom;
            if (paymentFilterParamTSHPaymentParams.EffectiveDateFrom.HasValue)
            {
                paymentFilterParamTSHPaymentParams.EffectiveDateFromSpecified = true;
            }
            paymentFilterParamTSHPaymentParams.EffectiveDateTp = requestParams.EffectiveDateTo;
            if (paymentFilterParamTSHPaymentParams.EffectiveDateTp.HasValue)
            {
                paymentFilterParamTSHPaymentParams.EffectiveDateTpSpecified = true;
            }
            paymentFilterParamTSHPaymentParams.EntityID = requestParams.EntityExternalID;

            if (!string.IsNullOrWhiteSpace(requestParams.EntityType))
            {
                int.TryParse(requestParams.EntityType, out tempInt);
                paymentFilterParamTSHPaymentParams.EntityType = tempInt;
                paymentFilterParamTSHPaymentParams.EntityTypeSpecified = true;
            }
            paymentFilterParamTSHPaymentParams.ExtertnalID = requestParams.ExternalID;

            if (!string.IsNullOrWhiteSpace(requestParams.PaymentID))
            {
                int.TryParse(requestParams.PaymentID, out tempInt);
                paymentFilterParamTSHPaymentParams.PaymentID = tempInt.ToString();
            }
            if (!string.IsNullOrWhiteSpace(requestParams.PaymentMethodType))
            {
                int.TryParse(requestParams.PaymentMethodType, out tempInt);
                paymentFilterParamTSHPaymentParams.PaymentMethodType = tempInt;
                paymentFilterParamTSHPaymentParams.PaymentMethodTypeSpecified = true;
            }
            if (!string.IsNullOrWhiteSpace(requestParams.PaymentAmount))
            {
                int.TryParse(requestParams.PaymentAmount, out tempInt);
                paymentFilterParamTSHPaymentParams.PaymentAmount = tempInt.ToString();
            }

            paymentFilterParamTSHPaymentParams.paymentDateFrom = requestParams.paymentDateFrom;
            if (paymentFilterParamTSHPaymentParams.paymentDateFrom.HasValue)
            {
                paymentFilterParamTSHPaymentParams.paymentDateFromSpecified = true;
            }
            if (!string.IsNullOrWhiteSpace(requestParams.PaymentOrderStatus))
            {
                int.TryParse(requestParams.PaymentOrderStatus, out tempInt);
                paymentFilterParamTSHPaymentParams.PaymentOrderStatus = tempInt;
                paymentFilterParamTSHPaymentParams.PaymentOrderStatusSpecified = true;
            }
            if (!string.IsNullOrWhiteSpace(requestParams.PaymentProcess))
            {
                int.TryParse(requestParams.PaymentProcess, out tempInt);
                paymentFilterParamTSHPaymentParams.PaymentProcess = tempInt;
                paymentFilterParamTSHPaymentParams.PaymentProcessSpecified = true;
            }
            if (!string.IsNullOrWhiteSpace(requestParams.PaymentType))
            {
                int.TryParse(requestParams.PaymentType, out tempInt);
                paymentFilterParamTSHPaymentParams.PaymentType = tempInt;
                paymentFilterParamTSHPaymentParams.PaymentTypeSpecified = true;
            }

            paymentFilterParamTSHPaymentParams.QueryDate = DateTime.Now;
            paymentFilterParamTSHPaymentParams.QueryTime = DateTime.Now;
            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
            this.MyRequestSheetParam.EntityId1 = requestParams.PaymentID;
            this.MyRequestSheetParam.RequestDescription = "שאילתא להוראת תשלום " + requestParams.PaymentID;

            return paymentFilterParamTSHPaymentParams;
        }

    }
}
