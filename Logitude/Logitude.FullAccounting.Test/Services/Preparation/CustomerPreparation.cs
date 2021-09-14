using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Services.Preparation
{
    public class CustomerPreparation
    {

        public void Prepare()
        {
            FullAccountingData.CustomerId = Create();
            CheckHasGLAccount();
        }
        
        private string Create()
        {
            var partnerParameters = GetPartnerParameters();
            return DataPreparation.CreatePartnerForUserTenant(partnerParameters);
        }

        private PartnerParameters GetPartnerParameters()
        {
            return new PartnerParameters()
            {
                Code = "FAC SpecFlowTest",
                IsCustomer = true,
                Name = "FAC SpecFlowTest",
                TypeCode = "CS"
            };
        }
        private void CheckHasGLAccount()
        {
            var customer = APICaller.CallGet<CustomerPM>(Urls.CustomersGetSingle(FullAccountingData.CustomerId), UserTenant.Token)?.Data;
            if (string.IsNullOrEmpty(customer.Card?.GLAccountId))
            {
                ConectWithGLAccount(customer);
            }
        }

        private void ConectWithGLAccount(CustomerPM customer)
        {
            

        }
        private GLAccountPM CreateGLAccountInstance(CustomerPM customer)
        {
            return new GLAccountPM()
            {
                DisplayNumber = customer.Code,
                NewGLAccountCardId = customer.Card.Id,
                IsMultiCurrency = false,
                AccountTypeCode = (int)GLAccountTypeEnum.Client + "",
                CurrencyId = BillingData.CurrencyNISId,
                Tenant = UserTenant.Tenant,
                AutomaticReconcileId = FullAccountingData.AutomaticReconcile_A,
                ChartOfAccountsTypeCode = (int)ChartOfAccountsTypeEnum.Customers + "",
                ReconcileMethodCode = (int)ReconcileMethodEnum.Local + "",
                RevenueExpenseType = (int)RevenueExpenseTypeEnum.Other + "",
                ChartOfAccountsId = FullAccountingData.CustomerChartOfAccountId,
                LocalName = customer.LocalName,
                EnglishName = customer.EnglishName,
            };
        }
    }
}
