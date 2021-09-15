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
    public class BranchPreparation
    {
        const string BZU = "BZU";
        const string Ramallah = "RMLAH";
        public void Prepare()
        {
            FullAccountingData.BZUBranchID = GetByCode(BZU);
            FullAccountingData.RamallahBranchID = GetByCode(Ramallah);
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
            var response = APICaller.CallGetByFilters<List<BranchPM>>(Urls.JournalActionTypeViewsByFilters, UserTenant.Token, filter);
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
            var branchPM = CreateInstance(code);
            var response = APICaller.CallPost<BranchPM>(branchPM, Urls.BranchesController, UserTenant.Token);
            return response.Data?.Id;
        }
        private BranchPM CreateInstance(string code)
        {
            switch (code)
            {
                case BZU:
                    return CreateBZUInstance(code);
                case Ramallah:
                    return CreateRamallahInstance(Ramallah);
                default:
                    return null;
            }
        }

        private BranchPM CreateBZUInstance(string code)
        {
            return new BranchPM()
            { 
                Code = code,
                EnglishName = "BZU",
                LocalName = "BZU",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{code},BZU"

            };
        }
        private BranchPM CreateRamallahInstance(string code)
        {
            return new BranchPM()
            { 
                Code = code,
                EnglishName = "BZU",
                LocalName = "BZU",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{code},BZU"

            };
        }
        
    }
}
