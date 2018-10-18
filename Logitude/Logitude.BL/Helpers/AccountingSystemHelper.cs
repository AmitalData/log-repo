using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.Helpers
{
    public class AccountingSystemHelper
    {
        public AccountingSystemPM GetAccountingSystem(int tenant)
        {
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            AccountingSettingQuery accountingSettingQuery = new AccountingSettingQuery(new AccountingSettingRepository(context));
            AccountingSystemQuery accountingSystemQuery = new AccountingSystemQuery(new AccountingSystemRepository(context));

            AccountingSystemPM accountingSystem = null;
            AccountingSettingPM accountingSetting = accountingSettingQuery.GetSingleAccountSettingPM(tenant);

            if (accountingSetting != null)
            {
                string code = accountingSetting.AccountingSystemCode;
                accountingSystem = accountingSystemQuery.GetSingleAccountingSystemPM(code);
            }

            return accountingSystem;
        }
    }
}
