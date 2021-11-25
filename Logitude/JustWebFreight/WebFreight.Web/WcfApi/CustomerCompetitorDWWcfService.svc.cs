using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.Helpers;
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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CustomerCompetitorDWWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CustomerCompetitorDWWcfService.svc or CustomerCompetitorDWWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CustomerCompetitorDWWcfService : ICustomerCompetitorDWWcfService
    {
        public List<CustomerCompetitorDW> GetCustomerCompetitors(int tenant, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                if (tenant == 0 || tenant == 341) return new List<CustomerCompetitorDW>();

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("CustomerCompetitor", "READ", tenant);
                CustomerCompetitorQuery customerCompetitorQuery = new CustomerCompetitorQuery(tenant);
                return customerCompetitorQuery.GetCustomerCompetitorsDW(tenant);
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
