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
        [HttpPost]
        public HttpResponseMessage GetQuotesRequestsByFilters(QuotesRequestFilters quotesRequestFilters)
        {
            try
            {
                AuthenticationToken authenticationToken = GetAuthenticationToken();
                SecurityUtility.AuthenticationOnTenant(authenticationToken.Tenant);
                SecurityUtility.CheckSharedContactAuthentication(authenticationToken.Tenant, quotesRequestFilters.PartnerId);
                List<QuotesRequest> quotesRequests = new QuotesRequestService(authenticationToken.Tenant, quotesRequestFilters).Get();
                return Request.CreateResponse(HttpStatusCode.OK, quotesRequests);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateQuotesRequestAndSendEmailFeedback(QuotesRequestEmailFeedback emailFeedback)
        {
            try
            {
                AuthenticationToken authenticationToken = GetAuthenticationToken();
                SecurityUtility.AuthenticationOnTenant(authenticationToken.Tenant);
                SecurityUtility.CheckSharedContactAuthentication(authenticationToken.Tenant, emailFeedback.PartnerId);
                QuotesRequestService quotesRequestService = new QuotesRequestService(authenticationToken.Tenant, emailFeedback);
                quotesRequestService.UpdateQuotesRequestAndSendEmailFeedback(emailFeedback);
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static AuthenticationToken GetAuthenticationToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            return AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        }
    }
}