using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.Customs.Data.Utils
{
    public class FreelancerCustomersUtil
    {
        public User user;
        string keyCombination;

        public FreelancerCustomersUtil(int tenant)
        {
            user = GetLoggedUser(tenant);
            keyCombination = "FreelancerConnectedCustomers-User-" + user.Id;

        }

        /// <summary>
        /// Get Connected Customers For Freelancer user.
        /// </summary>
        /// <param name="tenant">Freelancer user tenant</param>
        /// <returns>Array of freelancer connected customer Ids</returns>
        public List<string> GetConnectedCustomersIds(int tenant)
        {
            if (!IsConnectedCustomerCached()) // if the cache is empty, cache the connected customers
                CacheConnectedCustomers(tenant);

            var customersIds = GetFromCache();

            return customersIds;
        }

        //
        // Private methods
        private bool IsConnectedCustomerCached() // is cache exist
        {
            List<Customer> customers = (List<Customer>)CacheManager.CacheWrapper.Get(keyCombination);

            return customers != null;
        }

        private List<string>  GetFromCache()
        {
            List<string> customersIds = new List<string>();

            List<Customer> customers = (List<Customer>)CacheManager.CacheWrapper.Get(keyCombination);

            if (customers != null)// dsv error log customers shouldn't be null and if it is null it must not crash.
            {
                foreach (Customer customer in customers)
                {
                    if (customer != null) customersIds.Add(customer.Id);
                }
            }

            return customersIds;
        }

        private void CacheConnectedCustomers(int tenant)
        {
            CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(user.Tenant);
            
            List<Customer> customersList = new List<Customer>();
            List<string> codesList = new List<string>();


            if (user.IsFreelancer)
            {

                /// 1- Get customers codes 
                //if (custSettings.IsConnectedToUniFreight)
                CustomsSetting custSettings = null;
                
                //if (LogitudeSettings.WorkEnvironment == "Customs") { custSettings = custSettingsRepo.GetSettingByTenant(user.Tenant); }
                if (LogitudeSettings.IsCostomsDeploy) {
                    custSettings = custSettingsRepo.GetSettingByTenant(user.Tenant);
                }

                if (custSettings != null && !string.IsNullOrWhiteSpace(custSettings.UnfConnectionString))
                {

                    // (Amital)
                    // Get users from unf service
                    try
                    {
                        AmitalRestrictOwnerModel restOwnerModel = custSettingsRepo.GetMyAmitalRestrictOwnerModel(false, tenant);
                        codesList = restOwnerModel.Cards;
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Error when getting freelancer customers by amital", ex);
                    }
                }
                else
                {
                    // (FanarSoft)
                    // Get users codes from User.Note
                    string notes = string.IsNullOrWhiteSpace(user.Notes) ? "" : user.Notes.Trim();
                    string[] codes = notes.Split(',');
                    codesList = codes.ToList();
                }


                /// 2- maintain users from the DB
                customersList.Clear();
                if (codesList.Count > 0)
                {
                    foreach (string code in codesList)
                    {
                        CustomerRepository custRepo = new CustomerRepository(user.Tenant);
                        Customer customer = custRepo.GetSingleCustomerByCode(code, user.Tenant, false);
                        if (customer != null) customersList.Add(customer);
                    }
                }

                /// 3- Store the customers in the cache
                CacheManager.CacheWrapper.Invalidate(keyCombination);
                CacheManager.CacheWrapper.Insert(keyCombination, customersList);

            }
        }

        private User GetLoggedUser(int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            UserRepository Repo = new UserRepository(commonContext);

            string email = "";
            email = (IsAuthenticatedUserExists() ? GetAuthenticatedUser() : ("system@tenant" + tenant + ".com"));
            User user = Repo.GetSingleUserByEmail(email, tenant, false);

            return user;
        }

        private string GetAuthenticatedUser()
        {
            if (HttpContext.Current != null)
            {

                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    return HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        if (authToken != null)
                            return authToken.Email;
                    }

                    throw new Exception("Sorry! this user is not authorized!");
                }
            }

            throw new Exception("Sorry! this user is not authorized!");
        }

        private bool IsAuthenticatedUserExists()
        {
            bool exists = false;
            if (HttpContext.Current != null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    exists = true;
                }
            }

            return exists;
        }

        public static int GetLoggedTenant()
        {
            if (HttpContext.Current != null)
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                if (!string.IsNullOrEmpty(token))
                {
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    if (authToken != null)
                        return authToken.Tenant;
                }

                throw new Exception("Sorry! this user is not authorized!");
            }

            throw new Exception("Sorry! this user is not authorized!");
        }
    }
}
