using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole.MessageAnalyzer
{
    class TSH_MSG2_PaymentOrderReplyAnalyzerService
        : MessageAnalyzerServiceBase<UnifreightIIG.Common.AgentPaymentRequestServiceReference.TSH_MSG2_PaymentOrderReply>, IMessageAnalyzerService
    {
        public TSH_MSG2_PaymentOrderReplyAnalyzerService()
            :base("")
        {}
        public override Logitude.CustomsMessaging.ResponseData.INF_MSG_GenericResponseData AnalyzeToOverride()
        {
            var reData = new Logitude.CustomsMessaging.ResponseData.INF_MSG_GenericResponseData();
            var requestParams = new Logitude.CustomsMessaging.RequestParams.NewPaymentRequestParams();
            requestParams.Tenant = ResolveTenant();
            ///InsertPhysicalCheck(physicalCheckMessage);
            var customResponseService = new Logitude.CustomsMessaging.ResponseServices.TSH_MSG2_PaymentOrderReplyResponseService();
            customResponseService.Update(_CustomResponse, requestParams);
            if (customResponseService.MyResponseData == null)
            {
                //??
                return null;
            }
            reData.ExceptionMessage = customResponseService.MyResponseData.ExceptionMessage;
            reData.ApplicationID = customResponseService.MyResponseData.ApplicationID;
            reData.HasException = customResponseService.MyResponseData.HasException;
            reData.Succeeded = customResponseService.MyResponseData.Succeeded;
            return reData;
        }

        

        public override string GetObjectTableName()
        {
            ///ddd
            return "Customs.PhysicalChecks";
        }

        public override int ResolveTenant()
        {
            var externalID = this._CustomResponse.PaymentOrderReply.PaymentDetails.externalID;
            var paymentID = this._CustomResponse.PaymentOrderReply.PaymentDetails.paymentID;
            return 1;//  TODO: -  GetTenant() yaron 
        }
    }
}
