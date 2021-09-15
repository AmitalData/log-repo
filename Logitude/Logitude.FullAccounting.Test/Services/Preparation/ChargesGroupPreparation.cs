using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using Logitude.Test.Base.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Services.Preparation
{

    public class ChargesGroupPreparation
    {
        const string Group = "G1";
        public void Prepare()
        {
            FullAccountingData.ChargesGroup1ID = GetByCode(Group);
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
            var response = APICaller.CallGetByFilters<List<ChargesGroupPM>>(Urls.ChargesgroupviewsGetByFilters, UserTenant.Token, filter);
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
            var ChargesGroupPM = CreateInstance(code);
            var response = APICaller.CallPost<ChargesGroupPM>(ChargesGroupPM, Urls.ChargesGroupsController, UserTenant.Token);
            return response.Data?.Id;
        }
        private ChargesGroupPM CreateInstance(string code)
        {
            switch (code)
            {
                case Group:
                    return CreateTestGroupInstance(code);
                default:
                    return null;
            }
        }
        private ChargesGroupPM CreateTestGroupInstance(string code)
        {
            return new ChargesGroupPM()
            {
                Code = code,
                LocalName = "Test Group",
                Name = "Test Group",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{code},Test Group",
                
            };
        }
        

    }
}
