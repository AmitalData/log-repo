using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Services.Preparation
{
    public class AccountingPaymentMethodPreparation
    {
        public void Prepare()
        {
            FullAccountingData.CashPaymentMethodId = GetCash();
            FullAccountingData.ChequePaymentMethodId = GetCheque();
        }

        private string GetCheque()
        {
            return GetIdByCode(AccountingPaymentMethodCodes.Cheque) ?? Create(CreateCashInstance());
        }

        private string GetCash()
        {
            return GetIdByCode(AccountingPaymentMethodCodes.Cash) ?? Create(CreateChequeInstance());
        }

        private string GetIdByCode(string code)
        {
            var filter = GetFilterByCode(code);
            var response = APICaller.CallGetByFilters<List<AccountingPaymentMethodPM>>(Urls.AccountingPaymentMethodViewsByFilters, UserTenant.Token, filter);
            var method = response.Data?.FirstOrDefault();
            method.Should().NotBeNull();
            CheckActivate(method);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private ApiQueryFilters GetFilterByCode(string code)
        {
            return new ApiQueryFiltersBuilder()
                .WithDefualtValues()
                .Filter1Name("Code")
                .Filter1Value(code)
                .Build();
        }

        private string Create(AccountingPaymentMethodPM accountingPaymentMethodPM)
        {
            var response = APICaller.CallPost<AccountingPaymentMethodPM>(accountingPaymentMethodPM, Urls.AccountingPaymentMethodsController, UserTenant.Token);
            return response.Data?.Id;
        }
       

        private AccountingPaymentMethodPM CreateCashInstance()
        {
            return new AccountingPaymentMethodPM()
            { 
                Code = AccountingPaymentMethodCodes.Cash,
                LocalName = "Cash",
                Name = "Cash",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{AccountingPaymentMethodCodes.Cash},Cash"

            };
        }
       private AccountingPaymentMethodPM CreateChequeInstance()
        {
            return new AccountingPaymentMethodPM()
            { 
                Code = AccountingPaymentMethodCodes.Cheque,
                LocalName = "Cheque",
                Name = "Cheque",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{AccountingPaymentMethodCodes.Cheque},Cheque"

            };
        }
        private void CheckActivate(AccountingPaymentMethodPM method)
        {
            if (method.Inactive)
            {
                method.Inactive = false;
                APICaller.CallPut<AccountingPaymentMethodPM>(method, Urls.AccountingPaymentMethodsController, UserTenant.Token);
            }
        }

    }
}
