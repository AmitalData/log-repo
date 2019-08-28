using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.Validators;
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
    public class ExternalReconcileDataProvider : IExternalReconcileDataProvider
    {
        private IAccountingContext _AccountingContext;
        
        public ExternalReconcileDataProvider(IAccountingContext accountingContext)
        {
            _AccountingContext = accountingContext;
        
        }


        public BankAccountPM GetBankAccountFromTransferAccount(string myLedgerTransactionTransferInCreditAccountId,int tenant)
        {
            var bankAccountQS = new BankAccountQueryService(this._AccountingContext);
            var bankAccountPM = bankAccountQS.GetBankAccountByTransferGLAcccountId(myLedgerTransactionTransferInCreditAccountId, tenant);
            if (bankAccountPM == null)
            {
                throw new Exception("could not found bank from myLedgerTransactionTransferInCredit.id ");
            }

            return bankAccountPM;
        }


        public BankAccountPM GetBankAccountFromReconcileExternalPageLineId(string reconcileExternalPageLineId, int tenant)
        {
            var bankPageLineQS = new ReconcileExternalPageLineRepository(this._AccountingContext);
            var reconcileExternalPageLinePM = bankPageLineQS.GetSingle(reconcileExternalPageLineId, tenant);
            if (reconcileExternalPageLinePM==null)
            {
                throw new Exception("bankPageLine is null");
            }
            var bankPageQS = new ReconcileExternalPageRepository(this._AccountingContext);
            var page = bankPageQS.GetSingle(reconcileExternalPageLinePM.ReconcileExternalPageId, tenant);
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

        public virtual List<LedgerTransactionPM> GetLedgerTransactionList(List<string> theReconcileAgainstLTranIdList,int tenant)
        {
            var qs = new LedgerTransactionQueryService(_AccountingContext);

            var myOldTransToReconcile = qs.GetLedgerTransactionPMsByIdList(theReconcileAgainstLTranIdList, tenant);
            return myOldTransToReconcile;
        }

    }
}
