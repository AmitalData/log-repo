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
    public class BankCodePreparation
    {
        const string BankCode1 = "Bank1";
        public void Prepare()
        {
            FullAccountingData.BankCodeId = GetByCode(BankCode1);
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
            var response = APICaller.CallGetByFilters<List<BankCodePM>>(Urls.BankCodeViewsGetByFilters, UserTenant.Token, filter);
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

        public string Create(string code = null)
        {
            var bankCode = CreateInstance(code);
            var response = APICaller.CallPost<BankCodePM>(bankCode, Urls.BankCodesController, UserTenant.Token);
            return response.Data?.Id;
        }
        private BankCodePM CreateInstance(string code)
        {
            switch (code)
            {
                case BankCode1:
                    return CreateBankCode1Instance(code);
                default:
                    return CreateAny();
            }
        }

        private BankCodePM CreateBankCode1Instance(string code)
        {
            return new BankCodePM()
            { 
                Code = code,
                EnglishName = code,
                LocalName = code,
                Tenant = UserTenant.Tenant,
                SearchFields = $"{code}"

            };
        }
        
        private BankCodePM CreateAny()
        {
            string code = "Br" + DateTime.Now.Ticks;
            return new BankCodePM()
            {
                Code = code,
                EnglishName = code,
                LocalName = code,
                Tenant = UserTenant.Tenant,
                SearchFields = $"{code}"

            };
        }
    }
}
