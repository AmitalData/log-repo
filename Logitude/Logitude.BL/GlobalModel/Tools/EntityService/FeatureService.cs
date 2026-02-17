using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.GlobalModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Transactions;

namespace Logitude.BL.GlobalModel.Tools.EntityService
{
    public class FeatureService
    {
        bool isNewEntity;
        private int tenant;
        public Feature Poco { get; set; }
        private FeaturePM entityPM;
        private IGlobalContext objectContext;
        private FeatureRepository entityRepository;
        private RoleFeatureRepository roleFeatureRepository;
        private PackageFeatureRepository packageFeatureRepository;
        public bool RoleFeatureAdded = false;
        public bool RoleFeatureRemoved = false;
        public FeatureService(IGlobalContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new FeatureRepository(objectContext);
            this.roleFeatureRepository = new RoleFeatureRepository(objectContext);
            this.packageFeatureRepository = new PackageFeatureRepository(objectContext);
        }

        public void Create(FeaturePM theEntityPm)
        {
            this.isNewEntity = true;
            this.RoleFeatureAdded = false;
            this.RoleFeatureRemoved = false;

            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("Feature", tenant).ToString();
            this.Poco = new Feature();
            this.Poco.Id = this.entityPM.Id;

            FeatureValidating.Validate(theEntityPm);
            FeatureTracing.Trace(theEntityPm, Poco, isNewEntity);
            FeatureMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }
        public void Update(FeaturePM entityPM)
        {
            this.isNewEntity = false;
            this.RoleFeatureAdded = false;
            this.RoleFeatureRemoved = false;

            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleFeature(entityPM.Id);

            if (!string.IsNullOrEmpty(entityPM.RoleId))
            {
                if (CacheManager.CacheWrapper.Get(entityPM.RoleId) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityPM.RoleId);
                }
            }

            else if (!string.IsNullOrEmpty(entityPM.PackageCode))
            {
                if (CacheManager.CacheWrapper.Get(entityPM.PackageCode) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityPM.PackageCode);
                }
            }

            string listName = "featureslist" + entityPM.RoleId + tenant;

            if (CacheManager.CacheWrapper.Get(listName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(listName);
            }

            FeatureValidating.Validate(entityPM);
            FeatureTracing.Trace(entityPM, Poco, isNewEntity);
            FeatureMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            if (!string.IsNullOrEmpty(entityPM.RoleId))
            {
                this.UpdateRoleFeature();
                this.objectContext.SaveChanges();
            }

            else if (!string.IsNullOrEmpty(entityPM.PackageCode))
            {
                this.UpdatePackageFeature();
                this.objectContext.SaveChanges();
            }
        }

