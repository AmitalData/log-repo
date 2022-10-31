using System;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;
using System.Net.Http;
using WebFreight.Web.Controllers.DigitalPortal.Models;
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

                if (quotePM.CustomerId == cardId || quotePM.AgentId == cardId || string.IsNullOrWhiteSpace(cardId))
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
        [Route("DigitalQuote/GetByFilters")]
        public HttpResponseMessage GetByFilters(GeneralFilters newFilters)
        {
            int tenant = 0;
            string email = string.Empty;
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);

                tenant = authToken.Tenant;
                email = authToken.Email;
                newFilters.Tenant = authToken.Tenant;
                var quoteQuery = new QuoteQuery(authToken.Tenant);
                var entityLists = quoteQuery.GetByFilters(newFilters);

                var response = new ServiceResponse();
                if (newFilters.GetCount)
                {
                    response.Count = entityLists.Count();
                }

                entityLists = QueryableExtensions.Skip(entityLists, () => newFilters.PageIndex);
                entityLists = QueryableExtensions.Take(entityLists, () => newFilters.PageSize);

                List<QuoteList> listQuery = entityLists.ToList();

                response.Result = listQuery;

                var reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
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