using System;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;
using System.Net.Http;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;
using WebFreight.Web.DataContracts;
using System.Data.Entity;
using Logitude.BL.QuoteModel.EntityLists;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using Logitude.SystemLogs;
using Logitude.Extensions;
using Simplog.Data.QuoteModel.Repositories;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalQuoteController : ApiController
    {
        [HttpGet]
        [Route("DigitalQuote/GetSingle")]
        public HttpResponseMessage GetSingle(string id, string cardId)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var quoteIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, id);
                id = quoteIdAndTenant.Item1;
                tenant = quoteIdAndTenant.Item2;
                email = quoteIdAndTenant.Item3;
                QuoteQuery quoteQuery = new QuoteQuery(tenant);
                QuotePM quotePM = quoteQuery.GetSinglePM(id, tenant);

                var cardIds = cardId?.Split(',');
                if (string.IsNullOrWhiteSpace(cardId) || cardIds.Contains(quotePM.CustomerId) || cardIds.Contains(quotePM.AgentId))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, quotePM);
                }

                throw new AutenticationException("Sorry! you are not authorized to read data!");

            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        [Route("DigitalInvoice/GetByFilters")]
        public HttpResponseMessage GetByFilters(GeneralFilters newFilters)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);

                newFilters.Tenant = authToken.Tenant;
                var aRInvoiceQuery = new QuoteQuery(authToken.Tenant);
                var entityLists = aRInvoiceQuery.GetByFilters(newFilters);
                var res = entityLists.GetPaged(newFilters.PageIndex, newFilters.PageSize);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalQuote/[action]")]
        public HttpResponseMessage GetFromToCountryFilters(string cardId, string cardType, string searchType, string searchText = "")
        {
            int tenant = 0;
            string email = "";

            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);

                if (string.IsNullOrWhiteSpace(searchText) || searchText.Length < 3)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new List<FilterSearchResponse>());
                }

                bool isFrom = searchType.Equals("From", StringComparison.InvariantCultureIgnoreCase);
                var quoteRepository = new QuoteRepository(authToken.Tenant);
                var result = new QuoteQuery(quoteRepository).GetFromToCountryFilters(authToken.Tenant, cardType, cardId, isFrom, searchText);
                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}