using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
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

namespace WebFreight.Web.Controllers.QuoteModel.Extended
{
    public class QuoteDocumentVersionExtendedController : ApiController
    {
        public HttpResponseMessage GetQuoteDocumentVersionByQuoteId(string quoteId)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("QuoteTemplate", "READ", authToken.Tenant);
                QuoteDocumentVersionQuery quoteDocumentVersionQuery = new QuoteDocumentVersionQuery(authToken.Tenant);
                QuoteDocumentVersionPM quoteDocumentVersionPM = quoteDocumentVersionQuery.GetQuoteDocumentVersionPMByQuoteId(quoteId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, quoteDocumentVersionPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }



        }

    }
}