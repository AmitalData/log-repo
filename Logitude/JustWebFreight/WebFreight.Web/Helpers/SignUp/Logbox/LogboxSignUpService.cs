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

namespace WebFreight.Web.Helpers.SignUp
{
    public class LogboxSignUpService
    {
        private SignUpInfoClass signUpInfoClass;
        private int tenant;
        private ICommonDataContext commonContext;
        public LogboxSignUpService(SignUpInfoClass signUpInfo, int Tenant)
        {
            signUpInfoClass = signUpInfo;
            tenant = Tenant;
            commonContext = CommonDataContext.GetContext(tenant);
        }
        public void Update()
        {
            if (signUpInfoClass.PackageCode != "IMPO") return;

            LogboxSignUpCustomerService.CreateNewFromCloud(signUpInfoClass, tenant);

            if (signUpInfoClass.IsCreateLogboxTenantFromCloud)
            {
                LogboxSignUpCustomerService.UpdateFromCloud(signUpInfoClass, commonContext, tenant);
            }
            else
            {
                LogboxSignUpCustomerService.CreateStandardNew(signUpInfoClass, commonContext, tenant);
            }

            AddressPM tenantAddress = LogboxSignUpAddressService.GetNewTenantAddress(signUpInfoClass, commonContext, tenant);
            LogboxSignUpCurrencyService logboxSignUpCurrencyService = new LogboxSignUpCurrencyService(commonContext, tenant);
            logboxSignUpCurrencyService.AddLocalAndProfitCurrency();
            logboxSignUpCurrencyService.UpdateNewTenant(signUpInfoClass, tenantAddress);
        }
    }
}