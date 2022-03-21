using Logitude.FullAccounting.Test.Models;
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
    public class BankCodePreparation
    {
        const string BankCode1 = "Bank1";
        public void Prepare()
        {
            FullAccountingData.BankCodeId = GetBankCode1();
        }

        private string GetBankCode1()
        {
            return GetIdByCode(BankCode1) ?? Create(CreateBankCode1Instance());
        }
        public string GetNewBankCode()
        {
            return  Create(CreateAny());
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

        private string Create(BankCodePM bankCode)
        {
            var response = APICaller.CallPost<BankCodePM>(bankCode, Urls.BankCodesController, UserTenant.Token);
            return response.Data?.Id;
        }
        

        private BankCodePM CreateBankCode1Instance()
        {
            return new BankCodePM()
            { 
                Code = BankCode1,
                EnglishName = BankCode1,
                LocalName = BankCode1,
                Tenant = UserTenant.Tenant,
                SearchFields = $"{BankCode1}"

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
