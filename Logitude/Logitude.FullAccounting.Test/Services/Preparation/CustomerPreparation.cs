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
            var partnerParameters = GetPartnerParameters();
            FullAccountingData.CustomerId = DataPreparation.GetPartnerId(partnerParameters);
            var customer = APICaller.CallGet<CustomerPM>(Urls.CustomersGetSingle(FullAccountingData.CustomerId), UserTenant.Token)?.Data;
            AssertConnectWithGLAccount(customer);
            FullAccountingData.CustomerGLAccountId = customer.Card.GLAccountId;
            FullAccountingData.CustomerMainAddressId = GetAddressID(customer);
        }

        private string GetAddressID(CustomerPM customer)
        {
            var address = APICaller.CallGet<List<AddressPM>>(Urls.GetAllAddressesPMsbyCardId(customer.Card.Id), UserTenant.Token)?.Data;
            return address.FirstOrDefault().Id;
        }

        private PartnerParameters GetPartnerParameters()
        {
            return new PartnerParameters()
            {
                Code = "FACSpecFlowTest",
                IsCustomer = true,
                Name = "FAC SpecFlowTest",
                TypeCode = "CS"
            };
        }
        private void AssertConnectWithGLAccount(CustomerPM customer)
        {

            if (string.IsNullOrEmpty(customer.Card?.GLAccountId))
            {
                ConnectWithGLAccount(customer);
            }
           
        }

        private void ConnectWithGLAccount(CustomerPM customer)
        {
            var account = CreateGLAccountInstance(customer);
            var response = APICaller.CallPost<GLAccountPM>(account, Urls.GLAccountsController, UserTenant.Token);
        }
        private GLAccountPM CreateGLAccountInstance(CustomerPM customer)
        {
            return new GLAccountPM()
            {
                DisplayNumber = customer.Id,
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
