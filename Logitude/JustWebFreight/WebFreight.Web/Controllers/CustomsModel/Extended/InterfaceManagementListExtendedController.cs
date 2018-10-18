using WebFreight.Web.Security;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Logitude.Customs.Data.EntityLists;
using System.Net;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class InterfaceManagementListExtendedController : ApiController
    {
        public HttpResponseMessage GetInterfaceManagementwithDefinition()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                var interfaceManagementQuery = new InterfaceManagementQueryService(customContext);
                List<InterfaceManagementList> interfaceManagments = interfaceManagementQuery.GetInterfaceManagementwithDefinition(tenant);


                return Request.CreateResponse(HttpStatusCode.OK, interfaceManagementQuery);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}