using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile
{
    public class ExternalReconcileDataProvider
    {
        private IAccountingContext _AccountingContext;
        private int _Tenant;
        public ExternalReconcileDataProvider(IAccountingContext accountingContext,int tenant)
        {
            _AccountingContext = accountingContext;
            _Tenant = tenant;
        }


        public BankAccountPM GetBankAccountFromTransferAccount(string myLedgerTransactionTransferInCreditAccountId)
        {
            var bankAccountQS = new BankAccountQueryService(this._AccountingContext);
            var bankAccountPM = bankAccountQS.GetBankAccountByTransferGLAcccountId(myLedgerTransactionTransferInCreditAccountId, _Tenant);
            if (bankAccountPM == null)
            {
                throw new Exception("could not found bank from myLedgerTransactionTransferInCredit.id ");
            }

            return bankAccountPM;
        }


        public BankAccountPM GetBankAccountFromReconcileExternalPageLineId(string reconcileExternalPageLineId)
        {
            var bankPageLineQS = new ReconcileExternalPageLineRepository(this._AccountingContext);
            var bankPageLine = bankPageLineQS.GetSingle(reconcileExternalPageLineId, _Tenant);
            var bankPageQS = new ReconcileExternalPageRepository(this._AccountingContext);
            var page = bankPageQS.GetSingle(bankPageLine.ReconcileExternalPageId, _Tenant);
            var bankAccountQS = new BankAccountQueryService(this._AccountingContext);
            var bankAccount = bankAccountQS.GetSingle(page.BankAccountId, false, false);
            return bankAccount;

        }
        public ReconcileExternalPageLinePM GetReconcileExternalPageLinePM(int tenant, string reconcileExternalPageLineId)
        {
            var qs = new ReconcileExternalPageLineQueryService(this._AccountingContext);
            var PM = qs.GetSingle(reconcileExternalPageLineId, false, false);
            return PM;
        }

        public virtual List<LedgerTransactionPM> GetLedgerTransactionList(List<string> theReconcileAgainstLTranIdList)
        {
            var qs = new LedgerTransactionQueryService(_AccountingContext);

            var myOldTransToReconcile = qs.GetLedgerTransactionPMsByIdList(theReconcileAgainstLTranIdList, _Tenant);
            return myOldTransToReconcile;
        }

    }
}
