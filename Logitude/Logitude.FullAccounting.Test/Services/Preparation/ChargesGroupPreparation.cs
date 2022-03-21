using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using Logitude.Base.Models.Api;
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
            FullAccountingData.ChargesGroup1ID = GetTestGroup();
        }

        private string GetTestGroup()
        {
            return GetIdByCode(Group) ?? Create(CreateTestGroupInstance());
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
        private string Create(ChargesGroupPM chargesGroupPM)
        {
            var response = APICaller.CallPost<ChargesGroupPM>(chargesGroupPM, Urls.ChargesGroupsController, UserTenant.Token);
            return response.Data?.Id;
        }
        
        private ChargesGroupPM CreateTestGroupInstance()
        {
            return new ChargesGroupPM()
            {
                Code = Group,
                LocalName = "Test Group",
                Name = "Test Group",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{Group},Test Group",
                
            };
        }
        

    }
}
