using CHAMP;
using Logitude.CRM.BL.EntityDws;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityLists;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OpportunityDWWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OpportunityDWWcfService.svc or OpportunityDWWcfService.svc.cs at the Solution Explorer and start debugging.
       [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OpportunityDWWcfService : IOpportunityDWWcfService
    {
        public List<OpportunityDW> GetOpportunitiesByDates(int tenant, DateTime fromDate, DateTime toDate, int skip, int take, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);
                ICRMContext objectContext = CRMContext.GetContext(tenant);

                OpportunityQueryService opportunityQueryService = new OpportunityQueryService(objectContext);
                return opportunityQueryService.GetOpportunitiesDWByDates(tenant, fromDate, toDate, skip, take);


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

        public int GetOpportunitiesCountByDates(int tenant, DateTime fromDate, DateTime toDate, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);
                ICRMContext objectContext = CRMContext.GetContext(tenant);

                OpportunityQueryService opportunityQueryService = new OpportunityQueryService(objectContext);
                return opportunityQueryService.GetOpportunitiesCountDWByDates(tenant, fromDate, toDate);


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

                return 0;

            }

        }

        public List<OpportunityDW> GetOpportunitiesByUpdateDate(int tenant, DateTime updateDate, int skip, int take, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);//UPDATE//READ
                ICRMContext objectContext = CRMContext.GetContext(tenant);

                OpportunityQueryService opportunityQueryService = new OpportunityQueryService(objectContext);
                return opportunityQueryService.GetOpportunitiesDWByUpdateDate(tenant, updateDate, skip, take);


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

        public int GetOpportunitiesCountByUpdateDate(int tenant, DateTime updateDate, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);
                ICRMContext objectContext = CRMContext.GetContext(tenant);

                OpportunityQueryService opportunityQueryService = new OpportunityQueryService(objectContext);
                return opportunityQueryService.GetOpportunitiesDWCountByUpdateDate(tenant, updateDate);


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

                return 0;

            }

        }

    }
}
