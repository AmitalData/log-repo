using AmitalCloud.Infrastructure.Data;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web;
using AmitalCloud.Infrastructure.Domain.Interfaces;

namespace AmitalCloud.Infrastructure.Data.Security
{
    public class AmitalCloudSecurityUtility
    {
        [ThreadStatic]
        public static bool IsWorkerRoleCall = false;
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
        static ContactInformation contactinfo;
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
                contactinfo = GetContactInformation(email, tenant);
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
                string error = TextCodesTranslator.TranslateText("Accounting.General.O.YouDontHavePermission", tenant, showLocal);
                throw new Exception(error + " " + objectTableLocalName + ". Please contact your administrator.");
            }
        }
        private static Dictionary<string, FeaturePM> GetFeaturesForRole(string roleId, List<string> allowedPackages, int tenant, bool forceAPIFeaturesCheck = false)
        {
            // Vladi find Cache problem= can't get the features from cache by allowedPackages
            //string key = $"GetFeaturesForRole({roleId}, {tenant})"; 
            //Dictionary<string, FeaturePM> features = (Dictionary<string, FeaturePM>)CacheManager.CacheWrapper.Get(key); 
            //if (features == null || forceAPIFeaturesCheck)
            //{
            FeatureQuery featuresQuery = new FeatureQuery(tenant);
                List<FeaturePM> fet = featuresQuery.GetAllowedFeaturesForRole(roleId, allowedPackages, tenant);
                return  fet.ToDictionary(d => d.Code + d.ObjectTableId, d => d);
            //    CacheManager.CacheWrapper.Insert(key, features, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            //}
            //return features;
        }
        public static bool CheckFeature(string objectTableName, string featureCode, int tenant)
        {
            bool exists = false;
            string email = null;
            if (HttpContext.Current != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
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
            if (string.IsNullOrWhiteSpace(email))
            {
                email = AuthenticationUtil.ResolveLoggingUserId(tenant);
            }
            contactinfo = GetContactInformation(email, tenant);
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
            if (!exists)
            {
                return false;
            }
            else return true;
        }
        public static ContactInformation GetContactInformation(string email, int tenant, bool forceAPIFeaturesCheck = false)
        {
            int loggedTenant = tenant;
            ContactInformation myContactInfo = null;
            //string key = email + "_" + tenant + "_info";
            if (tenant == 0)
            {
                List<string> allPackages = GetAllPackagesCodes(email, loggedTenant, false);
                myContactInfo = new ContactInformation()
                {
                    ContactEmail = email,
                    IsLogitudeAdmin = true,
                    PackagesCodes = allPackages,
                };
            }
            else
            {
                string token = null;
                IAmitalCloudContext context = AmitalCloudContext.GetContext(0);
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
                    Contact contact = GetSingleContactByEmail(tenant, email, context);
                    if (contact != null)
                    {
                        bool isCustomerCare = false;
                        if (contact.Tenant == 0 && tenant != 0)
                        {
                            User zeroUser = GetUserByMail(email, tenant, context);
                            if (zeroUser != null)
                            {
                                isCustomerCare = true;
                                if (zeroUser.IsDistributor)
                                {
                                    using (TransactionScope scope2 = TransactionFactory.GetNewTransaction())
                                    {
                                        bool isDistributorToCurrentTenant = CheckDistributor(tenant, context, zeroUser);
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
                            User user = GetUserByMail(email, AmitalCloudSettings.LogitudeCRMTenantNumber, context);
                            if (user != null)
                            {
                                tenant = AmitalCloudSettings.LogitudeCRMTenantNumber;
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
                    }
                }
            }
            return myContactInfo;
        }

        private static bool CheckDistributor(int tenant, IAmitalCloudContext context, User zeroUser)
        {
            return new Repository<TenantManagement>(context).GetMulti(a => a.Id == tenant && a.DistributorCode == zeroUser.DistributorCode).Any();
        }

        private static User GetUserByMail(string email, int tenant, IAmitalCloudContext context)
        {
            return new Repository<User>(context).GetMulti(d => d.Contact.Email == email && d.Tenant == tenant).FirstOrDefault();
        }
        public static ContactInfo GetContactInfo(string email, int tenant, bool forceAPIFeaturesCheck = false)
        {
            string cacheKey = $"info_({email}_{tenant})";
            ContactInfo myContactInfo = (ContactInfo)CacheManager.CacheWrapper.Get(cacheKey);
            if (myContactInfo == null || forceAPIFeaturesCheck)
            {
                if (tenant == 0)
                {
                    myContactInfo = new ContactInfo()
                    {
                        ContactEmail = email,
                        IsLogitudeAdmin = true,
                        PackagesCodes = GetAllPackagesCodes(email, tenant, false),
                    };
                }
                else
                {
                    string token = null;
                    IAmitalCloudContext context = AmitalCloudContext.GetContext(0);
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
                        Contact contact = GetSingleContactByEmail(tenant, email, context);
                        if (contact != null)
                        {
                            bool isCustomerCare = false;
                            if (contact.Tenant == 0 && tenant != 0)
                            {
                                User zeroUser = GetUserByMail(email, 0, context);
                                if (zeroUser != null)
                                {
                                    isCustomerCare = true;
                                    if (zeroUser.IsDistributor)
                                    {
                                        using (TransactionScope scope2 = TransactionFactory.GetNewTransaction())
                                        {
                                            bool isDistributorToCurrentTenant = CheckDistributor(tenant, context, zeroUser); 
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
                                User user = GetUserByMail(email, AmitalCloudSettings.LogitudeCRMTenantNumber, context);
                                if (user != null)
                                {
                                    tenant = AmitalCloudSettings.LogitudeCRMTenantNumber;
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
                            myContactInfo = new ContactInfo()
                            {
                                Tenant = contact.Tenant,
                                ContactEmail = contact.Email,
                                IsLogitudeAdmin = isLogitudeAdmin,
                                RolesIds = allRolesIds,
                                PackagesCodes = GetAllPackagesCodes(email, tenant, isCustomerCare),
                            };
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
                        }
                    }
                }
                CacheManager.CacheWrapper.Insert(cacheKey, myContactInfo, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            }
            return myContactInfo;
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
                        IAmitalCloudContext context = AmitalCloudContext.GetContext(tenant);    
                        ObjectTablePM objectTable = ObjectTableQuery.GetObjectTableByCode(objectTableName, tenant);
                        Feature myFeature = new Repository<Feature>(context).GetMulti(a=> a.ObjectTableId == objectTable.Id && a.Code == featureCode
                            && (a.Tenant == tenant || a.Tenant == 0)).FirstOrDefault();   //.GetSingleFeatureByCode(objectTable.Id, featureCode, tenant);
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
                                        Contact contact = GetSingleContactByEmail(tenant, email, context);
                                        User logedUser = new Repository<User>(context).GetMulti(record => record.Id == contact.Id && record.Tenant == tenant).FirstOrDefault();   // .GetSingleUser(contact.Id, tenant, false);
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

        private static Contact GetSingleContactByEmail(int tenant, string email, IAmitalCloudContext context)
        {
            return new Repository<Contact>(context).GetMulti(a => a.Email == email && a.Tenant == tenant).FirstOrDefault();
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
            IAmitalCloudContext myCommonContext = AmitalCloudContext.GetContext(tenant);
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
                        User user = new Repository<User>(myCommonContext).GetMulti(a=>a.Contact.Email==email && a.Tenant==tenant).FirstOrDefault(); //.GetSingleUserByEmail(email, tenant, true);
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
        public static string GetAuthenticatedUser(int tenant)
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
                    IAmitalCloudContext context = AmitalCloudContext.GetContext(tenant);
                    string email = HttpContext.Current.User.Identity.Name;
                    Contact contact = GetSingleContactByEmail(tenant,email, context); 
                    if (contact != null)
                    {
                        CardContact cardContact = context.CardContacts.Where(d => d.ContactId == contact.Id && d.CardId == partnerId).FirstOrDefault();
                        if (cardContact != null)
                        {
                            exists = true;
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
                var url = AmitalCloudSecurityUtility.getLoggedDomain();
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
                                if (version < AmitalCloudSettings.IOSSharedAppMinimumVersion)
                                {
                                    mobileVersionError = "Your application version is out-of-date. Please upgrade your application to the latest version";
                                }
                            }
                            else
                            {
                                if (version < AmitalCloudSettings.AndroidSharedAppMinimumVersion)
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
        public static void AuthenticationOnEntityTenant(string objectTableName, int entityTenant, int authTokenTenant)
        {
            if (entityTenant != authTokenTenant)
                throw new Exception("Sorry! you have no permission to do this operation on Tenant:" + entityTenant + ". Please contact your administrator.");
        }
        public static bool CheckPackageFeature(string objectTableName, string featureCode, int tenant)
        {
            bool exists = false;
            IAmitalCloudContext context = AmitalCloudContext.GetContext(tenant);
            PackagesCodesManager iManager = new PackagesCodesManager(tenant, null, false);
            List<string> allowedPackages = iManager.BasePackagesCodes;
            ObjectTablePM objectTable = ObjectTableQuery.GetObjectTableByCode(objectTableName, tenant);
            if (objectTable != null)
            {
                FeatureQuery featuresQuery = new FeatureQuery(tenant);
                FeaturePM myFeature = featuresQuery.GetSingleFeaturePMByCodeAndObjectTable(featureCode, objectTable.Id, tenant);
                if (myFeature != null)
                {
                    List<PackageFeature> packageFeatures = (from a in context.PackageFeatures
                                                            where allowedPackages.Contains(a.PackageCode)
                                                            && (a.Tenant == tenant || a.Tenant == 0)
                                                            && a.FeatureUniqeCode == myFeature.FeatureUniqeCode
                                                            select a).ToList();
                    if (packageFeatures.Count > 0)
                    {
                        exists = true;
                    }
                }
            }
            return exists;
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

    }

}
