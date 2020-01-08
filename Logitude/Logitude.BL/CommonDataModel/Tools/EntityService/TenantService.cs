using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.GlobalModel;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityPOCOs;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class TenantService
    {
        bool isNewEntity;
        private int tenant;
        public Tenant Poco { get; set; }

        //public int Tenant
        //{
        //    get { return tenant; }
        //    set { tenant = value; }
        //}

        //public ICommonDataContext ObjectContext
        //{
        //    get { return objectContext; }
        //    set { objectContext = value; }
        //}

        private TenantPM entityPM;
        private ICommonDataContext objectContext;
        private TenantRepository entityRepository;
        public TenantService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new TenantRepository(objectContext);
        }

        public void Create(TenantPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.TenantVATManagement = true;

            
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                this.entityPM.Id = TenantCounter.GetNumber();
                this.tenant = entityPM.Id;
                scope.Complete();
            }

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                SettingRepository settingRepository = new SettingRepository();
                Setting setting = settingRepository.GetSingleSetting("1");
                string packageCode = "BUSN";
                if (setting.WorkEnvironment == "customs")
                {
                    packageCode = "CUST";
                }
                else
                {
                    packageCode = !string.IsNullOrEmpty(theEntityPm.PackageCode) ? theEntityPm.PackageCode : "BUSN";
                }

                GlobalTenantRepository globalTenantRepository = new GlobalTenantRepository();
                TenantManagementRepository tenantManagementRep = new TenantManagementRepository();
                GlobalDB database = GetActiveDatabaseNumber();
                int version = globalTenantRepository.GetCurrentVersion();
                GlobalTenant globalTenant = new GlobalTenant()
                {
                    Id = entityPM.Id,
                    GlobalDBId = database.Id,
                    CompanyName = entityPM.Company,
                    Version = version,
                    IsActive = true,
                };

                globalTenantRepository.Add(globalTenant);
                globalTenantRepository.SubmitChanges();

                TenantManagement tenantManagement = new TenantManagement()
                {
                    Id = globalTenant.Id,
                    IsTrial = true,
                    BillingByLogitude = true,
                    Name = globalTenant.CompanyName,
                    CreateDate = DateTime.Now,
                    TrialStartDate = DateTime.Now,
                    TrialEndDate = DateTime.Now.AddDays(31),
                    PackageCode = packageCode,
                    NumberOfUsers = 1,
                    SearchFields = globalTenant.Id + "," + globalTenant.CompanyName + ",1",
                    AWBMessagesCCSTypeCode = "CHAMP",                   
                };

                if (packageCode == "IMPO")
                {
                    tenantManagement.TenantTypeCode = "SHC";
                    tenantManagement.IsDistributorSupportEnabled = true;
                    tenantManagement.DistributorCode = "P2P";
                }

                if (theEntityPm.CreateTenantFromSignUp)
                {
                    tenantManagement.TenantTypeCode = "FOR";
                    tenantManagement.Technology = "AG";
                    //theEntityPm.ExportQuotationsToIntegratedSystem = false;
                }            

                tenantManagementRep.Add(tenantManagement);
                tenantManagementRep.SubmitChanges();
                scope.Complete();
            }

            this.Poco = new Tenant();
            this.Poco.Id = this.entityPM.Id;

            this.InitializeComponent();

            TenantValidating.Validate(theEntityPm);
            TenantTracing.Trace(theEntityPm, Poco, isNewEntity);
            TenantMapping.MapEntity(theEntityPm, Poco, isNewEntity);

            AesFunction aesFunction = new AesFunction();
            Poco.StorageEncryptionKey = aesFunction.GenerateAesKey();


            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            CreateDWHSettings();

        }
        public void Update(TenantPM theEntityPm)
        {
            string entityName = "TenantPM" + theEntityPm.Id;
            CacheManager.CacheWrapper.Remove(entityName);
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleTenant(theEntityPm.Id);

            this.InitializeComponent();

            TenantValidating.Validate(theEntityPm);
            TenantTracing.Trace(theEntityPm, Poco, isNewEntity);
            TenantMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private void InitializeComponent()
        {
            if (isNewEntity)
            {
                this.InitializeBusinessUnit();
            }

            if (string.IsNullOrEmpty(entityPM.VatUniqueTypeCode))
            {
                entityPM.VatUniqueTypeCode = "UNT";
            }

            if (string.IsNullOrEmpty(entityPM.VatMandatoryTypeCode))
            {
                entityPM.VatMandatoryTypeCode = "MNT";
            }

            if (entityPM.VatUniqueTypeCode != "USC")
            {
                entityPM.VatUniqueCountryId = null;
            }

            if (entityPM.VatMandatoryTypeCode != "MSC")
            {
                entityPM.VatMandatoryCountryId = null;
            }

            if (string.IsNullOrEmpty(entityPM.MasterExportFreightPrepaidCollectId))
            {
                entityPM.MasterExportFreightPrepaidCollectId = "P";
            }

            if (string.IsNullOrEmpty(entityPM.MasterExportOtherPrepaidCollectId))
            {
                entityPM.MasterExportOtherPrepaidCollectId = "P";
            }

            if (string.IsNullOrEmpty(entityPM.MasterImportFreightPrepaidCollectId))
            {
                entityPM.MasterImportFreightPrepaidCollectId = "P";
            }

            if (string.IsNullOrEmpty(entityPM.MasterImportOtherPrepaidCollectId))
            {
                entityPM.MasterImportOtherPrepaidCollectId = "P";
            }
        }
        private void InitializeBusinessUnit()
        {
            BusinessUnitRepository businessUnitRepository = new BusinessUnitRepository(tenant);
            bool isBusinessUnitExists = businessUnitRepository.IsBusinessUnitExists(tenant);

            if (!isBusinessUnitExists)
            {
                BusinessUnit businessUnit = new BusinessUnit()
                {
                    Id = tenant.ToString(),
                    Tenant = tenant,
                    InActive = false,
                    Name = "Organization",
                    SearchFields = "Organization",
                    ParentId = null,
                };

                businessUnitRepository.Add(businessUnit);
                businessUnitRepository.SubmitChanges();
            }
        }

        public static GlobalDB GetActiveDatabaseNumber()
        {
            GlobalDBRepository globaldbRep = new GlobalDBRepository();

            List<GlobalDB> activeDbs = globaldbRep.GetActiveDataBases();
            GlobalDB database = null;
            if (activeDbs.Count == 1)
            {
                database = activeDbs.FirstOrDefault();
            }
            if (activeDbs.Count > 1)
            {
                Random rand = new Random();
                int number = rand.Next(activeDbs.Count);
                database = activeDbs[number];
            }

            return database;
        }

        private void CreateDWHSettings()
        {
            var dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                return;
            }
            DWHSetting dWHSetting = new DWHSetting() { Tenant = entityPM.Id, ParentTenant = entityPM.Id, Server = null, Password = null, UserName = null, Catalog = null, IsParentTenant = false };
            DWHSettingRepository dWHSettingRepository = new DWHSettingRepository(dWHSetting.Tenant);
            dWHSettingRepository.Add(dWHSetting);
            dWHSettingRepository.SubmitChanges();
        }
    }
}
