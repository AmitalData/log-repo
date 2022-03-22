
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Builders;
using Logitude.FullAccounting.Test.Services.Preparation;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Billings;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.FullAccounting.Test.Services
{
    public class CashbooktService
    {   

        public CashBookPM Create(Table table)
        {
            dynamic cashBookTable = table.CreateDynamicInstance();
            var cashBook = new CashBookPM()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                CurrencyId = BillingData.CurrencyNISId,
                AccountId = new AccountPreparation().Create(),
                CashBookTypeCode = (int)CashBookTypeCodeEnum.Cash + "",
                EnglishName = cashBookTable.Name,
                BranchId = new BranchPreparation().GetNewBranch(),
                TotalAmount = 0,
                LocalName = cashBookTable.Name,
                Tenant = UserTenant.Tenant,
                SearchFields = cashBookTable.Name

            };

            return cashBook;

        }


    }
}
