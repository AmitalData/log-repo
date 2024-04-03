using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Services.Preparation
{
    public class BranchPreparation
    {
        public void Prepare()
        {
            FullAccountingData.BZUBranchID = GetBerzeitU();
            FullAccountingData.RamallahBranchID = GetRamallah();
        }
        public string GetNewBranch()
        {
            return Create(CreateAny());
        }

        private string GetRamallah()
        {
            return GetIdByCode(BranchCodes.BerzeitU) ?? Create(CreateBZUInstance());
        }

        private string GetBerzeitU()
        {
            return GetIdByCode(BranchCodes.BerzeitU) ?? Create(CreateRamallahInstance());
        }

        private string GetIdByCode(string code)
        {
            var filter = GetFilterByCode(code);
            var response = APICaller.CallGetByFilters<List<BranchPM>>(Urls.BranchviewsByFilters, UserTenant.Token, filter);
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

        public string Create(BranchPM branchPM)
        {
            var response = APICaller.CallPost<BranchPM>(branchPM, Urls.BranchesController, UserTenant.Token);
            return response.Data?.Id;
        }

        private BranchPM CreateBZUInstance()
        {
            return new BranchPM()
            { 
                Code = BranchCodes.BerzeitU,
                EnglishName = "BerzeitU",
                LocalName = "BerzeitU",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{BranchCodes.BerzeitU},BerzeitU"

            };
        }
        private BranchPM CreateRamallahInstance()
        {
            return new BranchPM()
            { 
                Code = BranchCodes.Ramallah,
                EnglishName = "RMLAH",
                LocalName = "RMLAH",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{BranchCodes.Ramallah},RMLAH"

            };
        }
        private BranchPM CreateAny()
        {
            string name = "Br" + DateTime.Now.Ticks;
            return new BranchPM()
            {
                EnglishName = name,
                LocalName = name,
                Tenant = UserTenant.Tenant,
                SearchFields = $"{name}"

            };
        }

    }
}
