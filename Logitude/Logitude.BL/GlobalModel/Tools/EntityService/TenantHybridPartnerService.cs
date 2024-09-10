using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.GlobalModel.Tools.EntityService
{
    class TenantHybridPartnerService
    {
        private HybridPartnerQuery hybridPartnerQuery;
        private HybridPartnerService hybridPartnerService;
        private ICommonDataContext iCommonDataContext;
        private int tenant;
        public TenantHybridPartnerService(int tenant)
        {
            hybridPartnerQuery = new HybridPartnerQuery(tenant);
            iCommonDataContext = CommonDataContext.GetContext(0);
            hybridPartnerService = new HybridPartnerService(iCommonDataContext);
            this.tenant = tenant;
        }
        public void UpdateHybridPartnerActivity(bool IsActive)
        {
            if (LogitudeSettings.WorkEnvironment != "cloud" && !SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Development))
                return;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(iCommonDataContext);
                var hybridPartner = hybridPartnerRepository.GetHybridPartnersByPartnerTenant(tenant).FirstOrDefault();
                if (hybridPartner == null)
                {
                    scope.Complete();
                    return;
                }
                hybridPartner.InActive = !IsActive;
                hybridPartnerRepository.Update(hybridPartner);
                hybridPartnerRepository.SubmitChanges();
                scope.Complete();
            }
        }

        public void UpdateHybridPartnerHybridization(bool IsHybrid)
        {
            if (LogitudeSettings.WorkEnvironment != "cloud")
                return;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository();
                var hybridPartner = hybridPartnerRepository.GetHybridPartnersByPartnerTenant(tenant).FirstOrDefault();
                if (hybridPartner == null)
                {
                    scope.Complete();
                    return;
                }
                hybridPartner.IsExternalPartner = !IsHybrid;
                hybridPartnerRepository.Update(hybridPartner);
                hybridPartnerRepository.SubmitChanges();
                scope.Complete();
            }
        }

        public void UpdateTenantHybridization(HybridPartnerPM entityPM)
        {
            if (LogitudeSettings.WorkEnvironment != "cloud")
                return;
            TenantRepository tenantRepository = new TenantRepository((int)entityPM.PartnerTenant);
            Tenant tenant = tenantRepository.GetSingleTenant((int)entityPM.PartnerTenant);
            if (tenant == null)
                return;
            tenant.IsHybrid = !entityPM.IsExternalPartner;
            tenantRepository.Update(tenant);
            tenantRepository.SubmitChanges();
        }
        public void UpdateTenantActivity(HybridPartnerPM entityPM)
        {
            if (LogitudeSettings.WorkEnvironment != "cloud")
                return;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                var tenantManagement = tenantManagementRepository.GetSingleTenantManagement((int)entityPM.PartnerTenant);
                if (tenantManagement == null || tenantManagement.GlobalTenant == null)
                {
                    scope.Complete();
                    return;
                }
                tenantManagement.GlobalTenant.IsActive = !entityPM.InActive;
                tenantManagementRepository.Update(tenantManagement);
                tenantManagementRepository.SubmitChanges();
                scope.Complete();
            }
        }
    }
}
