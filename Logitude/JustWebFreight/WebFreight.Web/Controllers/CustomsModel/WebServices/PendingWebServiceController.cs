using Logitude.Customs.Data.Repsitories;
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
        public HttpResponseMessage DeclarationsforBulkFeed(string goodsDescription, string weightFrom, string weightTo, string incotermCode, string SearchFilter, int? skip = null, int? take = null)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                var res =  new DeclarationRepository(tenant).GetforPendingBulkFeed(goodsDescription, weightFrom, weightTo, incotermCode, SearchFilter, skip, take);


                return Request.CreateResponse(HttpStatusCode.OK, res);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }
}