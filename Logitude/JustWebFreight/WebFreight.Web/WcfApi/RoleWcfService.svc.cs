using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using WebFreight.Web.Security;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RoleWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RoleWcfService.svc or RoleWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RoleWcfService : IRoleWcfService
    {
        public List<RoleList> GetRoles(int tenant, ref Response response)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            List<RoleList> result = new List<RoleList>();
            try
            {
                RoleQuery query = new RoleQuery();

                result = query.GetRoleListsByTenant(tenant).ToList();
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
            return result;
        }
    }
}
