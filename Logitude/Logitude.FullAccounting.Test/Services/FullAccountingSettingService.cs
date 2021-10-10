using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services.Preparation;
using Logitude.FullAccountingTests.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccountingTests.Services
{
    public class FullAccountingSettingService
    {
        public void UpdateSetting()
        {
            UpdateFullAccountingSetting();
            UpdateAccountingPeriodsCurrentMonth();
        }

        private void UpdateFullAccountingSetting()
        {
            var setting = APICaller.CallGet<FullAccountingSettingPM>(Urls.FullAccountingSettingsGetSingle(UserTenant.Tenant), UserTenant.Token).Data;
            if (!setting.AccountingActivated)
            {
                setting.AccountingActivated = true;
                setting.AccountingActivationDate = DateTime.Now;
            }
            setting.DefaultVATTypeId = BillingData.VATTypeZeroId;
            setting.TenantPaymentTermId = BillingData.PaymentTermCashId;
            setting.GLAccounterCounterLength = 8;
            setting.DefaultTaxWithholdPercentage = 0;
            if (string.IsNullOrEmpty(setting.CustomerControlAccountId))
            {
                setting.CustomerControlAccountId = GetCustomerID();
            }
            if (string.IsNullOrEmpty(setting.VendorControlAccountId))
            {
                setting.VendorControlAccountId = GetVendorID();
            }

            APICaller.CallPut<FullAccountingSettingPM>(setting, Urls.FullAccountingSettingsController, UserTenant.Token);
        }

        private string GetVendorID()
        {
            return APICaller.CallGet<InsertedControlAccountPM>(Urls.GetInsertControlAccount("VendorControlAccountId", FullAccountingData.VendorChartOfAccountId), UserTenant.Token)?.Data.AccountId.VendorControlAccountId;
        }

        private string GetCustomerID()
        {
            return APICaller.CallGet<InsertedControlAccountPM>(Urls.GetInsertControlAccount("CustomerControlAccountId", FullAccountingData.CustomerChartOfAccountId), UserTenant.Token)?.Data.AccountId.CustomerControlAccountId;
        }

        private void UpdateAccountingPeriodsCurrentMonth()
        {
            var accountingPeriods = GetAccountingPeriodsByYear(DateTime.Now.Year);
            if(accountingPeriods.Count == 0)
            {
                CreatePeriodsForYear();
                accountingPeriods = GetAccountingPeriodsByYear(DateTime.Now.Year);
            }
            foreach (var item in accountingPeriods)
            {
                item.OpenMonth = DateTime.Now.Month;
                var updateditem = APICaller.CallPut<AccountingPeriodList>(item, Urls.AccountingPeriodsController, UserTenant.Token).Data;
            }
        }

        private void CreatePeriodsForYear()
        {
            APICaller.CallPost<FullAccountingSettingPM>(null, Urls.PostCreatePeriodsForYear(DateTime.Now.Year), UserTenant.Token);
        }

        private List<AccountingPeriodList> GetAccountingPeriodsByYear(int year)
        {
            var filter = new ApiQueryFilters()
            {
                Filter1Name = "Year",
                Filter1Value = $"{year}",
                GetAll = true
            };
            var periods = APICaller.CallGetByFilters<List<AccountingPeriodList>>(Urls.AccountingPeriodViewsGetByFilters, UserTenant.Token, filter).Data;
            return periods;
        }
    }
}
