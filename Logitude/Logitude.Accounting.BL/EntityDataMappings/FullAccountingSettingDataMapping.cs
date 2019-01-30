
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class FullAccountingSettingDataMapping: IMapping<FullAccountingSettingPM, FullAccountingSetting>
   {

        public void CustomPMToPOCO(FullAccountingSettingPM entityPM, FullAccountingSetting entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(FullAccountingSettingPM entityPM, FullAccountingSetting entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomerControlAccountName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomerControlAccountNumber);
            this.CustomMappedPMProperties.Add(PMPropertyNames.VendorControlAccountName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.VendorControlAccountNumber);
            this.CustomMappedPMProperties.Add(PMPropertyNames.FileControlAccountName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.FileControlAccountNumber);
            this.CustomMappedPMProperties.Add(PMPropertyNames.OceanExportJobControlAccountName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.OceanExportJobControlAccountNumber);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AirExportJobControlAccountName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AirExportJobControlAccountNumber);
            this.CustomMappedPMProperties.Add(PMPropertyNames.OceanImportJobControlAccountName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.OceanImportJobControlAccountNumber);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AirImportJobControlAccountName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AirImportJobControlAccountNumber);

            if (entityPOCO.CustomerControlAccountId != null)
            {
                IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                GLAccountListQueryService gLAccountListQueryService = new GLAccountListQueryService(context);
                GLAccountList gLAccountList = gLAccountListQueryService.GetSingle(entityPOCO.CustomerControlAccountId);
                if (gLAccountList != null)
                {
                    entityPM.CustomerControlAccountName = gLAccountList.EnglishName;
                    entityPM.CustomerControlAccountNumber = gLAccountList.DisplayNumber;
                }
            }

            if (entityPOCO.VendorControlAccountId != null)
            {
                IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                GLAccountListQueryService gLAccountListQueryService = new GLAccountListQueryService(context);
                GLAccountList gLAccountList = gLAccountListQueryService.GetSingle(entityPOCO.VendorControlAccountId);
                if (gLAccountList != null)
                {
                    entityPM.VendorControlAccountName = gLAccountList.EnglishName;
                    entityPM.VendorControlAccountNumber = gLAccountList.DisplayNumber;
                }
            }

            if (entityPOCO.FileControlAccountId != null)
            {
                IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                GLAccountListQueryService gLAccountListQueryService = new GLAccountListQueryService(context);
                GLAccountList gLAccountList = gLAccountListQueryService.GetSingle(entityPOCO.FileControlAccountId);
                if (gLAccountList != null)
                {
                    entityPM.FileControlAccountName = gLAccountList.EnglishName;
                    entityPM.FileControlAccountNumber = gLAccountList.DisplayNumber;
                }
            }

            if (entityPOCO.OceanExportJobControlAccountId != null)
            {
                IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                GLAccountListQueryService gLAccountListQueryService = new GLAccountListQueryService(context);
                GLAccountList gLAccountList = gLAccountListQueryService.GetSingle(entityPOCO.OceanExportJobControlAccountId);
                if (gLAccountList != null)
                {
                    entityPM.OceanExportJobControlAccountName = gLAccountList.EnglishName;
                    entityPM.OceanExportJobControlAccountNumber = gLAccountList.DisplayNumber;
                }
            }
            if (entityPOCO.AirExportJobControlAccountId != null)
            {
                IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                GLAccountListQueryService gLAccountListQueryService = new GLAccountListQueryService(context);
                GLAccountList gLAccountList = gLAccountListQueryService.GetSingle(entityPOCO.AirExportJobControlAccountId);
                if (gLAccountList != null)
                {
                    entityPM.AirExportJobControlAccountName = gLAccountList.EnglishName;
                    entityPM.AirExportJobControlAccountNumber = gLAccountList.DisplayNumber;
                }
            }
            if (entityPOCO.OceanImportJobControlAccountId != null)
            {
                IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                GLAccountListQueryService gLAccountListQueryService = new GLAccountListQueryService(context);
                GLAccountList gLAccountList = gLAccountListQueryService.GetSingle(entityPOCO.OceanImportJobControlAccountId);
                if (gLAccountList != null)
                {
                    entityPM.OceanImportJobControlAccountName = gLAccountList.EnglishName;
                    entityPM.OceanImportJobControlAccountNumber = gLAccountList.DisplayNumber;
                }
            }
            if (entityPOCO.AirImportJobControlAccountId != null)
            {
                IAccountingContext context = AccountingContext.GetContext(entityPOCO.Tenant);
                GLAccountListQueryService gLAccountListQueryService = new GLAccountListQueryService(context);
                GLAccountList gLAccountList = gLAccountListQueryService.GetSingle(entityPOCO.AirImportJobControlAccountId);
                if (gLAccountList != null)
                {
                    entityPM.AirImportJobControlAccountName = gLAccountList.EnglishName;
                    entityPM.AirImportJobControlAccountNumber = gLAccountList.DisplayNumber;
                }
            }

            TenantPM tenantpm = TenantQuery.GetSingleTenantPM(entityPM.Tenant, false);
            if (tenantpm != null)
            {
                entityPM.TenantPaymentTermId = tenantpm.PaymentTermId;
                entityPM.AccountingActivated = tenantpm.AccountingActivated;
                entityPM.AccountingActivationDate = tenantpm.AccountingActivationDate;
            }
           

        }
   }


}
   