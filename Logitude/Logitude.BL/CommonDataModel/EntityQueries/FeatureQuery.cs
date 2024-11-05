using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Logitude.BL.Security;
using System.Diagnostics;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class FeatureQuery
    {
        FeatureRepository repository;

        public FeatureQuery()
        {
            repository = new FeatureRepository();
        }

        public FeatureQuery(int tenant)
        {
            repository = new FeatureRepository(tenant);
        }

        public FeatureQuery(FeatureRepository repository)
        {
            this.repository = repository;
        }

        public FeaturePM GetSingleFeaturePM(string id)
        {
            return (from a in repository.context.Features.Include("NameTextCode")
                    where a.Id == id
                    select new FeaturePM()
                    {
                        Code = a.Code,
                        Id = a.Id,
                        NameTextCodeId = a.NameTextCodeId,
                        ObjectTableId = a.ObjectTableId,
                        Tenant = a.Tenant,
                        NameTextCodeCode = a.NameTextCodeCode,
                        Packagable = a.Packagable,
                        FeatureTypeCode = a.FeatureTypeCode,
                        IsBusinessUnitEnabled = a.IsBusinessUnitEnabled,
                        IsOld = a.IsOld,
                        IsCoreFeature = a.IsCoreFeature,
                        ToggleCode = a.ToggleCode,
                        FeatureUniqeCode = a.FeatureUniqeCode
                    }).FirstOrDefault();
        }

        public List<FeaturePM> GetFeaturePMsByTenant(int tenant)
        {
            return (from a in repository.context.Features.Include("NameTextCode")
                    where a.Tenant == tenant
                    select new FeaturePM()
                    {
                        Code = a.Code,
                        Id = a.Id,
                        NameTextCodeId = a.NameTextCodeId,
                        ObjectTableId = a.ObjectTableId,
                        Tenant = a.Tenant,
                        NameTextCodeCode = a.NameTextCodeCode,
                        Packagable = a.Packagable,
                        FeatureTypeCode = a.FeatureTypeCode,
                        IsBusinessUnitEnabled = a.IsBusinessUnitEnabled,
                        IsOld = a.IsOld,
                        IsCoreFeature = a.IsCoreFeature,
                        ToggleCode = a.ToggleCode,
                        FeatureUniqeCode = a.FeatureUniqeCode
                    }).ToList();
        }

        public List<FeatureList> GetNewFeaturesList(int tenant)
        {
            return (from a in repository.context.Features.Include("NameTextCode").Include("ObjectTable").Include("FeatureType")
                    where a.Tenant == tenant && a.IsOld == false
                    select new FeatureList()
                    {
                        Code = a.Code,
                        Id = a.Id,
                        NameTextCodeId = a.NameTextCodeId,
                        ObjectTableId = a.ObjectTableId,
                        Tenant = a.Tenant,
                        NameTextCodeCode = a.NameTextCodeCode,
                        Packagable = a.Packagable,
                        FeatureTypeCode = a.FeatureTypeCode,
                        IsBusinessUnitEnabled = a.IsBusinessUnitEnabled,
                        IsOld = a.IsOld,
                        IsCoreFeature = a.IsCoreFeature,
                        ObjectTableName = a.ObjectTable == null ? "" : a.ObjectTable.Name,
                        FeatureTypeName = a.FeatureType == null ? "" : a.FeatureType.Name,
                        FeatureUniqeCode = a.FeatureUniqeCode
                    }).ToList();
        }

        public List<FeaturePM> GetSelectedAndUnSelectedFeatures(string myRoleId, List<string> allowedPackages, int tenant)
        {
            List<FeaturePM> myResult = new List<FeaturePM>();

            if (!string.IsNullOrEmpty(myRoleId))
            {
                Role myRole = (from d in repository.context.Roles where d.Id == myRoleId select d).FirstOrDefault();

                List<FeaturePM> allFeatures = new List<FeaturePM>();
                List<RoleFeature> allRoleFeatures = new List<RoleFeature>();

                #region allFeatures
                allFeatures = (from a in repository.context.Features.Include("NameTextCode").Include("ObjectTable")
                               where (a.Tenant == tenant || a.Tenant == 0)
                               select new FeaturePM()
                               {
                                   Id = a.Id,
                                   Code = a.Code,
                                   Tenant = a.Tenant,
                                   NameTextCodeId = a.NameTextCodeId,
                                   ObjectTableId = a.ObjectTableId,
                                   NameTextCodeCode = a.NameTextCodeCode,
                                   FeatureTypeCode = a.FeatureTypeCode,
                                   Packagable = a.Packagable,
                                   IsBusinessUnitEnabled = a.IsBusinessUnitEnabled,
                                   IsOld = a.IsOld,
                                   IsCoreFeature = a.IsCoreFeature,
                                   RoleId = myRoleId,
                                   RoleTenant = myRole.Tenant,
                                   ParentRoleId = myRole.ParentRoleId,
                                   IsCustomRole = myRole.IsCustomRole,
                                   ObjectTableName = a.ObjectTable == null ? "" : a.ObjectTable.Name,
                                   ToggleCode = a.ToggleCode,
                                   FeatureUniqeCode = a.FeatureUniqeCode
                               }).ToList();
                #endregion

                #region allRoleFeatures
                if (myRole.IsCustomRole)
                {
                    Role myParentRole = (from d in repository.context.Roles where d.Id == myRole.ParentRoleId select d).FirstOrDefault();

                    if (myParentRole != null)
                    {
                        allRoleFeatures = (from d in repository.context.RoleFeatures
                                           where
                                           (d.RoleId == myRole.Id || d.RoleId == myParentRole.Id)
                                           && (d.Tenant == myRole.Tenant || d.Tenant == myParentRole.Tenant)
                                           select d).ToList();

                        foreach (RoleFeature item in allRoleFeatures.Where(d => d.RoleId == myRole.ParentRoleId))
                        {
                            FeaturePM myFeature = (from a in allFeatures where a.FeatureUniqeCode == item.FeatureUniqeCode select a).FirstOrDefault();

                            if (myFeature != null)
                            {
                                myFeature.Exists = true;
                                myFeature.AccessLevelCode = item.FeatureAccessLevelCode;
                            }
                        }

                        foreach (RoleFeature item in allRoleFeatures.Where(d => d.RoleId == myRole.Id))
                        {
                            FeaturePM myFeature = (from a in allFeatures where a.FeatureUniqeCode == item.FeatureUniqeCode select a).FirstOrDefault();

                            if (myFeature != null)
                            {
                                myFeature.Exists = !item.IsDeleted;
                                myFeature.IsCustomRoleFeature = true;
                                myFeature.AccessLevelCode = item.FeatureAccessLevelCode;
                            }
                        }
                    }
                }

                else
                {
                    allRoleFeatures = (from d in repository.context.RoleFeatures
                                       where d.RoleId == myRole.Id && d.Tenant == myRole.Tenant
                                       select d).ToList();

                    foreach (RoleFeature item in allRoleFeatures)
                    {
                        FeaturePM myFeature = (from a in allFeatures where a.FeatureUniqeCode == item.FeatureUniqeCode select a).FirstOrDefault();

                        if (myFeature != null)
                        {
                            myFeature.Exists = true;
                            myFeature.AccessLevelCode = item.FeatureAccessLevelCode;
                        }
                    }
                }
                #endregion

                if (tenant == 0)
                {
                    myResult = allFeatures.ToList();
                }

                else
                {
                    List<PackageFeature> allPackageFeatures = new List<PackageFeature>();
                    List<FeaturePM> allAllowedPackageFeatures = new List<FeaturePM>();

                    if (allowedPackages.Count > 0)
                    {
                        allPackageFeatures = (from a in repository.context.PackageFeatures where allowedPackages.Contains(a.PackageCode) && (a.Tenant == tenant || a.Tenant == 0) select a).ToList();

                        if (allPackageFeatures.Count > 0)
                        {
                            foreach (PackageFeature item in allPackageFeatures)
                            {
                                FeaturePM myFeature = allFeatures.Where(f => f.FeatureUniqeCode == item.FeatureUniqeCode).FirstOrDefault();
                                if (myFeature != null)
                                {
                                    if (!allAllowedPackageFeatures.Where(d => d.Id == myFeature.Id).Any())
                                    {
                                        allAllowedPackageFeatures.Add(myFeature);
                                    }
                                }
                            }
                        }
                    }

                    foreach (FeaturePM myFeature in allFeatures)
                    {
                        bool isAddingItem = false;

                        if (myFeature.Packagable)
                        {
                            FeaturePM myAllowedPackageFeature = allAllowedPackageFeatures.Where(d => d.Id == myFeature.Id).FirstOrDefault();
                            if (myAllowedPackageFeature != null)
                            {
                                isAddingItem = true;
                            }
                        }

                        else
                        {
                            isAddingItem = true;
                        }

                        if (isAddingItem)
                        {
                            if (!myResult.Where(d => d.Id == myFeature.Id).Any())
                            {
                                myResult.Add(myFeature);
                            }
                        }
                    }
                }
            }

            return myResult;
        }

        public List<FeaturePM> GetSelectedAndUnselectedPackagesFeatures(string packageCode, int tenant)
        {
            List<PackageFeature> packageFeature = (from a in repository.context.PackageFeatures
                                                   where a.PackageCode == packageCode && (a.Tenant == tenant || a.Tenant == 0)
                                                   select a).ToList();

            List<FeaturePM> features = (from a in repository.context.Features.Include("NameTextCode")
                                        where (a.Tenant == tenant || a.Tenant == 0)
                                        select new FeaturePM()
                                        {
                                            Code = a.Code,
                                            Id = a.Id,
                                            NameTextCodeId = a.NameTextCodeId,
                                            ObjectTableId = a.ObjectTableId,
                                            Tenant = a.Tenant,
                                            NameTextCodeCode = a.NameTextCodeCode,
                                            FeatureTypeCode = a.FeatureTypeCode,
                                            Packagable = a.Packagable,
                                            IsBusinessUnitEnabled = a.IsBusinessUnitEnabled,
                                            IsOld = a.IsOld,
                                            IsCoreFeature = a.IsCoreFeature,
                                            ToggleCode = a.ToggleCode,
                                            FeatureUniqeCode = a.FeatureUniqeCode
                                        }).ToList();

            List<FeaturePM> ffffff = features.Where(d => d.ObjectTableId == "1-1301").ToList();

            foreach (PackageFeature item in packageFeature)
            {
                FeaturePM feature = (from a in features
                                     where a.FeatureUniqeCode == item.FeatureUniqeCode
                                     select a).FirstOrDefault();

                if (feature != null)
                {
                    feature.Exists = true;
                    feature.PackageCode = item.PackageCode;
                }
            }

            return features;
        }

        public LoggedUserFeatures GetAllowedFeaturesForLoggedUser(string loggedUserId, int tenant)
        {
            LoggedUserFeatures loggedUserFeatures  ;
            if (!SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.LogboxAndAccountingProduction))
            {
                string key = $"GetAllowedFeaturesForLoggedUser,{loggedUserId},{tenant}";
                 loggedUserFeatures = CacheManager.GetOrInsertNewObject<LoggedUserFeatures>(key, () =>
                {
                    return GetAllowedFeaturesForLoggedUserBL(loggedUserId, tenant);
                });
            }
            else
            {
                var sw = Stopwatch.StartNew();
                loggedUserFeatures = GetAllowedFeaturesForLoggedUserBL(loggedUserId, tenant);
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"GetAllowedFeaturesForLoggedUserBL({sw.Elapsed})");
            }


            return loggedUserFeatures;


        }
        LoggedUserFeatures GetAllowedFeaturesForLoggedUserBL(string loggedUserId, int tenant)
        {
            LoggedUserFeatures myResult = new LoggedUserFeatures();

            bool isDistributor = false;
            bool isCustomerCare = false;
            List<FeaturePM> allFeatures = new List<FeaturePM>();
            List<string> allowedPackages = new List<string>();

            ContactTenantRepository contactTenantsRepository = new ContactTenantRepository(this.repository.context);
            ContactTenantRoleRepository contactTenantRolesRepository = new ContactTenantRoleRepository(this.repository.context);
            ContactTenantQuery contactTenantQuery = new ContactTenantQuery(contactTenantsRepository);
            ContactTenantRoleQuery contactTenantRoleQuery = new ContactTenantRoleQuery(contactTenantRolesRepository);
            ContactTenantPM contactTenant = contactTenantQuery.GetContactTenantForUser(loggedUserId, tenant);

            if (contactTenant == null)
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    contactTenantsRepository = new ContactTenantRepository(this.repository.context);
                    contactTenantRolesRepository = new ContactTenantRoleRepository(this.repository.context);

                    contactTenantQuery = new ContactTenantQuery(contactTenantsRepository);
                    contactTenantRoleQuery = new ContactTenantRoleQuery(contactTenantRolesRepository);
                    contactTenant = contactTenantQuery.GetContactTenantForUser(loggedUserId, 0);
                    UserQuery userQuery = new UserQuery(0);
                    UserPM user = userQuery.GetSinglePM(loggedUserId, 0);
                    isDistributor = user.IsDistributor;
                    isCustomerCare = !user.IsDistributor;
                    scope.Complete();
                }
            }

            else if (contactTenant.TenantId == 0)
            {
                UserQuery userQuery = new UserQuery(0);
                UserPM user = userQuery.GetSinglePM(contactTenant.ContactId, 0);
                isDistributor = user.IsDistributor;
                isCustomerCare = !user.IsDistributor;
            }

            List<ContactTenantRolePM> contactTenantRoles = new List<ContactTenantRolePM>();
            if (isCustomerCare || isDistributor)
            {
                contactTenantRoles = contactTenantRoleQuery.GetContactTenantRolesForContactTenant(contactTenant.Id, 0);
            }

            else
            {
                contactTenantRoles = contactTenantRoleQuery.GetContactTenantRolesForContactTenant(contactTenant.Id, tenant);
            }

            if (contactTenantRoles.Count > 0)
            {
                List<string> allRolesIds = contactTenantRoles.Select(s => s.RoleId).ToList();

                RoleRepository myRoleRepository = new RoleRepository(this.repository.context);
                RoleQuery myRoleQuery = new RoleQuery(myRoleRepository);
                List<RolePM> allCustomRoles = myRoleQuery.GetCustomRolesByIds(allRolesIds);

                foreach (RolePM item in allCustomRoles)
                {
                    if (allRolesIds.Contains(item.ParentRoleId))
                    {
                        allRolesIds.Remove(item.ParentRoleId);
                    }
                }

                var isTenantZeroAccess = isCustomerCare || isDistributor ? true : false;
                allowedPackages = this.GetAllPackagesCodes(loggedUserId, tenant, isTenantZeroAccess);

                foreach (string myRoleId in allRolesIds)
                {
                    List<FeaturePM> myFeatures = this.GetAllowedFeaturesForRole(myRoleId, allowedPackages, tenant);

                    foreach (FeaturePM feature in myFeatures)
                    {
                        if (!allFeatures.Contains(feature))
                        {
                            allFeatures.Add(feature);
                        }
                    }
                }
            }

            myResult.Features = allFeatures;
            myResult.AllowedPackagesCodes = allowedPackages;

            return myResult;
        }

        public List<FeaturePM> GetAllowedFeaturesForRole(string myRoleId, List<string> allowedPackages, int tenant)
        {
            List<FeaturePM> myResult = new List<FeaturePM>();

            if (!string.IsNullOrEmpty(myRoleId))
            {
                Role myRole = (from d in repository.context.Roles where d.Id == myRoleId select d).FirstOrDefault();

                List<FeaturePM> allFeatures = new List<FeaturePM>();
                List<RoleFeature> allRoleFeatures = new List<RoleFeature>();
                List<PackageFeature> allPackageFeatures = new List<PackageFeature>();

                #region allFeatures
                allFeatures = (from a in repository.context.Features.Include("NameTextCode")
                               where (a.Tenant == 0 || a.Tenant == tenant)
                               select new FeaturePM()
                               {
                                   Id = a.Id,
                                   Code = a.Code,
                                   Tenant = a.Tenant,
                                   NameTextCodeId = a.NameTextCodeId,
                                   NameTextCodeCode = a.NameTextCodeCode,
                                   ObjectTableId = a.ObjectTableId,
                                   Packagable = a.Packagable,
                                   FeatureTypeCode = a.FeatureTypeCode,
                                   IsBusinessUnitEnabled = a.IsBusinessUnitEnabled,
                                   IsOld = a.IsOld,
                                   IsCoreFeature = a.IsCoreFeature,
                                   ToggleCode = a.ToggleCode,
                                   FeatureUniqeCode = a.FeatureUniqeCode
                               }).ToList();
				#endregion

				#region allRoleFeatures
				RoleFeatureQuery roleFeatureQuery = new RoleFeatureQuery(tenant);

                if (myRole.IsCustomRole)
                {

					allRoleFeatures = roleFeatureQuery.GetRoleFeaturesForRoleFromCache(myRole.ParentRoleId, tenant);

					List<RoleFeature> allChildFeatures = roleFeatureQuery.GetRoleFeaturesForRoleFromCache(myRole.Id, tenant);


                    foreach (RoleFeature item in allChildFeatures)
                    {
                        RoleFeature myParentItem = allRoleFeatures.Where(d => d.FeatureUniqeCode == item.FeatureUniqeCode).FirstOrDefault();

                        if (item.IsDeleted)
                        {
                            if (myParentItem != null)
                            {
                                allRoleFeatures.Remove(myParentItem);
                            }
                        }

                        else
                        {
                            if (myParentItem == null)
                            {
                                allRoleFeatures.Add(item);
                            }
                        }
                    }
                }

                else
                {
                    allRoleFeatures = roleFeatureQuery.GetRoleFeaturesForRoleFromCache(myRole.Id, tenant);
				}
                #endregion

                #region allPackageFeatures
                allPackageFeatures = (from a in repository.context.PackageFeatures where allowedPackages.Contains(a.PackageCode) && (a.Tenant == tenant || a.Tenant == 0) select a).ToList();
                #endregion

                List<FeaturePM> allAllowedPackageFeatures = new List<FeaturePM>();

                if (allPackageFeatures.Count > 0)
                {
                    foreach (PackageFeature item in allPackageFeatures)
                    {
                        FeaturePM myFeature = allFeatures.Where(f => f.FeatureUniqeCode == item.FeatureUniqeCode).FirstOrDefault();
                        if (myFeature != null)
                        {
                            if (!allAllowedPackageFeatures.Where(d => d.Id == myFeature.Id).Any())
                            {
                                myFeature.PackageCode = item.PackageCode;
                                allAllowedPackageFeatures.Add(myFeature);
                            }
                        }
                    }
                }

                if (allRoleFeatures.Count > 0)
                {
                    foreach (RoleFeature item in allRoleFeatures)
                    {
                        FeaturePM myFeature = allFeatures.Where(f => f.FeatureUniqeCode == item.FeatureUniqeCode).FirstOrDefault();
                        if (myFeature != null)
                        {
                            if (!myResult.Where(d => d.Id == myFeature.Id).Any())
                            {
                                myFeature.RoleId = item.RoleId;

                                if (myFeature.Packagable)
                                {
                                    FeaturePM myAllowedPackageFeature = allAllowedPackageFeatures.Where(d => d.Id == myFeature.Id).FirstOrDefault();
                                    if (myAllowedPackageFeature != null)
                                    {
                                        myFeature.Exists = true;
                                        myFeature.PackageCode = myAllowedPackageFeature.PackageCode;
                                        myResult.Add(myFeature);
                                    }
                                }

                                else
                                {
                                    myFeature.Exists = true;
                                    myResult.Add(myFeature);
                                }
                            }
                        }
                    }
                }

                List<FeaturePM> allModuleFeatures = (from a in allAllowedPackageFeatures
                                                     where a.FeatureTypeCode == "MODL"
                                                     select a).ToList();

                foreach (FeaturePM item in allModuleFeatures)
                {
                    if (!myResult.Where(d => d.Id == item.Id).Any())
                    {
                        item.Exists = true;
                        myResult.Add(item);
                    }
                }
            }

            return myResult;
        }

        public bool CheckIfFeatureshasAccess(string myRoleId, List<string> allowedPackages, int tenant, string FeatureCode, string ObjectTableId)
        {
            //List<FeaturePM> myResult = new List<FeaturePM>();
            bool hasAccess = false;
            if (!string.IsNullOrEmpty(myRoleId))
            {
                Role myRole = (from d in repository.context.Roles where d.Id == myRoleId select d).FirstOrDefault();


                List<RoleFeature> allRoleFeatures = new List<RoleFeature>();
                List<PackageFeature> allPackageFeatures = new List<PackageFeature>();

                #region allFeatures
                IQueryable<FeaturePM> allFeatures = (from a in repository.context.Features.Include("NameTextCode")
                                                     where (a.Tenant == 0 || a.Tenant == tenant)
                                                     select new FeaturePM()
                                                     {
                                                         Id = a.Id,
                                                         Code = a.Code,
                                                         Tenant = a.Tenant,
                                                         NameTextCodeId = a.NameTextCodeId,
                                                         NameTextCodeCode = a.NameTextCodeCode,
                                                         ObjectTableId = a.ObjectTableId,
                                                         Packagable = a.Packagable,
                                                         FeatureTypeCode = a.FeatureTypeCode,
                                                         IsBusinessUnitEnabled = a.IsBusinessUnitEnabled,
                                                         IsOld = a.IsOld,
                                                         IsCoreFeature = a.IsCoreFeature,
                                                         ToggleCode = a.ToggleCode,
                                                         FeatureUniqeCode = a.FeatureUniqeCode
                                                     });
                #endregion

                #region allRoleFeatures

                IQueryable<RoleFeature> iQueryable =
                    (from a in repository.context.RoleFeatures
                     where a.Tenant == tenant || a.Tenant == 0
                     select a);

                if (myRole.IsCustomRole)
                {
                    allRoleFeatures = (from a in iQueryable where a.RoleId == myRole.ParentRoleId && a.Feature.Code == FeatureCode && a.Feature.ObjectTableId == ObjectTableId select a).ToList();
                    List<RoleFeature> allChildFeatures = (from a in iQueryable where a.RoleId == myRole.Id && a.Feature.Code == FeatureCode && a.Feature.ObjectTableId == ObjectTableId select a).ToList();

                    foreach (RoleFeature item in allChildFeatures)
                    {
                        RoleFeature myParentItem = allRoleFeatures.Where(d => d.FeatureUniqeCode == item.FeatureUniqeCode).FirstOrDefault();

                        if (item.IsDeleted)
                        {
                            if (myParentItem != null)
                            {
                                allRoleFeatures.Remove(myParentItem);
                            }
                        }

                        else
                        {
                            if (myParentItem == null)
                            {
                                allRoleFeatures.Add(item);
                            }
                        }
                    }
                }

                else
                {
                    allRoleFeatures = (from a in iQueryable where a.RoleId == myRole.Id && a.Feature.Code == FeatureCode && a.Feature.ObjectTableId == ObjectTableId select a).ToList();
                }
                #endregion

                #region allPackageFeatures
                allPackageFeatures = (from a in repository.context.PackageFeatures where allowedPackages.Contains(a.PackageCode) && (a.Tenant == tenant || a.Tenant == 0) && a.Feature.Code == FeatureCode && a.Feature.ObjectTableId == ObjectTableId select a).ToList();
                #endregion

                List<FeaturePM> allAllowedPackageFeatures = new List<FeaturePM>();
                if (allPackageFeatures.Count > 0)
                {
                    foreach (PackageFeature item in allPackageFeatures)
                    {
                        FeaturePM myFeature = allFeatures.Where(f => f.FeatureUniqeCode == item.FeatureUniqeCode && f.ObjectTableId == ObjectTableId).FirstOrDefault();
                        if (myFeature != null)
                        {
                            if (!allAllowedPackageFeatures.Where(d => d.FeatureUniqeCode == myFeature.FeatureUniqeCode).Any())
                            {
                                allAllowedPackageFeatures.Add(myFeature);
                            }
                        }
                    }
                }

                if (allRoleFeatures.Count > 0)
                {
                    foreach (RoleFeature item in allRoleFeatures)
                    {
                        FeaturePM myFeature = allFeatures.Where(f => f.Code == FeatureCode && f.ObjectTableId == ObjectTableId).FirstOrDefault();
                        if (myFeature != null)
                        {
                            myFeature.RoleId = item.RoleId;

                            if (myFeature.Packagable)
                            {
                                FeaturePM myAllowedPackageFeature = allAllowedPackageFeatures.Where(d => d.Id == myFeature.Id).FirstOrDefault();
                                if (myAllowedPackageFeature != null)
                                {
                                    hasAccess = true;
                                }
                            }

                            else
                            {
                                hasAccess = true;
                            }

                        }
                    }
                }
                else
                {
                    hasAccess = false;
                }




            }

            return hasAccess;
        }

        private List<string> GetAllPackagesCodes(string loggedUserId, int tenant, bool isCustomerCare)
        {
            PackagesCodesManager iManager = new PackagesCodesManager(tenant, loggedUserId, isCustomerCare);
            return iManager.BasePackagesCodes;
        }

        private List<string> GetAllPackagesCodes_Old(string loggedUserId, int tenant, bool isCustomerCare)
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
                //bool isCustomerCare = false;

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

        public FeaturePM GetSingleFeaturePMByCodeAndObjectTable(string code, string objectTableId, int tenant)
        {
            return (from a in repository.context.Features.Include("NameTextCode")
                    where a.Code == code && a.ObjectTableId == objectTableId
                    select new FeaturePM()
                    {
                        Code = a.Code,
                        Id = a.Id,
                        NameTextCodeId = a.NameTextCodeId,
                        ObjectTableId = a.ObjectTableId,
                        Tenant = a.Tenant,
                        NameTextCodeCode = a.NameTextCodeCode,
                        Packagable = a.Packagable,
                        FeatureTypeCode = a.FeatureTypeCode,
                        IsBusinessUnitEnabled = a.IsBusinessUnitEnabled,
                        IsOld = a.IsOld,
                        IsCoreFeature = a.IsCoreFeature,
                        ToggleCode = a.ToggleCode,
                        FeatureUniqeCode = a.FeatureUniqeCode
                    }).FirstOrDefault();
        }
    }

    public class LoggedUserFeatures
    {
        public List<FeaturePM> Features { get; set; }
        public List<string> AllowedPackagesCodes { get; set; }
    }

    public class PackagesCodesManager
    {
        public int Tenant { get; set; }
        public string LoggedUserId { get; set; }
        public bool IsCustomerCare { get; set; }
        public string MainPackageCode { get; set; }
        public bool IsMultiPackage { get; set; }
        public bool MainAdditionalPackageApplied { get; set; }
        private bool IsUserAdditionalPackagesOnly { get; set; }
        public List<string> BasePackagesCodes { get; set; }
        public List<string> AdonsPackagesCodes { get; set; }
        public List<string> AdditionalPackagesCodes { get; set; }

        private TenantManagement tenantManagement;
        private ICommonDataContext iCommonContext;
        public PackagesCodesManager(int tenant, string loggedUserId, bool isCustomerCare)
        {
            this.Tenant = tenant;
            this.LoggedUserId = loggedUserId;
            this.IsCustomerCare = isCustomerCare;
            this.BasePackagesCodes = new List<string>();
            this.AdonsPackagesCodes = new List<string>();
            this.AdditionalPackagesCodes = new List<string>();
            this.iCommonContext = CommonDataContext.GetContext(this.Tenant);
            this.GetGlobalData();
            this.GetUserData();
            this.BuildPackages();
        }

        private void GetGlobalData()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IGlobalContext globalContext = GlobalContext.GetContext();

                this.tenantManagement = (from d in globalContext.TenantManagements where d.Id == this.Tenant select d).FirstOrDefault();

                this.AdonsPackagesCodes = (from d in globalContext.TenantAddOns
                                           where d.Tenant == this.Tenant
                                           group d by d.PackageCode into g
                                           select g.Key).ToList();

                this.AdditionalPackagesCodes = (from d in globalContext.TenantManagementLicenses
                                                where d.Tenant == this.Tenant
                                                group d by d.PackageCode into g
                                                select g.Key).ToList();
                this.GetTenantManagementData();

                scope.Complete();
            }
        }

        private void GetTenantManagementData()
        {
            if (this.tenantManagement != null)
            {
                this.Tenant = this.tenantManagement.Id;
                this.MainPackageCode = this.tenantManagement.PackageCode;
                this.IsMultiPackage = this.tenantManagement.IsMultiPackage;
                this.MainAdditionalPackageApplied = this.tenantManagement.MainAdditionalPackageApplied;

                if (!string.IsNullOrEmpty(this.tenantManagement.TemporalPackageCode) && this.tenantManagement.TemporalStartDate != null && this.tenantManagement.TemporalEndDate != null)
                {
                    if (this.tenantManagement.TemporalStartDate.Value.Date <= DateTime.Now.Date && DateTime.Now.Date <= this.tenantManagement.TemporalEndDate.Value.Date)
                    {
                        this.MainPackageCode = this.tenantManagement.TemporalPackageCode;
                    }
                }
            }
        }

        private void GetUserData()
        {
            User loggedUser = this.iCommonContext.Users.Where(d => d.Id == this.LoggedUserId).FirstOrDefault();
            if(loggedUser != null)
            {
                this.IsUserAdditionalPackagesOnly = loggedUser.AdditionalPackagesOnly;
            }
        }

        private void BuildPackages()
        {
            if (this.MainAdditionalPackageApplied)
            {
                this.BuildPackagesFromMainAdditional();
            }

            else
            {
                this.BuildPackagesFromSingleMulti();
            }

            this.FillBasePackages(this.GetConnectedPackages(this.AdonsPackagesCodes));
        }

        public void BuildPackagesFromMainAdditional()
        {
            List<string> allCodes = new List<string>();

            if (this.IsMultiPackage)
            {
                allCodes.AddRange(this.AdditionalPackagesCodes);

                if (!this.IsCustomerCare)
                {
                    allCodes = this.FilterPackagesUserLicenses(allCodes);
                }
            }

            if (!this.IsUserAdditionalPackagesOnly && !allCodes.Contains(this.MainPackageCode))
            {
                allCodes.Add(this.MainPackageCode);
            }

            this.FillBasePackages(this.GetConnectedPackages(allCodes));
        }

        public void BuildPackagesFromSingleMulti()
        {
            if (this.IsMultiPackage)
            {
                if (this.AdditionalPackagesCodes.Count > 0)
                {
                    if (!this.IsCustomerCare)
                    {
                        AdditionalPackagesCodes = this.FilterPackagesUserLicenses(AdditionalPackagesCodes);
                    }
                }

                this.FillBasePackages(this.GetConnectedPackages(this.AdditionalPackagesCodes));
            }

            else
            {
                Package myPackage = (from a in iCommonContext.Packages where a.Code == this.MainPackageCode select a).FirstOrDefault();
                if (myPackage != null)
                {
                    if (myPackage.FeaturePackageTypeCode == "BS")
                    {
                        if (!this.BasePackagesCodes.Contains(this.MainPackageCode))
                        {
                            this.BasePackagesCodes.Add(this.MainPackageCode);
                        }
                    }

                    else
                    {
                        this.FillBasePackages(this.GetConnectedPackages(new List<string>() { this.MainPackageCode }));
                    }
                }
            }
        }

        private List<string> FilterPackagesUserLicenses(List<string> iPackagesCodes)
        {
            List<string> allUserLicenses = new List<string>();

            if (string.IsNullOrEmpty(this.LoggedUserId))
            {
                allUserLicenses = (from a in iCommonContext.UserLicenses
                                   where a.Tenant == this.Tenant
                                   group a by a.PackageCode into g
                                   select g.Key).ToList();
            }

            else
            {
                allUserLicenses = (from a in iCommonContext.UserLicenses
                                                where a.Tenant == this.Tenant
                                                && a.UserId == this.LoggedUserId
                                                group a by a.PackageCode into g
                                                select g.Key).ToList();
            }

            iPackagesCodes = (from a in iPackagesCodes where allUserLicenses.Contains(a) select a).ToList();

            return iPackagesCodes;
        }

        private List<string> GetConnectedPackages(List<string> iPackagesCodes)
        {
            List<string> iConnectedCodes = new List<string>();

            if (iPackagesCodes.Count > 0)
            {
                iConnectedCodes = (from a in iCommonContext.PackageConnectedPackages
                                   where iPackagesCodes.Contains(a.PackageCode)
                                   group a by a.ConnectedPackageCode into g
                                   select g.Key).ToList();
            }

            return iConnectedCodes;
        }

        private void FillBasePackages(List<string> iPackagesCodes)
        {
            foreach (string itemCode in iPackagesCodes)
            {
                if (!this.BasePackagesCodes.Contains(itemCode))
                {
                    this.BasePackagesCodes.Add(itemCode);
                }
            }
        }
    }
}
