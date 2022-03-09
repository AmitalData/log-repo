using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{

    public class LogisticActionServiceWebServiceController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SendCustomsMessage8410([FromBody] LogisticActionRequestRequestParams requestParams)
        {
            try
            {
                var myRequestMessagingService = new DCAInLG_NG_8410_LogisticActionRequestMessageMessagingService();
                var resData = "";//myRequestMessagingService.Send(requestParams);
                return Request.CreateResponse(HttpStatusCode.OK, resData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}