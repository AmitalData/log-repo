using System;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;

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
    }
}