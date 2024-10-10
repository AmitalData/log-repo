using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Server.Infrastructure.Helpers;
using System.Threading;
using Simplog.Data.CommonDataModel;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.SystemLogs;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.DataContracts;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Resolvers;
using WebFreight.Web.Helpers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Configuration;

namespace WebFreight.Web.Security
{
    public class SecurityUtility
    {
        public static void AuthenticateAPICall(int tenant)
        {
            if (LogitudeSettings.WorkEnvironment != "logbox" && LogitudeSettings.WorkEnvironment != "cloud")
            {
                bool exist = CheckUserTableFeature("General", "EXTERNALAPIS", tenant, true);
                if (!exist)
                {
                    throw new AutenticationException("API is not activated. Please contact your system administrator");
                }
            }
        }

        public static void AuthenticateAccessibleAPI(string apiName, int tenant)
        {
            ExternalAPITemplatesBuilder externalAPIHelper = new ExternalAPITemplatesBuilder(tenant);
            ExternalAPIResponseParameters responseParameters = externalAPIHelper.GetExternalAPIResponseParameters();

            if (!responseParameters.APIsNames.Contains(apiName))
            {
                throw new AutenticationException("You have no permission to use this API. Please contact your system administrator");
            }
        }

        private static bool CheckUserTableFeature(string objectTableName, string featureCode, int tenant, bool forceAPIFeaturesCheck)
        {
            bool exists = false;

            if (objectTableName.Contains("Customs."))
            {

            }
            //if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            //{

            //}

            string email = null;
            if (HttpContext.Current != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                /*string */
                email = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                email = AuthenticationUtil.ResolveLoggingUserId(tenant);
            }
            ContactInfo contactinfo = GetContactInfo(email, tenant, forceAPIFeaturesCheck);

            if (contactinfo != null)
            {
                if (contactinfo.IsLogitudeAdmin)
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
                            Dictionary<string, FeaturePM> features = GetFeaturesForRole(myRoleId, contactinfo.PackagesCodes, tenant, true);
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
            //}

            if (!exists)
            {
                return false;
            }
            else return true;
        }

        public static void AuthenticationOnTenant(int tenant)
        {
            if (HttpContext.Current != null)
            {
                string email = HttpContext.Current.User.Identity.Name;

                if (HttpContext.Current.Items != null)
                {
                    CheckHttpContextCurrentItems();
                }

                ContactInfo contactinfo = GetContactInfo(email, tenant);
                if (contactinfo == null || string.IsNullOrEmpty(email))
                {
                    throw new AutenticationException("Sorry! this user is not authorized!");
                }

                if (contactinfo.IsApi && (contactinfo.Tenant != tenant) && contactinfo.Tenant != 0)
                {
                    throw new AutenticationException("Sorry! this user is not authorized!");
                }

                TenantManagmentPrivateLabelsPM privatelabel = null;
                var url = SecurityUtility.getLoggedDomain();
                if (!url.Contains("system.logitudeworld.com") && !url.Contains("system.logbox.co.il") && !url.Contains("cloud.amital.co.il"))
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        TenantManagmentPrivateLabelsQuery query = new TenantManagmentPrivateLabelsQuery(0);
                        privatelabel = query.GetSingleActivePMByUrl_Cache(url);
                        if (privatelabel != null)
                        {
                            IGlobalContext globalContext = GlobalContext.GetContext();
                            GlobalTenant myTenant = (from d in globalContext.GlobalTenants where d.Id == tenant select d).FirstOrDefault();
                            if (contactinfo.Tenant != 0)
                            {
                                if (myTenant == null || string.IsNullOrEmpty(myTenant.PrivateLabelId) || myTenant.PrivateLabelId != privatelabel.Id)
                                {
                                    throw new AutenticationException("Sorry! this user is not authorized!");
                                }
                            }

                        }
                        scope.Complete();
                    }
                }


                if (HttpContext.Current.Request != null)
                {
                    string mobileVersion = HttpContext.Current.Request.Headers["MobileVersion"];
                    string Platform = HttpContext.Current.Request.Headers["Platform"];
                    if (!string.IsNullOrEmpty(mobileVersion))
                    {
                        double version = 0;
                        if (double.TryParse(mobileVersion, out version))
                        {
                            string mobileVersionError = "";
                            if (Platform == "IOS")
                            {
                                if (version < LogitudeSettings.IOSSharedAppMinimumVersion)
                                {
                                    mobileVersionError = "Your application version is out-of-date. Please upgrade your application to the latest version";
                                }
                            }
                            else
                            {
                                if (version < LogitudeSettings.AndroidSharedAppMinimumVersion)
                                {
                                    mobileVersionError = "Your application version is out-of-date. Please upgrade your application to the latest version";
                                }

                            }

                            if (!string.IsNullOrEmpty(mobileVersionError))
                            {
                                if (HttpContext.Current.Response.Headers["MobileVersionError"] != null)
                                {
                                    HttpContext.Current.Response.Headers["MobileVersionError"] = mobileVersionError;
                                }
                                else HttpContext.Current.Response.Headers.Add("MobileVersionError", mobileVersionError);
                            }


                        }
                    }


                    bool isBlocking;

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalcontext = GlobalContext.GetContext();
                        isBlocking = (from a in globalcontext.GlobalDBs select a).FirstOrDefault().IsBlocking;

                        if (isBlocking)
                        {
                            if (HttpContext.Current.Response.Headers["MobileUpgrading"] != null)
                            {
                                HttpContext.Current.Response.Headers["MobileUpgrading"] = "Unifreight mobile is being updated, please try again later . Sorry for the inconvenience";
                            }

                            else HttpContext.Current.Response.Headers.Add("MobileUpgrading", "Unifreight mobile is being updated, please try again later . Sorry for the inconvenience");
                        }

                        scope.Complete();
                    }

                }

            }
        }

        [ThreadStatic]
        public static bool IsWorkerRoleCall = false;

        public static void CheckContactFeature(string objectTableName, string featureCode, int tenant, string overrideEmail = null)
        {
            if (IsWorkerRoleCall && HttpContext.Current == null) //for calling the excel export data from WR 
            {
                return;
            }

            bool exists = false;

            if (HttpContext.Current != null && string.IsNullOrWhiteSpace(overrideEmail))
            {
                overrideEmail = HttpContext.Current.User.Identity.Name;
            }

            if (!string.IsNullOrEmpty(overrideEmail))
            {
                string email = overrideEmail;//HttpContext.Current.User.Identity.Name;
                ContactInfo contactinfo = GetContactInfo(email, tenant);

                if (contactinfo != null)
                {
                    if (contactinfo.IsLogitudeAdmin || contactinfo.IsApi)
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

                //AzureLog.SaveLogsInStorage(errorMessage, "E", DateTime.Now, errorMessage, null, 0, HttpContext.Current.User.Identity.Name, HttpContext.Current.User.Identity.Name, ip);

                throw new SecurityException("Sorry! you have no permission to do this operation on " + objectTableName + ". Please contact your administrator.");
            }


        }

        public static bool CheckFeature(string objectTableName, string featureCode, int tenant)
        {
            bool exists = false;

            if (objectTableName.Contains("Customs."))
            {

            }
            //if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            //{

            //}

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
            ContactInfo contactinfo = GetContactInfo(email, tenant);

            if (contactinfo != null)
            {
                if (contactinfo.IsLogitudeAdmin)
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
            //}

            if (!exists)
            {
                return false;
            }
            else return true;


        }


        public static bool CheckTableContactFeature(string objectTableName, string featureCode, int tenant)
        {
            if (IsWorkerRoleCall && HttpContext.Current == null) //for calling the excel export data from WR 
            {
                return true;
            }

            bool exists = false;

            if (tenant == 0)
            {
                exists = true;
            }

            else if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                string email = HttpContext.Current.User.Identity.Name;

                ContactInfo myContactInfo = GetContactInfo(email, tenant);

                if (myContactInfo != null)
                {
                    if (myContactInfo.IsLogitudeAdmin)
                    {
                        exists = true;
                    }

                    else
                    {
                        ObjectTablePM objectTable = ObjectTableQuery.GetObjectTableByCode(objectTableName, tenant);

                        foreach (string roleid in myContactInfo.RolesIds)
                        {
                            Dictionary<string, FeaturePM> features = GetFeaturesForRole(roleid, myContactInfo.PackagesCodes, tenant);
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

            return exists;
        }

        public static bool CheckFeatureAccessLevelPermission(string objectTableName, string featureCode, string entityUserId, string entityBusinessUnitId, int tenant)
        {
            bool isAllowed = false;

            if (tenant == 0)
            {
                isAllowed = true;
            }

            else if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                string email = HttpContext.Current.User.Identity.Name;

                ContactInfo myContactInfo = GetContactInfo(email, tenant);

                if (myContactInfo != null)
                {
                    if (myContactInfo.IsLogitudeAdmin)// || myContactInfo.IsApi)
                    {
                        isAllowed = true;
                    }

                    else
                    {
                        ObjectTablePM objectTable = ObjectTableQuery.GetObjectTableByCode(objectTableName, tenant);

                        FeatureRepository featureRepository = new FeatureRepository(tenant);
                        Feature myFeature = featureRepository.GetSingleFeatureByCode(objectTable.Id, featureCode, tenant);

                        if (myFeature != null)
                        {
                            RoleFeatureRepository roleFeatureRepository = new RoleFeatureRepository(tenant);

                            if (myFeature.IsBusinessUnitEnabled)
                            {
                                List<RoleFeature> myFeatureRoles = new List<RoleFeature>();

                                foreach (string myRoleId in myContactInfo.RolesIds)
                                {
                                    RoleFeature myRoleFeature = roleFeatureRepository.GetBusinessUnitFilterRoleFeature(myRoleId, myFeature.Id, tenant);
                                    if (myRoleFeature != null)
                                    {
                                        myFeatureRoles.Add(myRoleFeature);
                                    }
                                }

                                if (myFeatureRoles.Count > 0)
                                {
                                    if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                                    {
                                        isAllowed = true;
                                    }

                                    else
                                    {
                                        ContactRepository contactRepository = new ContactRepository(tenant);
                                        Contact contact = contactRepository.GetSingleContactByEmail(email, tenant);
                                        UserRepository userRepository = new UserRepository(tenant);
                                        User logedUser = userRepository.GetSingleUser(contact.Id, tenant, false);

                                        if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                                        {
                                            if (logedUser.Id == entityUserId)
                                            {
                                                isAllowed = true;
                                            }
                                        }

                                        else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "BU").Any())
                                        {
                                            if (logedUser.BusinessUnitId == entityBusinessUnitId)
                                            {
                                                isAllowed = true;
                                            }
                                        }

                                        else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                                        {
                                            if (entityBusinessUnitId.StartsWith(logedUser.BusinessUnitId))
                                            {
                                                isAllowed = true;
                                            }
                                        }
                                    }
                                }
                            }

                            else
                            {
                                foreach (string myRoleId in myContactInfo.RolesIds)
                                {
                                    RoleFeature myRoleFeature = roleFeatureRepository.GetBusinessUnitFilterRoleFeature(myRoleId, myFeature.Id, myFeature.Tenant);
                                    if (myRoleFeature != null)
                                    {
                                        isAllowed = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (!isAllowed)
                {
                    throw new Exception("Sorry! you have no permission to do this operation on " + objectTableName + ". Please contact your administrator.");
                }
            }

            return isAllowed;
        }

        public static ContactInfo GetContactInfo(string email, int tenant, bool forceAPIFeaturesCheck = false)
        {
            int loggedTenant = tenant;

            ContactInfo myContactInfo = null;
            string cacheKey = $"ContactInfo_{email}_{tenant}";
            myContactInfo = (ContactInfo)CacheManager.CacheWrapper.Get(cacheKey);

            if (myContactInfo == null || forceAPIFeaturesCheck)
            {
                if (tenant == 0)
                {
                    List<string> allPackages = GetAllPackagesCodes(email, loggedTenant, false);

                    myContactInfo = new ContactInfo()
                    {
                        ContactEmail = email,
                        IsLogitudeAdmin = true,
                        PackagesCodes = allPackages,
                    };

                    CacheManager.CacheWrapper.Insert(cacheKey, myContactInfo, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
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

                            myContactInfo = new ContactInfo()
                            {
                                Tenant = contact.Tenant,
                                ContactEmail = contact.Email,
                                IsLogitudeAdmin = isLogitudeAdmin,
                                RolesIds = allRolesIds,
                                PackagesCodes = allPackages,
                            };

                            CacheManager.CacheWrapper.Insert(cacheKey, myContactInfo, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                    else
                    {
                        if (authToken != null)
                        {
                            myContactInfo = new ContactInfo()
                            {
                                Tenant = authToken.Tenant,
                                ContactEmail = authToken.Email,
                                IsApi = authToken.APIToken,

                            };

                            CacheManager.CacheWrapper.Insert(cacheKey, myContactInfo, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                }
            }
            return myContactInfo;
        }

        public static void AuthenticationOnEntityTenant(string objectTableName, int entityTenant, int authTokenTenant)
        {
            //if (HttpContext.Current != null && string.IsNullOrWhiteSpace(overrideEmail))
            //{
            //	overrideEmail = HttpContext.Current.User.Identity.Name;
            //}

            if (entityTenant != authTokenTenant)
                throw new Exception("Sorry! you have no permission to do this operation on Tenant:" + entityTenant + ". Please contact your administrator.");
            //string errorMessage = "Sorry! you have no permission to do this operation" + Environment.NewLine + "Table:" + objectTableName + Environment.NewLine + "User:" + overrideEmail + Environment.NewLine + "Tenant:" + entityTenant;

        }
        //private static string GetComputingPartnerCode(AuthenticationToken authToken)
        //{
        //    string computingPartnerCode = "";
        //    if (authToken != null && !string.IsNullOrEmpty(authToken.APICredentialID))
        //    {
        //            ApiCredintialsRepository apiCredintialsRepository = new ApiCredintialsRepository();
        //            ApiCredintials apiCredintials = apiCredintialsRepository.GetSingleApiCredintials(authToken.APICredentialID, authToken.Tenant);
        //            if (apiCredintials != null && !string.IsNullOrEmpty(apiCredintials.ComputingPartnerId))
        //            {
        //                ComputingPartnerRepository computingPartnerRepository = new ComputingPartnerRepository(authToken.Tenant);
        //                computingPartnerCode = computingPartnerRepository.GetSingleComputingPartnerCodeById(apiCredintials.ComputingPartnerId);
        //            }

        //    }
        //    return computingPartnerCode;
        //}

        private static List<string> GetAllPackagesCodes(string email, int tenant, bool isCustomerCare)
        {
            string loggedUserId = GetLoggedUserId(email, tenant);

            PackagesCodesManager iManager = new PackagesCodesManager(tenant, loggedUserId, isCustomerCare);
            return iManager.BasePackagesCodes;
        }

        public static string GetLoggedUserId(string email, int tenant)
        {
            string loggedUserId = null;

            UserRepository userRep = new UserRepository(tenant);
            User user = userRep.GetSingleUserByEmail(email, tenant, true);
            if (user != null)
            {
                loggedUserId = user.Id;
            }

            return loggedUserId;
        }


        private static List<string> GetAllPackagesCodes_Old(string email, int tenant, bool isCustomerCare)
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

                        //if (!this.IsUserAdditionalPackagesOnly && !allPackagesCodes_PK.Contains(myTenantManagement.PackageCode))
                        //{
                        //    allPackagesCodes_PK.Add(myTenantManagement.PackageCode);
                        //}
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
        private static Dictionary<string, FeaturePM> GetFeaturesForRoleNotCached(string roleId, List<string> allowedPackages, int tenant)
        {
            Dictionary<string, FeaturePM> features = null;


            FeatureQuery featuresQuery = new FeatureQuery(tenant);
            List<FeaturePM> fet = featuresQuery.GetAllowedFeaturesForRole(roleId, allowedPackages, tenant);
            features = fet.ToDictionary(d => d.Code + d.ObjectTableId, d => d);
            return features;
        }
        private static bool CheckIfFeatureHasAccess(string roleId, List<string> allowedPackages, int tenant, string FeatureCode, string ObjectTableId)
        {
            FeatureQuery featuresQuery = new FeatureQuery(tenant);
            bool fet = featuresQuery.CheckIfFeatureshasAccess(roleId, allowedPackages, tenant, FeatureCode, ObjectTableId);

            return fet;
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

        public static bool CheckDigitalUserAuthentication(int tenant, string partnerId)
        {
            if (tenant == 0) return true;
            if (string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name)) throw new AutenticationException("Sorry! you are not authorized to read data!");

            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            string email = HttpContext.Current.User.Identity.Name;

            ContactRepository contactrep = new ContactRepository(commonDataContext);
            Contact customerCareContact = contactrep.GetSingleContactByEmail(email, 0);
            if (customerCareContact != null)
            {
                return true;
            }

            Contact contact = contactrep.GetSingleContactByEmail(email, tenant);
            if (contact != null)
            {
                List<string> partners = partnerId?.Split(',').ToList<string>();
                CardContact cardContact = commonDataContext.CardContacts.Where(d => d.ContactId == contact.Id && partners.Contains(d.CardId)).FirstOrDefault();

                if (cardContact != null)
                {
                    return true;
                }
            }

            UserRepository userRepository = new UserRepository(commonDataContext);
            User logedUser = userRepository.GetSingleUserByEmail(email, tenant, true);
            if (logedUser != null)
            {
                return true;
            }

            throw new AutenticationException("Sorry! you are not authorized to read data!");
        }

        public static string GetAuthenticatedUser()
        {
            if (IsWorkerRoleCall && !string.IsNullOrEmpty(AuthenticationUtil.AuthenticatedUserEmail)) //for calling the excel export data from WR 
            {
                return AuthenticationUtil.AuthenticatedUserEmail;
            }

            if (IsWorkerRoleCall && HttpContext.Current == null) //for calling the excel export data from WR 
            {
                var loggedContact = LoggedContactResolver.GetLoggedContact(0);
                return loggedContact?.Email;
            }

            if (HttpContext.Current != null)
            {

                if (HttpContext.Current.Items != null)
                {
                    CheckHttpContextCurrentItems();
                }


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

        private static void CheckHttpContextCurrentItems()
        {
            CheckSessionExpiration();
            CheckAPICredintialExpiration();
        }

        private static void CheckSessionExpiration()
        {
            if (!HttpContext.Current.Items.Contains("Session")) return;
            string sessionItem = HttpContext.Current.Items["Session"] as string;
            if (sessionItem == "SessionExpiration")
            {
                throw new Exception("Sorry! this user is not authorized! due to session expiration");
            }
        }

        private static void CheckAPICredintialExpiration()
        {
            if (!HttpContext.Current.Items.Contains("APICredintial")) return;
            string aPICredintialItem = HttpContext.Current.Items["APICredintial"] as string;
            if (aPICredintialItem == "APICredintialExpired")
            {
                throw new Exception("Sorry! this user is not authorized! due to api credintial expiration");
            }
        }

        public static void RedirectToHttps(bool IsEndResponse = true)
        {
            bool redirect = false;

            if (!SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Logbox) && !SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Development))
            {
                if (ContinueRedirectToHttps())
                {

                    redirect = true;


                    //string ip = HttpContext.Current.Request.UserHostAddress;
                    //if (!string.IsNullOrEmpty(ip))
                    //{
                    //    string[] IpParts = ip.Split('.');
                    //    if (IpParts.Length > 3)
                    //    {
                    //        if (!string.IsNullOrEmpty(IpParts[3]))
                    //        {
                    //            int lastPart = int.Parse(IpParts[3]);
                    //            if (lastPart % 10 == 0) redirect = true;
                    //        }
                    //    }

                }
            }


            HttpContext context = HttpContext.Current;
            string IsSecureConnection = context.Request.IsSecureConnection.ToString();
            if (context.Request.Headers.AllKeys.Contains("X-IsSecure"))
            {
                IsSecureConnection = context.Request.Headers["X-IsSecure"];
            }
            if (LogitudeSettings.ForceHttps || redirect)
            {
                if (IsSecureConnection != "true")
                {

                    string redirectUrl = context.Request.Url.ToString().Replace("http:", "https:").Replace(":81", "");
                    if (IsEndResponse)
                    {
                        context.Response.Redirect(redirectUrl);
                    }
                    else
                    {
                        if (!context.Request.Url.ToString().Contains("https"))
                        {
                            context.Response.Redirect(redirectUrl, false);
                        }

                    }
                }
            }

        }

        public static string getLoggedDomain()
        {
                HttpContext context = HttpContext.Current;
                string Url = context.Request.Url.ToString().Split('/')[2];//("http://", "");
                Url = Url.Split(':')[0];

                var isAppServiceENV = Environment.GetEnvironmentVariable("IsAppService") == "true";
                bool isAppService = ConfigurationManager.AppSettings["IsAppService"] == "true";

                if (isAppServiceENV || isAppService)
                {
                     if (!string.IsNullOrEmpty(context.Request.Headers["X-ORIGINAL-HOST"]))
                          Url = context.Request.Headers["X-ORIGINAL-HOST"];

                }

                return Url;
        }

        private static bool ContinueRedirectToHttps()
        {
            try
            {
                //var domain = Environment.UserDomainName ?? "";
                //if (domain.ToLower().Contains("ntdomain"))
                {
                    var machineName = Environment.MachineName ?? "";
                    machineName = machineName.ToLower();
                    if (machineName.Contains("alex") || machineName.Contains("itzik"))
                    {
                        return false;
                    }


                }
                return true;
            }

            catch (Exception)
            {

                return true;
            }
        }

        public static void CheckContactTableFeatures(List<FeatureAccessInfo> featuresList, string email, int tenant)
        {

            if (!string.IsNullOrEmpty(email))
            {
                List<string> allowedPackages = new List<string>();
                ContactInfo myContactInfo = GetContactInfo(email, tenant, true);
                if (myContactInfo != null)
                {
                    allowedPackages = myContactInfo.PackagesCodes;

                    if (myContactInfo.IsLogitudeAdmin)
                    {
                        foreach (FeatureAccessInfo inf in featuresList)
                        {
                            inf.HasAccess = true;
                        }
                    }

                    else
                    {
                        foreach (FeatureAccessInfo inf in featuresList)
                        {
                            ObjectTablePM objectTable = ObjectTableQuery.GetObjectTableByCode(inf.ObjectTableName, tenant);

                            foreach (string roleid in myContactInfo.RolesIds)
                            {
                                bool features = CheckIfFeatureHasAccess(roleid, allowedPackages, tenant, inf.FeatureCode, objectTable.Id);
                                if (features)
                                {
                                    inf.HasAccess = true;
                                    break;
                                }
                                else
                                {
                                    inf.HasAccess = false;
                                }
                            }
                        }
                    }
                }
            }

            //return featuresList;
        }

        public static void CheckCustomContactTableFeatures(List<FeatureAccessInfo> featuresList, string email, int tenant)
        {

            if (!string.IsNullOrEmpty(email))
            {
                foreach (FeatureAccessInfo inf in featuresList)
                {
                    inf.HasAccess = CheckOutlookContactFeature(inf.ObjectTableName, inf.FeatureCode, tenant, email);
                }
            }
        }
        private static bool CheckOutlookContactFeature(string objectTableName, string featureCode, int tenant, string email)
        {
            bool exists = false;
            if (!string.IsNullOrEmpty(email))
            {
                //string email = HttpContext.Current.User.Identity.Name;
                ContactInfo contactinfo = GetContactInfo(email, tenant);

                if (contactinfo != null)
                {
                    if (contactinfo.IsLogitudeAdmin)
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
            return exists;

        }


        public static bool CheckFeaturesForHelpCenter(string objectTableName, string featureCode, int tenant)
        {
            bool exists = false;

            if (objectTableName.Contains("Customs."))
            {

            }

            if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                string email = HttpContext.Current.User.Identity.Name;
                ContactInfo contactinfo = GetContactInfo(email, tenant);

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
            }

            return exists;
        }

        public static bool CheckIsUserCustomerCare(string email)
        {
            bool isCustomerCare = false;
            IGlobalContext globalObjectContext = GlobalContext.GetContext();
            GlobalContact zeroContact = globalObjectContext.GlobalContacts.Where(d => d.GlobalTenantId == 0 && d.Email == email && d.InActive == false).FirstOrDefault();
            if (zeroContact != null)
            {
                ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
                var logitudeUser = (from a in commonDataContext.Users
                                    where a.Id == zeroContact.Id
                                    select a).FirstOrDefault();
                if (logitudeUser != null)
                {
                    if (logitudeUser.Tenant == 0) isCustomerCare = !logitudeUser.IsDistributor;
                }
            }

            return isCustomerCare;
        }

        public static bool IsUser(string email, int tenant)
        {

            IGlobalContext globalObjectContext = GlobalContext.GetContext();
            GlobalContact contact = globalObjectContext.GlobalContacts.Where(m => m.Email == email && m.GlobalTenant.IsActive == true && m.InActive == false && (m.IsUser == true || m.InternetAccess == true) 
            && m.GlobalTenant.Id == tenant).FirstOrDefault();
            
            if(contact != null)
                return contact.IsUser;

            return false;

        }
        public static bool isUserAdmin(string email, int tenant)
        {
            UserRepository userRepository = new UserRepository(tenant);
            User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, email, tenant, false);
            if(loggedUser!=null && loggedUser.UserRoles!=null) {
                if (loggedUser.UserRoles.Contains("Administrator"))
                {
                    return true;
                }
            }
           
            return false;

        }

        public static void AuthenticateDashboardReadFeatures(string objectTableName, string featureCode, int tenant)
        {
            bool exists = false;

            if (HttpContext.Current != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                string email = HttpContext.Current.User.Identity.Name;
                ContactInfo contactinfo = GetContactInfo(email, tenant);

                if (contactinfo != null)
                {
                    if (contactinfo.IsLogitudeAdmin || contactinfo.IsApi)
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
                                Dictionary<string, FeaturePM> features = GetFeaturesForRole(myRoleId, contactinfo.PackagesCodes, tenant, true);
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
                throw new SecurityException("Sorry! you have no permission to do this operation on " + objectTableName + ". Please contact your administrator.");
            }
        }
    }
}
