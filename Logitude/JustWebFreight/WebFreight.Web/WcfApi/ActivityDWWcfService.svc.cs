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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ActivityDWWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ActivityDWWcfService.svc or ActivityDWWcfService.svc.cs at the Solution Explorer and start debugging.



    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ActivityDWWcfService : IActivityDWWcfService
    {
        public List<ActivitiyDW> GetActivitiesByDates(int tenant, DateTime fromDate, DateTime toDate, int skip, int take, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                if (CrmWebServicesValidator.IsDisabled(tenant)) return new List<ActivitiyDW>();
                // Test
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Activity", "READ", tenant);
                ICRMContext objectContext = CRMContext.GetContext(tenant);

                ActivityQueryService activityQueryService = new ActivityQueryService(objectContext);
                return activityQueryService.GetctivitiesDWListsByDates(tenant, fromDate, toDate, skip, take);
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

        public int GetActivitiesCountByDates(int tenant, DateTime fromDate, DateTime toDate, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                if (CrmWebServicesValidator.IsDisabled(tenant)) return 0;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Activity", "READ", tenant);
                ICRMContext objectContext = CRMContext.GetContext(tenant);

                ActivityQueryService activityQueryService = new ActivityQueryService(objectContext);
                return activityQueryService.GetctivitiesDWListsCountByDates(tenant, fromDate, toDate);
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


        public List<ActivitiyDW> GetActivitiesByUpdateDate(int tenant, DateTime updateDate, int skip, int take, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                if (CrmWebServicesValidator.IsDisabled(tenant)) return new List<ActivitiyDW>();

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Activity", "READ", tenant);//UPDATE//READ
                ICRMContext objectContext = CRMContext.GetContext(tenant);

                ActivityQueryService activityQueryService = new ActivityQueryService(objectContext);
                return activityQueryService.GetActivitiesDWBListsByUpdateDate(tenant, updateDate, skip, take);
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

        public int GetActivitiesCountByUpdateDate(int tenant, DateTime updateDate, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                if (CrmWebServicesValidator.IsDisabled(tenant)) return 0;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Activity", "READ", tenant);
                ICRMContext objectContext = CRMContext.GetContext(tenant);

                ActivityQueryService activityQueryService = new ActivityQueryService(objectContext);
                return activityQueryService.GetctivitiesDWCountByUpdateDate(tenant, updateDate);
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