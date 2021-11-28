using Logitude.BL.GlobalModel.EntityDws;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TenantManagementDWWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TenantManagementDWWcfService.svc or TenantManagementDWWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TenantManagementDWWcfService : ITenantManagementDWWcfService
    {
        public List<TenantManagementDW> GetTenantManagements(int tenant, int skip, int take, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                if (CrmWebServicesValidator.IsDisabled(tenant)) return new List<TenantManagementDW>();

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("TenantManagement", "READ", tenant);
                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
                List<TenantManagementDW> result = tenantManagementQuery.GetTenantManagementDWs(tenant, skip, take);
                return result;
            }

            catch (Exception ex)
            {
                response = new Response();
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

        }
    }
}
