using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.AgentPaymentRequestServiceReference;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    //INF_MSG_GenericResponseData, 
   public class TSH_MSG2_PaymentOrderReplyAnalyzerService
        : MessageAnalyzerServiceBase<
        NewPaymentRequestParams, INF_MSG_GenericResponseData,
        TSH_MSG2_PaymentOrderReply, TSH_MSG2_3050_PaymentOrderReplyResponseService>, IMessageAnalyzerService
    {
        public TSH_MSG2_PaymentOrderReplyAnalyzerService()
            :base("")
        {}

#if false
        public override Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData AnalyzeToOverride()
        {
            var reData = new Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData();
            var requestParams = new Logitude.CustomsMessaging.Common.RequestParams.NewPaymentRequestParams();
            requestParams.Tenant = ResolveTenant();
            ///InsertPhysicalCheck(physicalCheckMessage);
            var customResponseService = new Logitude.CustomsMessaging.ResponseServices.TSH_MSG2_PaymentOrderReplyResponseService();
            customResponseService.Update(_CustomResponse, requestParams);
            
            if (customResponseService.MyResponseData == null)
            {
                //??
                return null;
            }
            reData.ApplicationID = customResponseService.MyResponseData.ApplicationID;
            reData.ExceptionMessage = customResponseService.MyResponseData.ExceptionMessage;
            reData.ApplicationID = customResponseService.MyResponseData.ApplicationID;
            reData.HasException = customResponseService.MyResponseData.HasException;
            reData.Succeeded = customResponseService.MyResponseData.Succeeded;
            return reData;
        }
#endif


        public override string GetObjectTableName()
        {
            return "Customs.PaymentOrder";
            ///ddd
            //return "Customs.PhysicalChecks";
        }

        public override int ResolveTenant()
        {
            var externalID = this._CustomResponse.PaymentOrderReply.PaymentDetails.externalID;
            var paymentID = this._CustomResponse.PaymentOrderReply.PaymentDetails.paymentID;
            return 1;//  TODO: -  GetTenant() yaron 
        }

        

        

        public override string GetLoggingEntityReference()
        {
            return _CustomResponse.PaymentOrderReply.PaymentDetails.paymentID.ToString() ;
        }

        
    }
}
