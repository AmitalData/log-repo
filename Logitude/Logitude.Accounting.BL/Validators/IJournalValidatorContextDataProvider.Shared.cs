
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.Validators
{


    public interface IJournalValidatorContextDataProvider : IGLAccountDataProvider
    {
        //GLAccountPM GetGLAccount(string GLAccountId, int tenant);//DO NOT USE OBJECT FROM DIFF TENANT
        CurrencyPM GetCurrency(string CurrencyId, int tenant);//DO NOT USE OBJECT FROM DIFF TENANT
        
        List<String> GetGLAccountCurrencyList(string CustomerGLAccountId, int tenant);
        string CheckExternalNoAndSystemReturnJournalNumber(string externalNo, string externalSystem, int tenant);
    }
    public interface IGLAccountDataProvider
    {
        GLAccountPM GetGLAccount(string GLAccountId, int tenant);//DO NOT USE OBJECT FROM DIFF TENANT
    }
    public interface IReconciliationValidatorContextDataProvider : IGLAccountDataProvider
    {

        //GLAccountPM GetGLAccount(string GLAccountId, int tenant);//DO NOT USE OBJECT FROM DIFF TENANT
        List<LedgerTransactionPM> GetLedgerTransactionPMsByIdList(List<string> transactionIdList, int tenant);
    }
}
