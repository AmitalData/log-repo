
using Logitude.Accounting.Data.EntityPOCOs;
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

    public interface IExternalReconcileDataProvider 
    {
        BankAccountPM GetBankAccountFromTransferAccount(string myLedgerTransactionTransferInCreditAccountId, int tenant);//DO NOT USE OBJECT FROM DIFF TENANT
        BankAccountPM GetBankAccountFromReconcileExternalPageLineId(string reconcileExternalPageLineId, int tenant);

        ReconcileExternalPageLinePM GetReconcileExternalPageLinePM(int tenant, string reconcileExternalPageLineId);
        List<LedgerTransactionPM> GetLedgerTransactionList(List<string> theReconcileAgainstLTranIdList, int tenant);
        DateTime GetCurrentDateTime(int tenant);
        string ResolveUserId(int tenant);
    }
    public interface IGLAccountDataProvider
    {
        GLAccountPM GetGLAccount(string GLAccountId, int tenant);//DO NOT USE OBJECT FROM DIFF TENANT
    }
    public interface IReconciliationValidatorContextDataProvider : IGLAccountDataProvider
    {

        //GLAccountPM GetGLAccount(string GLAccountId, int tenant);//DO NOT USE OBJECT FROM DIFF TENANT
        List<LedgerTransactionPM> GetLedgerTransactionPMsByIdList(List<string> transactionIdList, int tenant);
        List<JournalLine> GetJournalLineByLedgerTransactionIdList(List<string> transactionIdList, int tenant);
        
    }
}
