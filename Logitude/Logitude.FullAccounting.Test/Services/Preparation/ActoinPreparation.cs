using Logitude.FullAccounting.Test.Models;
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
    public partial class FullAccountingPreparation
    {
        public void ActoinPreparation()
        {
            FullAccountingData.CreditActoinID = GetCreditActoinIDByCode("1");
        }

        private string GetCreditActoinIDByCode(string code)
        {
            
        }
        private string GetSprintId(ApiQueryFilters apiQueryFilters)
        {
            var responce = APICaller.CallGet<>(Urls.GetJournalActionTypeByFilters(code), UserTenant.Token).Data; return response.Data?.FirstOrDefault()?.Id;
        }
    }
}
