using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Common
{
    public class RoleExtendedController : ApiController
    {

    public HttpResponseMessage GetRolesForUser(string userId, int tenant)

        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (userId == "null")
            {
                userId = null;
            }
             RoleQuery roleQuery = new RoleQuery(tenant);
             List<RolePM> result = roleQuery.GetRolesByUser(userId, tenant);
             return Request.CreateResponse(HttpStatusCode.OK, result);

        }
    }
}