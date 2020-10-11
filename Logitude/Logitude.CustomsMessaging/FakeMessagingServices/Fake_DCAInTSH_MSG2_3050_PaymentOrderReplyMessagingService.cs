using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.AgentPaymentRequestServiceReference;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    class Fake_DCAInTSH_MSG2_3050_PaymentOrderReplyMessagingService
    {
        private ResponseContentHeader _responseContentHeader;
        private PaymentOrderReply _paymentOrderReply;
        private int paymentStatus=0;
        private int paymentProcess=0;

        internal TSH_MSG2_PaymentOrderReply GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {
            dynamic data = JObject.Parse(requestParamsData.TestCase.Param1);
            paymentStatus = data.paymentStatus;
            paymentProcess = data.paymentProcess;

            SetRequestContentHeader();
            SetPaymentOrderReply(requestParamsData);

            TSH_MSG2_PaymentOrderReply fake = new TSH_MSG2_PaymentOrderReply
            {
                PaymentOrderReply = _paymentOrderReply,
                ResponseContentHeader = _responseContentHeader
            };
            return fake;
        }
        public void SetRequestContentHeader()
        {
            _responseContentHeader = new ResponseContentHeader
            {
                TransmitionDateTime = DateTime.Now
            };
        }
        public void SetPaymentOrderReply(GenericRequestParams requestParamsData)
        {
            _paymentOrderReply = new PaymentOrderReply();
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsData.Tenant);
            ConsignmentQueryService consignmentQueryService = new ConsignmentQueryService(requestParamsData.Tenant);
            DeclarationPM _dec = declarationQueryService.GetSingle(requestParamsData.AppicationId, false, false);





            ConsignmentPM _con = consignmentQueryService.GetSingle(_dec.Id, 1, false, false);
            _paymentOrderReply.PaymentDetails = new PaymentDetails();
            //_paymentOrderReply.PaymentDetails.paymentID = Convert.ToInt32(_dec.DeclarationNumber.Substring(_dec.DeclarationNumber.Length-4)); // Or Use counter?\
            _paymentOrderReply.PaymentDetails.paymentID = Convert.ToInt32(DateTime.Now.Ticks.ToString().Substring(10, 7));
            _paymentOrderReply.customsHouse = 2;
            _paymentOrderReply.paymentProcess = 1;
            _paymentOrderReply.paymentOrderType = 1;
            _paymentOrderReply.paymentStatus = ( paymentStatus != 0 ) ? paymentStatus : 3;
            _paymentOrderReply.paymentProcess = (paymentProcess != 0) ? paymentProcess : 1;
            _paymentOrderReply.ConnectedEntity = new ConnectedEntity
            {
                entityType = 1055,
                entityIdKey1 = _dec.DeclarationNumber
            };
            _paymentOrderReply.taxParagraph = new TaxParagraph[1];
            _paymentOrderReply.taxParagraph[0] = new TaxParagraph
            {
                paragraphType = 15,
                amount = 99
            };


        }


    }
}
