using CHAMP;
using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.BL.CommonDataModel.EntityQueries;
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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CustomerDWWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CustomerDWWcfService.svc or CustomerDWWcfService.svc.cs at the Solution Explorer and start debugging.
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CustomerDWWcfService : ICustomerDWWcfService
    {

        public List<CustomerDW> GetCustomers(int tenant, int skip, int take, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);
                //CustomerQuery customerQuery = new CustomerQuery(tenant);
                //return customerQuery.GetCustomersDWLists(tenant, skip, take);
                return new List<CustomerDW>();


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

        public int GetCustomersCount(int tenant, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);
                //CustomerQuery customerQuery = new CustomerQuery(tenant);
                //return customerQuery.GetCustomersDWCount(tenant);
                return 0;


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

        public List<CustomerDW> GetCustomersByUpdateDate(int tenant, DateTime updateDate, int skip, int take, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);//UPDATE//READ
                //CustomerQuery customerQuery = new CustomerQuery(tenant);
                //return customerQuery.GetCustomersDWByListsUpdateDate(tenant, updateDate, skip, take);

                return new List<CustomerDW>();

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


        public int GetCustomersCountByUpdateDate(int tenant, DateTime updateDate, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);
                //CustomerQuery customerQuery = new CustomerQuery(tenant);
                //return customerQuery.GetCustomersDWCountByUpdateDate(tenant, updateDate);
                return 0;


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
