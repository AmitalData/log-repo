
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.BL.Validators;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
namespace Logitude.BL.Security
{
    public class SecurityUtility
    {
        public static bool IsWorkerRoleCall = false;
        public static string GetAuthenticatedUser()
        {
            if (IsWorkerRoleCall && HttpContext.Current == null) //for calling the excel export data from WR 
            {
                var loggedContact = LoggedContactResolver.GetLoggedContact(0);
                return loggedContact?.Email;
            }

            if (HttpContext.Current != null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    return HttpContext.Current.User.Identity.Name;
                }

                else
                {
                    throw new AutenticationException("Sorry! this user is not authorized!");
                }
            }

            throw new AutenticationException("Sorry! this user is not authorized!");
        }

       static   ContactInformation contactinfo;
        public static void CheckContactFeature(string objectTableName, string featureCode, int tenant, string overrideEmail = null)
        {
            if (IsWorkerRoleCall && HttpContext.Current == null) //for calling the excel export data from WR 
            {
                return;
            }

            bool exists = false;

            if (objectTableName.Contains("Customs."))
            {

            }
            if (HttpContext.Current != null && string.IsNullOrWhiteSpace(overrideEmail))
            {
                overrideEmail = HttpContext.Current.User.Identity.Name;
            }
            if (!string.IsNullOrEmpty(overrideEmail))
            {
                string email = overrideEmail;//HttpContext.Current.User.Identity.Name;
                 contactinfo = GetContactInfo(email, tenant);

                if (contactinfo != null)
                {
                    if (contactinfo.IsApi)
                    {
                        exists = true;
                    }

                    else
                    {
                        ObjectTablePM objectTable = ObjectTableQuery.GetObjectTableByCode(objectTableName, tenant);
                        if (objectTable != null)
                        {
                            foreach (string myRoleId in contactinfo.RolesIds)
                            {
                                Dictionary<string, FeaturePM> features = GetFeaturesForRole(myRoleId, contactinfo.PackagesCodes, tenant);
                                if (features.Keys.Contains(featureCode + objectTable.Id))
                                {
                                    FeaturePM feature = features[featureCode + objectTable.Id];
                                    if (feature != null)
                                    {
                                        exists = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!exists)
            {
                string errorMessage = "Sorry! you have no permission to do this operation" + Environment.NewLine + "Table:" + objectTableName + Environment.NewLine + "User:" + overrideEmail + Environment.NewLine + "Tenant:" + tenant;

                string ip = "";

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                bool showLocal = contactinfo != null ? (!contactinfo.DontShowLocalLabels) : false;
                string objectTableLocalName = TextCodesTranslator.TranslateText(objectTableName, tenant, showLocal);
                //AzureLog.SaveLogsInStorage(errorMessage, "E", DateTime.Now, errorMessage, null, 0, HttpContext.Current.User.Identity.Name, HttpContext.Current.User.Identity.Name, ip);
                string error = TextCodesTranslator.TranslateText("Accounting.General.O.YouDontHavePermission", tenant, showLocal);
                throw new Exception(error + " " + objectTableLocalName + ". Please contact your administrator.");
            }


        }

        private static Dictionary<string, FeaturePM> GetFeaturesForRole(string roleId, List<string> allowedPackages, int tenant, bool forceAPIFeaturesCheck = false)
        {
            Dictionary<string, FeaturePM> features = null;
            string roleKey = roleId + "_" + tenant;

            if (CacheManager.CacheWrapper.Get(roleKey) == null || forceAPIFeaturesCheck)
            {
                FeatureQuery featuresQuery = new FeatureQuery(tenant);
                List<FeaturePM> fet = featuresQuery.GetAllowedFeaturesForRole(roleId, allowedPackages, tenant);
                features = fet.ToDictionary(d => d.Code + d.ObjectTableId, d => d);
                CacheManager.CacheWrapper.Insert(roleKey, features, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            }

            else
            {
                features = (Dictionary<string, FeaturePM>)CacheManager.CacheWrapper.Get(roleKey);
            }

            return features;
        }


        public static bool CheckFeature(string objectTableName, string featureCode, int tenant)
        {
            bool exists = false;

   

            string email = null;
            if (HttpContext.Current != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                /*string */
                email = HttpContext.Current.User.Identity.Name;
            }
            else if (AuthenticationUtil.AuthenticatedUserEmail != null)
            {
                email = AuthenticationUtil.AuthenticatedUserEmail;
            }
            else if (AuthenticationUtil.AuthenticatedUserEmail != null)
            {
                email = AuthenticationUtil.AuthenticatedUserEmail;
            }
            else
            {
                email = AuthenticationUtil.ResolveLoggingUserId(tenant);
            }
              contactinfo = GetContactInfo(email, tenant);

            if (contactinfo != null)
            {
                
                    ObjectTablePM objectTable = ObjectTableQuery.GetObjectTableByCode(objectTableName, tenant);
                    if (objectTable != null)
                    {
                        foreach (string myRoleId in contactinfo.RolesIds)
                        {
                            Dictionary<string, FeaturePM> features = GetFeaturesForRole(myRoleId, contactinfo.PackagesCodes, tenant);
                            if (features.Keys.Contains(featureCode + objectTable.Id))
                            {
                                FeaturePM feature = features[featureCode + objectTable.Id];
                                if (feature != null)
                                {
                                    exists = true;
                                }
                            }
                        }
                    }
                
            }
            //}

            if (!exists)
            {
                return false;
            }
            else return true;


        }

        public static ContactInformation GetContactInfo(string email, int tenant, bool forceAPIFeaturesCheck = false)
        {
            int loggedTenant = tenant;

            ContactInformation myContactInfo = null;
           
            string key = email + "_" + tenant + "_info";
          

            //if (CacheManager.CacheWrapper.Get(key) != null && !forceAPIFeaturesCheck)
            //{
            //    myContactInfo = (ContactInformation)CacheManager.CacheWrapper.Get(key);
            //}
            //else
            //{

                if (tenant == 0)
                {
                    List<string> allPackages = GetAllPackagesCodes(email, loggedTenant, false);

                    myContactInfo = new ContactInformation()
                    {
                        ContactEmail = email,
                        IsLogitudeAdmin = true,
                        PackagesCodes = allPackages,
                    };

                    //CacheManager.CacheWrapper.Insert(key, myContactInfo, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }

                else
                {
                    string token = null;
                    ICommonDataContext context = CommonDataContext.GetContext(0);
                    AuthenticationTokenRepository tokenRep = new AuthenticationTokenRepository(context);
                    AuthenticationToken authToken = null;
                    if (HttpContext.Current != null)
                    {
                        token = HttpContext.Current.Request.Headers["Token"];
                    }
                    if (!string.IsNullOrEmpty(token))
                    {
                        authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    }

                    if (authToken == null || !authToken.APIToken || forceAPIFeaturesCheck)
                    {
                        ContactRepository contactrep = new ContactRepository(tenant);
                        Contact contact = contactrep.GetSingleContactByEmail(email, tenant);

                        if (contact != null)
                        {
                            bool isCustomerCare = false;

                            if (contact.Tenant == 0 && tenant != 0)
                            {
                                UserRepository userRep = new UserRepository(0);
                                User zeroUser = userRep.GetSingleUserByEmail(email, 0, true);
                                if (zeroUser != null)
                                {
                                    isCustomerCare = true;

                                    if (zeroUser.IsDistributor)
                                    {
                                        using (TransactionScope scope2 = TransactionFactory.GetNewTransaction())
                                        {
                                            TenantManagementRepository tenantManagementRep = new TenantManagementRepository();
                                            bool isDistributorToCurrentTenant = tenantManagementRep.CheckDistributor(zeroUser.DistributorCode, tenant);
                                            if (!isDistributorToCurrentTenant)
                                            {
                                                return null;
                                            }

                                            scope2.Complete();
                                        }
                                    }
                                }
                            }

                            bool isLogitudeAdmin = false;
                            if (tenant != 0)
                            {
                                UserRepository userRep = new UserRepository(LogitudeSettings.LogitudeCRMTenantNumber);
                                User user = userRep.GetSingleUserByEmail(email, LogitudeSettings.LogitudeCRMTenantNumber, true);
                                if (user != null)
                                {
                                    tenant = LogitudeSettings.LogitudeCRMTenantNumber;
                                    isLogitudeAdmin = true;
                                }
                            }

                            RoleQuery roleQuery = new RoleQuery(tenant);
                            List<RolePM> allRoles = roleQuery.GetRolesForContact(contact.Id, contact.Tenant).ToList();

                            List<string> allRolesIds = allRoles.Select(s => s.Id).ToList();
                            List<RolePM> allCustomRoles = allRoles.Where(d => d.IsCustomRole == true).ToList();

                            foreach (RolePM item in allCustomRoles)
                            {
                                if (allRolesIds.Contains(item.ParentRoleId))
                                {
                                    allRolesIds.Remove(item.ParentRoleId);
                                }
                            }

                            List<string> allPackages = GetAllPackagesCodes(email, loggedTenant, isCustomerCare);

                            myContactInfo = new ContactInformation()
                            {
                                Tenant = contact.Tenant,
                                ContactEmail = contact.Email,
                                IsLogitudeAdmin = isLogitudeAdmin,
                                RolesIds = allRolesIds,
                                PackagesCodes = allPackages,
                                DontShowLocalLabels = contact.DontShowLocalLabels
                            };
                            //myContactInfo.ComputingPartnerCode = GetComputingPartnerCode(authToken);


                            //CacheManager.CacheWrapper.Insert(key, myContactInfo, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }

                    else
                    {
                        if (authToken != null)
                        {
                            myContactInfo = new ContactInformation()
                            {
                                Tenant = authToken.Tenant,
                                ContactEmail = authToken.Email,
                                IsApi = authToken.APIToken,

                            };

                            //myContactInfo.ComputingPartnerCode = GetComputingPartnerCode(authToken);


                            //CacheManager.CacheWrapper.Insert(key, myContactInfo, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }

                }

            //}
            return myContactInfo;
        }

        private static List<string> GetAllPackagesCodes(string email, int tenant, bool isCustomerCare)
        {
            List<string> myResult = new List<string>();

            string myPackageCode = null;
            bool isMultiPackage = false;
            bool isAddingAddOns = false;
            List<string> allPackagesCodes_AD = new List<string>();
            List<string> allPackagesCodes_PK = new List<string>();
            List<string> allPackagesCodes_BS = new List<string>();
            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                #region
                IGlobalContext globalContext = GlobalContext.GetContext();
                TenantManagement myTenantManagement = (from d in globalContext.TenantManagements where d.Id == tenant select d).FirstOrDefault();

                if (myTenantManagement != null)
                {
                    isMultiPackage = myTenantManagement.IsMultiPackage;

                    allPackagesCodes_AD = (from d in globalContext.TenantAddOns
                                           where d.Tenant == tenant
                                           group d by d.PackageCode into g
                                           select g.Key).ToList();

                    if (isMultiPackage)
                    {
                        allPackagesCodes_PK = (from d in globalContext.TenantManagementLicenses
                                               where d.Tenant == tenant
                                               group d by d.PackageCode into g
                                               select g.Key).ToList();
                    }

                    else
                    {
                        myPackageCode = myTenantManagement.PackageCode;

                        if (!string.IsNullOrEmpty(myTenantManagement.TemporalPackageCode) && myTenantManagement.TemporalStartDate != null && myTenantManagement.TemporalEndDate != null)
                        {
                            if (myTenantManagement.TemporalStartDate.Value.Date <= DateTime.Now.Date && DateTime.Now.Date <= myTenantManagement.TemporalEndDate.Value.Date)
                            {
                                myPackageCode = myTenantManagement.TemporalPackageCode;
                            }
                        }
                    }
                }

                scope.Complete();
                #endregion
            }

            if (isMultiPackage)
            {
                // Ayman Origin design
                // If multi then
                // if customer care then include addons
                // else if has at least 1 package Licensed then include addons

                if (isCustomerCare)
                {
                    isAddingAddOns = true;
                }

                else
                {
                    isAddingAddOns = true;

                    string loggedUserId = null;

                    if (loggedUserId == null)
                    {
                        UserRepository userRep = new UserRepository(tenant);
                        User user = userRep.GetSingleUserByEmail(email, tenant, true);
                        if (user != null)
                        {
                            loggedUserId = user.Id;
                        }
                    }

                    List<string> allUserLicenses = (from a in myCommonContext.UserLicenses
                                                    where a.Tenant == tenant && a.UserId == loggedUserId
                                                    group a by a.PackageCode into g
                                                    select g.Key).ToList();

                    allPackagesCodes_PK = (from a in allPackagesCodes_PK
                                           where allUserLicenses.Contains(a)
                                           select a).ToList();

                    //if (allUserLicenses.Count > 0)
                    //{
                    //    isAddingAddOns = true;
                    //}
                }

                List<string> allConnectedCodes_PK = (from a in myCommonContext.PackageConnectedPackages
                                                     where allPackagesCodes_PK.Contains(a.PackageCode)
                                                     group a by a.ConnectedPackageCode into g
                                                     select g.Key).ToList();

                foreach (string itemCode in allConnectedCodes_PK)
                {
                    if (!allPackagesCodes_BS.Contains(itemCode))
                    {
                        allPackagesCodes_BS.Add(itemCode);
                    }
                }
            }

            else
            {
                isAddingAddOns = true;

                Package myPackage = (from a in myCommonContext.Packages where a.Code == myPackageCode select a).FirstOrDefault();
                if (myPackage != null)
                {
                    if (myPackage.FeaturePackageTypeCode == "BS")
                    {
                        if (!allPackagesCodes_BS.Contains(myPackageCode))
                        {
                            allPackagesCodes_BS.Add(myPackageCode);
                        }
                    }

                    else
                    {
                        var allCodes = (from a in myCommonContext.PackageConnectedPackages
                                        where a.PackageCode == myPackageCode
                                        group a by a.ConnectedPackageCode into g
                                        select g.Key).ToList();

                        foreach (string itemCode in allCodes)
                        {
                            if (!allPackagesCodes_BS.Contains(itemCode))
                            {
                                allPackagesCodes_BS.Add(itemCode);
                            }
                        }
                    }
                }
            }

            if (isAddingAddOns)
            {
                if (allPackagesCodes_AD.Count > 0)
                {
                    List<string> allConnectedCodes_AD = (from a in myCommonContext.PackageConnectedPackages
                                                         where allPackagesCodes_AD.Contains(a.PackageCode)
                                                         group a by a.ConnectedPackageCode into g
                                                         select g.Key).ToList();

                    foreach (string itemCode in allConnectedCodes_AD)
                    {
                        if (!allPackagesCodes_BS.Contains(itemCode))
                        {
                            allPackagesCodes_BS.Add(itemCode);
                        }
                    }
                }
            }

            myResult = allPackagesCodes_BS;

            return myResult;
        }


        public static string  GetAuthenticatedUser(int tenant)
        {
            string email = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                email = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                email = "system@tenant" + tenant.ToString() + ".com";
            }

            return email;
        }

        public static string GetAuthenticatedWorkWebUser()
        {
            if (HttpContext.Current != null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    return HttpContext.Current.User.Identity.Name;
                }

                else
                {
                    throw new AutenticationException("Sorry! this user is not authorized!");
                }
            }

            else if (!string.IsNullOrEmpty(AuthenticationUtil.AuthenticatedUserEmail))
            {
                return AuthenticationUtil.AuthenticatedUserEmail;
            }

            throw new AutenticationException("Sorry! this user is not authorized!");
        }
 

        public static bool CheckSharedContactAuthentication(int tenant, string partnerId)
        {
            if (tenant != 0)
            {
                bool exists = false;
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                    string email = HttpContext.Current.User.Identity.Name;

                    ContactRepository contactrep = new ContactRepository(commonDataContext);
                    Contact contact = contactrep.GetSingleContactByEmail(email, tenant);

                    if (contact != null)
                    {
                        CardContact cardContact = commonDataContext.CardContacts.Where(d => d.ContactId == contact.Id && d.CardId == partnerId).FirstOrDefault();
                        if (cardContact != null)
                        {
                            //Card card = commonDataContext.Cards.Where(d => d.Id == partnerId).FirstOrDefault();
                            //if (card.PartnerTypeId == partnerTypeId)
                            //{
                            exists = true;
                            //}
                        }
                    }
                }
                if (!exists)
                {
                    throw new AutenticationException("Sorry! you are not authorized to read data!");
                }
                return exists;
            }
            return true;
        }


    }
}
