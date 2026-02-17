using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
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

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CustomsExchangeRatesExtendedController : ApiController
    {
        public HttpResponseMessage GetCustomsExchangeRateForCurrencyAndDate(string currencyTypeCode, DateTime date)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                CustomsExchangeRateQueryService query = new CustomsExchangeRateQueryService(customContext);
                List<CustomsExchangeRatePM> rates = query.GetExchangeRateByCurrencyAndDate(currencyTypeCode, date, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, rates);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        

       public HttpResponseMessage GetCustomsExchangeRateForDate( DateTime date)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                CustomsExchangeRateQueryService query = new CustomsExchangeRateQueryService(customContext);
                List<CustomsExchangeRatePM> rates = query.GetCustomsExchangeRateForDate( date, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, rates);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}