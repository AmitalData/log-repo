using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ReverseEngineer
{
    public class AccountingIntegrityService
    {
        public string CheckParams(AccountingIntegrityInParam accountingIntegrityInParam)
        {
            if (accountingIntegrityInParam == null)
            {
                return "accountingIntegrityInParam is null";
            }
            var fullAccountingSettingPM = FullAccountingSettingQueryService.Get(accountingIntegrityInParam.Tenant);
            if (fullAccountingSettingPM == null)
            {
                return "FullAccountingSetting is null for tenant " + accountingIntegrityInParam.Tenant;
            }
            if (accountingIntegrityInParam.FromMonthInclusive.Year != accountingIntegrityInParam.ToMonthInclusive.Year)
            {
                return "must the same year";
            }
            if (accountingIntegrityInParam.FromMonthInclusive.Year > accountingIntegrityInParam.ToMonthInclusive.Year)
            {
                return "Bad months";
            }
            if (accountingIntegrityInParam.ToMonthInclusive.Subtract(accountingIntegrityInParam.FromMonthInclusive) > TimeSpan.FromDays(365))
            {
                return "day  Subtract  > 365 ";
            }
            return "";


        }
        public AccountingIntegrityResult CheckIntegrity(AccountingIntegrityInParam accountingIntegrityInParam)
        {

            
            var myAccountingIntegrityResult = new AccountingIntegrityResult();
            try
            {
                var ReverseEngineerLedgerTransactionService =new ReverseEngineerLedgerTransactionService(accountingIntegrityInParam.FromMonthInclusive, accountingIntegrityInParam.Tenant);
            }
            catch (Exception e)
            {

                //throw;
            }
            return myAccountingIntegrityResult;
        }
    }
    public class AccountingIntegrityInParam
    {
        public int Tenant { get; set; }
        public DateTime FromMonthInclusive { get; set; }
        public DateTime ToMonthInclusive { get; set; }
    }
    public class AccountingIntegrityResult
    {

        public List<JournalLineLedgerDTO> JournalLineToLedgerResult { get; set; }
        public List<GLAccountTotalByMonthsDTO> LedgerToMounthTotalResult { get; set; }
        public List<GLAccountBalanceDTO> BalanceInLocalCurrencyResult { get; set; }
        public List<DueLocalBalanceDiffM> DueLocalBalance { get; set; }
        
    }

}
