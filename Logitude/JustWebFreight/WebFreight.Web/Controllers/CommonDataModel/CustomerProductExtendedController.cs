using Logitude.BL.CommonDataModel.EntityQueries;
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

namespace WebFreight.Web.Controllers.CommonDataModel.Generated
{
    public class CustomerProductExtendedController : ApiController
    {
        public HttpResponseMessage GetCustomerProducts(string customerId, int tenant)
        {

            try
            {
                Authentication();
                CustomerProductQuery query = new CustomerProductQuery(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, query.GetCustomerProductPMsByCustomerId(customerId, tenant));
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

        }
    }
}