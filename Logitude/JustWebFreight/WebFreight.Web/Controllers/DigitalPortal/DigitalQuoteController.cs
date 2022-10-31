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

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalQuoteController : ApiController
    {
        [HttpGet]
        [Route("DigitalQuote/GetSingle")]
        public IHttpActionResult GetSingle(string id, string cardId)
        {
            try
            {
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var quoteIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, id);
                id = quoteIdAndTenant.Item1;
                var tenant = quoteIdAndTenant.Item2;
                QuoteQuery quoteQuery = new QuoteQuery(tenant);
                QuotePM quotePM = quoteQuery.GetSinglePM(id, tenant);

                if (quotePM.CustomerId == cardId || quotePM.AgentId == cardId || string.IsNullOrWhiteSpace(cardId))
                {
                    return Ok(quotePM);
                }

                throw new AutenticationException("Sorry! you are not authorized to read data!");

            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpPost]
        [Route("DigitalQuote/GetByFilters")]
        public HttpResponseMessage GetByFilters(GeneralFilters newFilters)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);

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
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}