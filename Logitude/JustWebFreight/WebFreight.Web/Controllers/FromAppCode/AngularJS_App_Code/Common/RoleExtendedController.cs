using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
using Logitude.BL.GlobalModel.EntityQueries;

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
            RoleQuery roleQuery = new RoleQuery();
            List<RolePM> result = roleQuery.GetRolesByUser(userId, tenant);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        public HttpResponseMessage GetUsersConnectedToRole(string roleId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ContactTenantRoleRepository contactTenantRoleRepository = new ContactTenantRoleRepository(authToken.Tenant);
                ContactTenantRepository contactTenantRepository = new ContactTenantRepository(authToken.Tenant);
                IQueryable<ContactTenantRole> contactTenantRoles = contactTenantRoleRepository.GetContactTenantRolesByRoleId(roleId, authToken.Tenant);

                List<UserList> roleUsers = new List<UserList>(); 
                foreach (ContactTenantRole contactTenantRole in contactTenantRoles)
                {
                    ContactTenant contactTenant = contactTenantRepository.GetSingleContactTenant(contactTenantRole.ContactTenantId, authToken.Tenant);
                    if (contactTenant != null) {
                        roleUsers.Add(new UserList()
                        {
                            Id = contactTenant.ContactId,
                            EnglishName = contactTenant.Contact ==  null ? null : contactTenant.Contact.EnglishName,
                            Email = contactTenant.Contact == null ? null : contactTenant.Contact.Email,
                        });
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, roleUsers);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}