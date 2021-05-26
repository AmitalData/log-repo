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
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CRMModel.Extended
{
    public class QuotesRequestController : ApiController
    {


        public HttpResponseMessage PostFilteredQuotesRequests(QuotesRequestFilters quotesRequestFilters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckSharedContactAuthentication(tenant, quotesRequestFilters.PartnerId);
         
                return Request.CreateResponse(HttpStatusCode.OK, "");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



    }
}