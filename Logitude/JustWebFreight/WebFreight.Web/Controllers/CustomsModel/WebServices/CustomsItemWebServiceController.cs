using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class CustomsItemWebServiceController : ApiController
    {
        [HttpPost]

        public HttpResponseMessage SendCustomsMessage8413([FromBody] CD_NG_8314_Web01_CustomsItemDetailsRequestParams requestParams)
        {
            try
            {
                var resData = new DCAInCB_MSG_8314_8888_CustomItemDetailsHeaderMessagingService().Send(requestParams);
                return Request.CreateResponse(HttpStatusCode.OK, resData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


       [HttpPost]
        public HttpResponseMessage CloseRequests([FromBody] string[] ids)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);                

                new LogisticActionRequestQueryService(authToken.Tenant)
                    .CloseRequests(ids);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}