        private void UpdateRoleFeature()
        {
            RoleFeature myRoleFeature = roleFeatureRepository.GetRoleFeatureByRoleAndFeatureUCode(entityPM.RoleId, entityPM.FeatureUniqeCode, entityPM.RoleTenant);

            if (entityPM.AccessLevelCode == "NO")
            {       
                this.RemoveRoleFeature(myRoleFeature);
                this.RoleFeatureRemoved = true;
            }

            else
            {
                this.AddRoleFeature(myRoleFeature);
                this.RoleFeatureAdded = true;
            }
        }
        private void AddRoleFeature(RoleFeature myRoleFeature)
        {
            if (myRoleFeature == null)
            {
                myRoleFeature = new RoleFeature()
                {
                    Id = IdCounter.GetNumber("RoleFeature", tenant).ToString(),
                    RoleId = entityPM.RoleId,
                    FeatureId = entityPM.Id,
                    Tenant = entityPM.RoleTenant,
                    IsDeleted = false,
                    FeatureAccessLevelCode = entityPM.AccessLevelCode,
                    FeatureUniqeCode = entityPM.FeatureUniqeCode,
                };

                roleFeatureRepository.Add(myRoleFeature);
            }

            else
            {
                myRoleFeature.IsDeleted = false;
                myRoleFeature.FeatureAccessLevelCode = entityPM.AccessLevelCode;
                myRoleFeature.FeatureUniqeCode = entityPM.FeatureUniqeCode;

                roleFeatureRepository.Update(myRoleFeature);
            }
        }
        private void RemoveRoleFeature(RoleFeature myRoleFeature)
        {
            if (entityPM.ParentRoleId != null)
            {
                if (myRoleFeature == null)
                {
                    myRoleFeature = new RoleFeature()
                    {
                        Id = IdCounter.GetNumber("RoleFeature", tenant).ToString(),
                        RoleId = entityPM.RoleId,
                        FeatureId = entityPM.Id,
                        Tenant = entityPM.RoleTenant,
                        IsDeleted = true,
                        FeatureAccessLevelCode = entityPM.AccessLevelCode,
                        FeatureUniqeCode = entityPM.FeatureUniqeCode,

                    };

                    roleFeatureRepository.Add(myRoleFeature);
                }

                else
                {
                    myRoleFeature.IsDeleted = true;
                    myRoleFeature.FeatureAccessLevelCode = entityPM.AccessLevelCode;
                    myRoleFeature.FeatureUniqeCode = entityPM.FeatureUniqeCode;

                    roleFeatureRepository.Update(myRoleFeature);
                }
            }

            else
            {
                if (myRoleFeature != null)
                {
                    roleFeatureRepository.Remove(myRoleFeature);
                }
            }           
        }

        private void UpdatePackageFeature()
        {
            if (entityPM.IsAdded)
            {
                PackageFeature instanceDb = packageFeatureRepository.GetSinglePackageFeatureByPackageAndFeatureUCode(entityPM.PackageCode, entityPM.FeatureUniqeCode, tenant);
                if (instanceDb == null)
                {
                    PackageFeature newPackageFeature = new PackageFeature()
                    {
                        Id = IdCounter.GetNumber("PackageFeature", tenant).ToString(),
                        PackageCode = entityPM.PackageCode,
                        FeatureId = entityPM.Id,
                        Tenant = tenant,
                        FeatureUniqeCode = entityPM.FeatureUniqeCode
                    };

                    packageFeatureRepository.Add(newPackageFeature);
                }
            }

            if (entityPM.IsRemoved)
            {
                PackageFeature packageFeature = packageFeatureRepository.GetSinglePackageFeatureByPackageAndFeatureUCode(entityPM.PackageCode, entityPM.FeatureUniqeCode, tenant);
                if (packageFeature != null)
                {
                    packageFeatureRepository.Remove(packageFeature);

                    if (entityPM.Code == "MOBILE")
                    {
                        #region Disable Mobile From Tenant

                        if (!string.IsNullOrEmpty(entityPM.PackageCode))
                        {
                            bool isUpdate = false;
                            TenantManagementRepository tenantManagementRepository;
                            TenantRepository tenantRepository;
                            List<string> tenantManagementIds = null;
                            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                            {
                                tenantManagementRepository = new TenantManagementRepository();
                                tenantManagementIds = tenantManagementRepository.GetTenantManagementsIdsForPackage(entityPM.PackageCode.Substring(0, 4));
                                scope.Complete();
                            }

                            if (tenantManagementIds != null && tenantManagementIds.Count > 0)
                            {
                                tenantRepository = new TenantRepository(entityPM.Tenant);
                                List<Tenant> tenantLists = tenantRepository.GetTenantListByListIds(tenantManagementIds);
                                foreach (Tenant item in tenantLists)
                                {
                                    if (item != null && item.IsMobileActivated)
                                    {
                                        item.IsMobileActivated = false;
                                        tenantRepository.Update(item);
                                        isUpdate = true;

                                    }

                                }

                                if (isUpdate) tenantRepository.SubmitChanges();
                            }
                        }
                        #endregion
                    }
                }
            }
        }

    }
}
