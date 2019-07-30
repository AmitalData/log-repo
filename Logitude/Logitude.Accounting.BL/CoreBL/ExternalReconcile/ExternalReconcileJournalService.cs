using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile
{
    public class ExternalReconcileJournalService
    {
        private ExternalReconcileDataProvider _ExternalReconcileDataProvider;

        public JournalPM TheJournalPM { get; private set; }

        public void MustInit(ExternalReconcileDataProvider externalReconcileDataProvider)
        {
            _ExternalReconcileDataProvider = externalReconcileDataProvider;
        }

        public void MoveBankCheckFromTransfer2GLAccount(int tenant, string ledgerTransactionId, string reconcileExternalPageLineId)
        {


            LedgerTransactionPM myLedgerTransactionTransferPM = _ExternalReconcileDataProvider.GetLedgerTransactionList(new List<string>() { ledgerTransactionId }).FirstOrDefault();
            if (myLedgerTransactionTransferPM == null)
            {
                throw new Exception("Ledger not exist ");
            }
            BankAccountPM bankAccountFromTransfer = _ExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferPM.AccountId);

            BankAccountPM bankAccountFromReconcileExternalPageLine = _ExternalReconcileDataProvider.GetBankAccountFromReconcileExternalPageLineId(reconcileExternalPageLineId);

            ReconcileExternalPageLinePM myReconcileExternalPageLinePM = this._ExternalReconcileDataProvider.GetReconcileExternalPageLinePM(tenant, reconcileExternalPageLineId);
            


            var errString= Validate(tenant, myLedgerTransactionTransferPM, myReconcileExternalPageLinePM, bankAccountFromTransfer, bankAccountFromReconcileExternalPageLine);

            if (!string.IsNullOrWhiteSpace(errString))
            {
                throw new Exception(errString);
            }
            JournalPM journal = CreateJournal(myLedgerTransactionTransferPM, myReconcileExternalPageLinePM, bankAccountFromTransfer);
            if (journal.JournalExternalReconciles.Count > 1)
            {
                throw new Exception("Sorry meanwhile only one Adjust Allowed !!!");
            }
            TheJournalPM = journal;
        }

        private JournalPM CreateJournal(LedgerTransactionPM myLedgerTransactionTransferPM, ReconcileExternalPageLinePM myReconcileExternalPageLinePM, BankAccountPM myBankAccountPM)
        {
            //newJournalMoveBankCheckFromTransfer2GLAccount/
               var journal = new JournalPM()
            {

            };
            //newJournalMoveBankCheckFromTransfer2GLAccount.JournalReconciles.Add()
            //newJournalMoveBankCheckFromTransfer2GLAccount.JournalLines.Add
              journal.Tenant = myLedgerTransactionTransferPM.Tenant;
            journal.Id = "new";
            journal.JournalNumber = "1";
            journal.CreateDate = TenantServerConfigration.GetCurrentDateTime(myLedgerTransactionTransferPM.Tenant);
            journal.AccountingDate = //theEntityPm.AccountingDate != null ? theEntityPm.AccountingDate.Value : 
                TenantServerConfigration.GetCurrentDateTime(myLedgerTransactionTransferPM.Tenant);
            journal.TypeCode = "0";
            journal.StatusCodeEnum = JournalStatusTypePM.StatusCodeEnum.Draft;
            journal.CreatedByUserId = AuthenticationUtil.ResolveUserId(myLedgerTransactionTransferPM.Tenant); ;
            journal.AccountingEntityCode = "6"; journal.AccountingEntityCode = ""; // Cheque Deposit//
            journal.AccountingEntityCode = "";//if  AccountingEntityCode = "6" crush while aRPaymentCheque.StatusCode = "6"; due aRPaymentCheque not found !!
            //journal.AccountingEntityId = theEntityPm.Id;
            journal.AccountingEntityReference = myReconcileExternalPageLinePM.Reference;
            journal.UpdateDate = TenantServerConfigration.GetCurrentDateTime(myLedgerTransactionTransferPM.Tenant);
            journal.UpdatedByUserId = AuthenticationUtil.ResolveUserId(myLedgerTransactionTransferPM.Tenant); ;
            journal.ApproveDate = TenantServerConfigration.GetCurrentDateTime(myLedgerTransactionTransferPM.Tenant);
            journal.ApprovedByUserId = AuthenticationUtil.ResolveUserId(myLedgerTransactionTransferPM.Tenant); ;
            journal.ChangeSetOp = ChangeSetOperation.Insert;


            JournalLinePM journalLineDebitTransfer = new JournalLinePM();
            journalLineDebitTransfer.Tenant = myLedgerTransactionTransferPM.Tenant;
            journalLineDebitTransfer.JournalId = journal.Id;
            journalLineDebitTransfer.Line = 1;
            journalLineDebitTransfer.ActionTypeCodeEnum =  MyJournalActionTypeEnum.Debit;
            //journalLine.ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit;
            journalLineDebitTransfer.DocumentDate = myReconcileExternalPageLinePM.ReferenceDate;
            journalLineDebitTransfer.DueDate= myReconcileExternalPageLinePM.ReferenceDate;
            journalLineDebitTransfer.AccountingDate = journal.AccountingDate;
            journalLineDebitTransfer.LocalAmount = myLedgerTransactionTransferPM.LocalAmountCredit;
            journalLineDebitTransfer.CurrencyId = myLedgerTransactionTransferPM.CurrencyId;
            journalLineDebitTransfer.ForeignAmount = myLedgerTransactionTransferPM.ForeignAmountCredit;
            //journalLine.ExchangeRate = (decimal)theEntityPm.InvoiceCurrencyExchangeRate;
            journalLineDebitTransfer.Reference1 = myReconcileExternalPageLinePM.Reference;

            journalLineDebitTransfer.DebitAccountId = myBankAccountPM.TransferGLAcccountId;
            journalLineDebitTransfer.CreditAccountId = myBankAccountPM.GLAccountId;
            //journalLine.CreditControlAccountId = glAccount == null ? "" : glAccount.ControlAccountId;
            journalLineDebitTransfer.ChangeSetOp = ChangeSetOperation.Insert;
            journal.JournalLines.Add(journalLineDebitTransfer);



            JournalLinePM journalLineCreditBankGLId = new JournalLinePM();
            journalLineCreditBankGLId.Tenant = myLedgerTransactionTransferPM.Tenant;
            journalLineCreditBankGLId.JournalId = journal.Id;
            journalLineCreditBankGLId.Line = 1;
            journalLineCreditBankGLId.ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit;

            journalLineCreditBankGLId.DocumentDate = myReconcileExternalPageLinePM.ReferenceDate;
            journalLineCreditBankGLId.DueDate = myReconcileExternalPageLinePM.ReferenceDate;
            journalLineCreditBankGLId.AccountingDate = journal.AccountingDate;
            journalLineCreditBankGLId.LocalAmount = myLedgerTransactionTransferPM.LocalAmountCredit;
            journalLineCreditBankGLId.CurrencyId = myLedgerTransactionTransferPM.CurrencyId;
            journalLineCreditBankGLId.ForeignAmount = myLedgerTransactionTransferPM.ForeignAmountCredit;
            journalLineCreditBankGLId.Reference1 = myReconcileExternalPageLinePM.Reference;
            journalLineCreditBankGLId.DebitAccountId = myBankAccountPM.TransferGLAcccountId;
            journalLineCreditBankGLId.CreditAccountId = myBankAccountPM.GLAccountId;

            journalLineCreditBankGLId.ChangeSetOp = ChangeSetOperation.Insert;
            journal.JournalLines.Add(journalLineCreditBankGLId);


            journal.JournalExternalReconciles.Add(new JournalExternalReconcilePM()
            {
                Tenant= journal.Tenant,
                 ChangeSetOp =  ChangeSetOperation.Insert,

                JournalId = journal.Id,
                Line = 1,
                LedgerTransactionId = myLedgerTransactionTransferPM.Id,
                ReconcileExternalPageLineId = myReconcileExternalPageLinePM.Id
            });

            return journal;
        }


        private string Validate(int tenant, LedgerTransactionPM myLedgerTransactionTransfer, ReconcileExternalPageLinePM myReconcileExternalPageLinePM, 
            BankAccountPM bankAccountFromTransfer, BankAccountPM bankAccountFromReconcileExternalPageLine)
        {
            var err = new List<string>();
            if (myLedgerTransactionTransfer.Tenant != tenant)
            {
                err.Add("myLedgerTransaction.Tenant!= tenant");
            }
            if (myReconcileExternalPageLinePM.Tenant != tenant)
            {
                err.Add("myReconcileExternalPageLinePM.Tenant!= tenant");
            }
            if (bankAccountFromTransfer == null)
            {
                err.Add("התנועה איננה בחשבון בנק לשלם ");
            }
            if (bankAccountFromReconcileExternalPageLine == null)
            {
                err.Add("הדף איננה בחשבון בנק לשלם ");
            }

            if (bankAccountFromTransfer!= null && bankAccountFromReconcileExternalPageLine!=null && 
                bankAccountFromReconcileExternalPageLine.Id!=bankAccountFromTransfer.Id)
            {
                err.Add("אין תאימות דף הבנק שייך לבנק אחר הנשלף מהתנועה");
            }
            if (myLedgerTransactionTransfer.LocalAmountCredit != myReconcileExternalPageLinePM.DebitAmount)
            {
                err.Add("סכום החובה בדף בנק חייב להיות זהה לסכום התנועה בכרטסת בזכות");
            }
            if (myReconcileExternalPageLinePM.DebitAmount <= 0){
                err.Add("סכום החובה בדף בנק חייב להיות גדול מאפס כנדרש בצק");
            }

            if (myLedgerTransactionTransfer.InProgressExternalReconcile)
            {
                err.Add("התנועה מסומנת בתהליך התאמה חצונית");
            }
            if (myLedgerTransactionTransfer.IsExternalReconcile)
            {
                err.Add("התנועה מסומנת שהותאמה כבר חיצונית");
            }


            if (myReconcileExternalPageLinePM.IsReconciled)
            {
                err.Add("השורה בדף מסומנת שהותאמה כבר חיצונית");
            }
            if (myReconcileExternalPageLinePM.InProgressExternalReconcile)
            {
                err.Add("השורה בדף מסומנת בתהליך התאמה חצונית");
            }
            return string.Join(Environment.NewLine, err.ToArray());

        }

        

        //private LedgerTransactionPM GetLedgerTransactionPM(int tenant, string ledgerTransactionId)
        //{
        //    var qs = new LedgerTransactionQueryService(tenant);
        //    var LedgerTransactionPM=qs.GetSingle(ledgerTransactionId,false,false);
        //    return LedgerTransactionPM;
        //}

        private void CheckNotInProgress(int tenant, string ledgerTransactionId, string reconcileExternalPageLineId)
        {
            throw new NotImplementedException();
        }

    }
}
