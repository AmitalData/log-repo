using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.CustomWebServices;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class PaymentMessagesController: ApiController
    {

        public HttpResponseMessage PostPaymentOrderQueryRequest(TSH_NG_8285_Web01_PaymentRequestParams requestParams)
        {
            try
            {
                TSH_NG_8285_Web01_PaymentResponseData responseData = null;


                // use messageing service

                var service = new TSH_NG_8285_Web01_PaymentFilterParamMessagingService();
                responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostMasavPaymentsToAgentRequest(MasavPaymentsToAgentRequestParams requestParams)
        {
            try
            {
                MasavPaymentsToAgentResponseData responseData = null;


                // use messageing service

                var service = new TSH_8368_MasavPaymentsToAgentMessagingService();
                responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostNewPaymentRequest(NewPaymentRequestParams requestParams)
        {
            try
            {
                PaymentOrderReplyResponseData responseData = null;
                var service = new TSH_NG_3053_MSG8_AgentPaymentRequestMessageService();
                responseData = service.Send(requestParams);

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}
