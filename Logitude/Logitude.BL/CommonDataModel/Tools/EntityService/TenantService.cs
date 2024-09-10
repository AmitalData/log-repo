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
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class TenantService
    {
        bool isNewEntity;
        private int tenant;
        public Tenant Poco { get; set; }
        public LogBoxTenantSetting LBtenantsettingPoco { get; set; }
        private TenantPM entityPM;
        private ICommonDataContext objectContext;
        private TenantRepository entityRepository;
        private LogBoxTenantSettingPM LBTenantSettingentityPM;
        private LogBoxTenantSettingRepository LBsettingentityRepository;
        public TenantService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new TenantRepository(objectContext);
            this.LBsettingentityRepository = new LogBoxTenantSettingRepository(objectContext);
        }

        public void Create(TenantPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.TenantVATManagement = true;
            this.entityPM.UseNewTermsOfUse = SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Development);
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                this.entityPM.Id = TenantCounter.GetNumber();
                this.tenant = entityPM.Id;
                scope.Complete();
            }

            PackageRepository packageRepository = new PackageRepository(0);
            List<Package> packages = packageRepository.GetPackages().ToList();


            string packageCode = "BUSN";
            Setting setting;
            using (TransactionScope setScope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 0, 0)))
            {
                SettingRepository settingRepository = new SettingRepository();
                  setting = settingRepository.GetSingleSetting("1");
                setScope.Complete();
            }
           


            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
 
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
                    NumberOfUsers = IsEmptyTotalDefaultNumberOfUsers(theEntityPm) ? 1 : theEntityPm.TotalDefaultNumberOfUsers,
                    TotalNumberOfUsers = IsEmptyTotalDefaultNumberOfUsers(theEntityPm) ? 1 : theEntityPm.TotalDefaultNumberOfUsers,
                    SearchFields = globalTenant.Id + "," + globalTenant.CompanyName + ",1",
                    AWBMessagesCCSTypeCode = "CHAMP",
                    DPArchiveShipmentArrivalFilter = 3,
                    DPArchiveShipmentDepartFilter = 3,
                    DPArchiveShipmentCreateFilter = 12
            };

                Package tenantPackage = packages.Where(d => d.Code == tenantManagement.PackageCode).FirstOrDefault();
                if (tenantPackage != null)
                {
                    tenantManagement.PackageName = tenantPackage.Name;
                }

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
                
                MapNewLogboxFromCloudTenantManagement(tenantManagement, theEntityPm);
                
                tenantManagementRep.Add(tenantManagement);
                tenantManagementRep.SubmitChanges();


                scope.Complete();
            }

            this.Poco = new Tenant();
            this.Poco.Id = this.entityPM.Id;
            this.LBtenantsettingPoco = new LogBoxTenantSetting();
            this.LBtenantsettingPoco.Id = this.entityPM.Id;
            this.InitializeComponent();

            if (!(setting.WorkEnvironment == "customs"))

            {
                TenantValidating.Validate(theEntityPm);
            }
            TenantTracing.Trace(theEntityPm, Poco, isNewEntity);
            TenantMapping.MapEntity(theEntityPm, Poco, isNewEntity);

            AesFunction aesFunction = new AesFunction();
            Poco.StorageEncryptionKey = aesFunction.GenerateAesKey();
            
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            LBsettingentityRepository.Add(LBtenantsettingPoco);
            LBsettingentityRepository.SubmitChanges();
            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
            //    TenantManagementLicenseRepository tenantManagementLicenseRepository = new TenantManagementLicenseRepository();
            //    TenantManagementLicense tenantManagementLicense = new TenantManagementLicense()
            //    {
            //        Id = IdCounter.GetNumber("TenantManagementLicense", Poco.Id),
            //        Tenant = Poco.Id,
            //        NumberOfUsers = 1,
            //        PackageCode = packageCode,
            //    };
            //    tenantManagementLicenseRepository.Add(tenantManagementLicense);
            //    tenantManagementLicenseRepository.SubmitChanges();

            //    scope.Complete();
            //}

            CreateDWHSettings();
        }

        private static void MapNewLogboxFromCloudTenantManagement(TenantManagement tenantManagement, TenantPM newTenantPM)
        {
            if (!newTenantPM.IsNewLogboxFromCloud) return;

            const string ShipperConsigneeTenantTypeCode = "SHC";
            tenantManagement.TenantTypeCode = ShipperConsigneeTenantTypeCode;
            tenantManagement.IsTrial = false;
        }

        private static bool IsEmptyTotalDefaultNumberOfUsers(TenantPM theEntityPm)
        {
            return theEntityPm.TotalDefaultNumberOfUsers == null || theEntityPm.TotalDefaultNumberOfUsers <= 0;
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

            this.LBtenantsettingPoco = LBsettingentityRepository.GetSingleLBTenant(theEntityPm.Id);
            this.LBtenantsettingPoco.IsDocumentsArchive = theEntityPm.IsDocumentsArchive;
            this.LBtenantsettingPoco.CustomerTenantShareImportFile = theEntityPm.CustomerTenantShareImportFile;
            this.LBtenantsettingPoco.AutoArchiveOnInvoice = theEntityPm.AutoArchiveOnInvoice;
            this.LBtenantsettingPoco.AutoArchiveOnPODExport = theEntityPm.AutoArchiveOnPODExport;
            this.LBtenantsettingPoco.DocumentShareAsDefault = theEntityPm.DocumentShareAsDefault;

            LBsettingentityRepository.Update(this.LBtenantsettingPoco);
            LBsettingentityRepository.SubmitChanges();
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
