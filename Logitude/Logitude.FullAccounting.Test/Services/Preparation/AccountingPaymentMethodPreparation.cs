using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.Api;
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
    public class AccountingPaymentMethodPreparation
    {
        const string CashPaymentMethod = "CA";
        public void Prepare()
        {
            FullAccountingData.CashPaymentMethodId = GetByCode(CashPaymentMethod);
        }
        private string GetByCode(string code)
        {
            var id = GetIdByCode(code);
            if (string.IsNullOrEmpty(id))
            {
                return Create(code);
            }
            return id;
        }
        private string GetIdByCode(string code)
        {
            var filter = GetFilterByCode(code);
            var response = APICaller.CallGetByFilters<List<AccountingPaymentMethodPM>>(Urls.AccountingPaymentMethodViewsByFilters, UserTenant.Token, filter);
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

        private string Create(string code)
        {
            var accountingPaymentMethodPM = CreateInstance(code);
            var response = APICaller.CallPost<AccountingPaymentMethodPM>(accountingPaymentMethodPM, Urls.AccountingPaymentMethodsController, UserTenant.Token);
            return response.Data?.Id;
        }
        private AccountingPaymentMethodPM CreateInstance(string code)
        {
            switch (code)
            {
                case CashPaymentMethod:
                    return CreateCashInstance(code);
                
                default:
                    return null;
            }
        }

        private AccountingPaymentMethodPM CreateCashInstance(string code)
        {
            return new AccountingPaymentMethodPM()
            { 
                Code = code,
                LocalName = "Cash",
                Name = "Cash",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{code},Cash"

            };
        }
       
    }
}
