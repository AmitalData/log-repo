using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Helpers.SignUp.Logbox;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;

namespace WebFreight.Web.Helpers.SignUp
{
    public class LogboxSignUpService
    {
        private SignUpInfoClass signUpInfoClass;
        private int tenant;
        private ICommonDataContext commonContext;
        private TenantManagmentPrivateLabels selectedTenantManagmentPrivateLabel;
        public LogboxSignUpService(SignUpInfoClass signUpInfo, int Tenant)
        {
            signUpInfoClass = signUpInfo;
            tenant = Tenant;
            commonContext = CommonDataContext.GetContext(tenant);
        }
        public void Update()
        {
            if (signUpInfoClass.PackageCode != "IMPO") return;

            string connectedCustomerId = LogboxSignUpCustomerService.CreateNewFromCloud(signUpInfoClass, tenant);

            if (signUpInfoClass.IsCreateLogboxTenantFromCloud)
            {
                LogboxSignUpCustomerService.UpdateFromCloud(signUpInfoClass, commonContext, tenant);
            }
            else
            {
                connectedCustomerId = LogboxSignUpCustomerService.CreateStandardNew(signUpInfoClass, commonContext, tenant);
            }

            AddressPM tenantAddress = LogboxSignUpAddressService.GetNewTenantAddress(signUpInfoClass, commonContext, tenant);
            LogboxSignUpCurrencyService logboxSignUpCurrencyService = new LogboxSignUpCurrencyService(commonContext, tenant);
            logboxSignUpCurrencyService.AddLocalAndProfitCurrency();
            logboxSignUpCurrencyService.UpdateNewTenant(signUpInfoClass, tenantAddress, connectedCustomerId);
            UpdateGlobalTenants();
        }

        private void UpdateGlobalTenants()
        {
            HybridPartnerQuery hybridPartnerQuery = new HybridPartnerQuery(tenant);
            HybridPartnerPM selectedHybridPartner = hybridPartnerQuery.GetSinglePMByPartnerTenant(signUpInfoClass.Tenant);
            if (selectedHybridPartner == null) return;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagmentPrivateLabelsRepository tenantManagmentPrivateLabelsRepository = new TenantManagmentPrivateLabelsRepository(GlobalContext.GetContext());
                selectedTenantManagmentPrivateLabel = tenantManagmentPrivateLabelsRepository.GetSingleTenantManagmentPrivateLabelsByHybridPartnerId(selectedHybridPartner.Id);

                GlobalTenantRepository globalTenantRepository = new GlobalTenantRepository();
                GlobalTenant globalTenant = globalTenantRepository.GetGlobalTenantsByTenant(tenant);
                globalTenant.PrivateLabelId = selectedTenantManagmentPrivateLabel.Id;

                globalTenantRepository.Update(globalTenant);
                globalTenantRepository.SubmitChanges();
                scope.Complete();
            }
        }

        public TenantManagmentPrivateLabels GetSelectedTenantManagmentPrivateLabels()
        {
            return selectedTenantManagmentPrivateLabel;
        }
    }
}