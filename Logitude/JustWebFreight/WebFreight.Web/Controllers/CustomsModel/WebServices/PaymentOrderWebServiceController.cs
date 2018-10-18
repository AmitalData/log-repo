using Logitude.BL.Security;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Def.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.Repsitories;
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
    public class PaymentOrderWebServiceController : ApiController
    {
        public HttpResponseMessage PostSendPaymentOrderRequest(GenericRequestParams requestParamsData)
        {
            try
            {
                INF_MSG_GenericResponseData responseData;
                var messagingService = new TSH_MSG6_AgentPaymentMessageService();
                responseData = messagingService.Send(requestParamsData);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetPaymentOrderByPaymentOrderConnection(string ConnectedEntityCode, string ConnectedEntityId, int tenant)
        {
            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);

                PaymentOrderConnectionTableQueryService connectionTableQueryService = new PaymentOrderConnectionTableQueryService(customContext);
                List<PaymentOrderConnectionTableList> connectionTables = connectionTableQueryService.GetPaymentOrderConnectionTable(ConnectedEntityCode, ConnectedEntityId, tenant);

                PaymentOrderListQueryService listService = new PaymentOrderListQueryService(customContext);
                List<PaymentOrderList> paymentOrderList = listService.GetPaymentOrderList(connectionTables);

                return Request.CreateResponse(HttpStatusCode.OK, paymentOrderList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}