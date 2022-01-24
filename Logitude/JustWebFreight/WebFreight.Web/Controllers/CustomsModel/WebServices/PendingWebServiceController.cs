using Logitude.BL.Security;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.WebServices
{
    public class PendingWebServiceController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage DeclarationsforBulkFeed(string courierMasterId, string goodsDescription, string weightFrom, string weightTo, string incotermCode, string searchFilter, string totalInvoice, string fastIndividualProcess, int? skip = null, int? take = null)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                var res =  new DeclarationRepository(tenant).GetforPendingBulkFeed(courierMasterId, goodsDescription, weightFrom, weightTo, incotermCode, searchFilter, totalInvoice, fastIndividualProcess, skip, take);

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        [HttpPost]
        public HttpResponseMessage BulkFeeding([FromBody]AddMultiPendingsRequestParams requestParamsData)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string res = new DCAInUCBUCADPE_MsgMessagingService().CreateCRS(authToken.Tenant, requestParamsData.listPending, requestParamsData.listPendingRemark, requestParamsData.declarationIdsList);

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }
}