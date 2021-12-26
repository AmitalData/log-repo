using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile.AdjustDueBankFees
{
    class CreateJournal
    {
        private JournalPM _TheNewJournal;
        private IExternalReconcileDataProvider _ExternalReconcileDataProvider;
        public void MustInit(IExternalReconcileDataProvider externalReconcileDataProvider, JournalPM theNewJournal)
        {
            _ExternalReconcileDataProvider = externalReconcileDataProvider;
            this._TheNewJournal = theNewJournal;
        }
        public void CreateOnlyJournalLines(
            List<string> listOfpageLineIdsList, List<string> ledgerTransactionIdsOnlyCalcDiff,

            string adjustGLAccountId, string screenNotes, DateTime accountingDate
            )
        {
            var listOfpageLineList = _ExternalReconcileDataProvider
    .GetReconcileExternalPageLineList(_TheNewJournal.Tenant, listOfpageLineIdsList);

            var ledgerTransactionList = _ExternalReconcileDataProvider.GetLedgerTransactionList(ledgerTransactionIdsOnlyCalcDiff, this._TheNewJournal.Tenant);
            string accountingCurrencyId = this._ExternalReconcileDataProvider.GetaccountingCurrencyId(this._TheNewJournal.Tenant);

            var ReconcileExternalPageIds = listOfpageLineList.Select(r => r.ReconcileExternalPageId).ToList();
            var listOfpageList = _ExternalReconcileDataProvider.GetReconcileExternalPageList(this._TheNewJournal.Tenant, ReconcileExternalPageIds);
            var listOfAccId = listOfpageList.Select(r => r.GLAccountId).ToList();

            List<GLAccountList> ListOfGLAccountList = _ExternalReconcileDataProvider.GetListOfGLAccountList(this._TheNewJournal.Tenant, listOfAccId);
            var bankGLAccountListFromReconcileExternalPageIds = ListOfGLAccountList.First();//must have 





            decimal PageLineForeignAmount = listOfpageLineList.Sum(r => r.DebitAmount - r.CreditAmount);
            
            var ledgerForeignAmount = ledgerTransactionList.Sum(r => r.ForeignAmountDebit - r.ForeignAmountCredit);
            

            decimal foreignTotalAmount = (ledgerForeignAmount + PageLineForeignAmount);
            bool creditTheBank =(foreignTotalAmount > 0);


            decimal localTotalAmountConvertFromforeign = Convert2LocalAmountFast(_TheNewJournal.Tenant, foreignTotalAmount, bankGLAccountListFromReconcileExternalPageIds.CurrencyId, accountingCurrencyId,
                    accountingDate
                );
            int maxLine = _TheNewJournal.JournalLines.Max(r => r.Line);

            var firstJL =   
                GetFirstJournalLineSum(_TheNewJournal.Tenant, adjustGLAccountId, listOfpageLineList, bankGLAccountListFromReconcileExternalPageIds, screenNotes, foreignTotalAmount, localTotalAmountConvertFromforeign, creditTheBank);
            firstJL.Line = maxLine++;

            var LstJL =
                GetFirstJournalLineSum(_TheNewJournal.Tenant, adjustGLAccountId, listOfpageLineList, bankGLAccountListFromReconcileExternalPageIds, screenNotes, foreignTotalAmount, localTotalAmountConvertFromforeign, creditTheBank);
            LstJL.Line = maxLine++;
            LstJL.ActionTypeCodeEnum = creditTheBank ? JournalActionTypeEnum.Debit : JournalActionTypeEnum.Credit;

            
            _TheNewJournal.JournalLines.Add(firstJL);
            _TheNewJournal.JournalLines.Add(LstJL);


            






        }

        private decimal Convert2LocalAmountFast(int tenant, decimal foreignAmount, string currencyId, string accountingCurrencyId, DateTime ReferenceDate)
        {
            //decimal foreignAmount = r.DebitAmount != 0 ? r.DebitAmount : r.CreditAmount;
            if (currencyId == accountingCurrencyId)
            {
                return foreignAmount;
            }
            var rate = _ExternalReconcileDataProvider.GetLastRateByValueDate(tenant, currencyId, accountingCurrencyId, ReferenceDate);
            rate = rate ?? new Logitude.BL.DataContracts.LastRate() { Rate = 1 };
            double? itemCurrencyRateRounded = MethodHelper.Round(rate.Rate, 5);
            itemCurrencyRateRounded = itemCurrencyRateRounded ?? 1;
            decimal local = foreignAmount * (decimal)itemCurrencyRateRounded.GetValueOrDefault();
            return local;
        }
        private JournalLinePM GetFirstJournalLineSum(int tenant, string adjustGLAccountId, List<ReconcileExternalPageLineList> listOfpageLineList, GLAccountList bankGLAccountList, string screenNotes, decimal ForeignAmount, decimal LocalAmount, bool creditTheBank)
        {
            /*
             *צד הלדג'ר (GLACCOUNT בצד) כל תנועה  בקרדיט
מכפילים במינוס 1
מחברים את כל התנועות (גם הדביט וגם הקרדיט
+ 
צד דפי הבנק כל תנועה  בקרדיט
מכפיל ב מינוס 1
מחבר את כל התונעות_גם הדביט וגם הקרדיט) 

 


אם התוצאה קטנה מאפס יש להכפילה במינוס אחד ולחייב את צד הלדג'ר
אחרת
נזכה את צד הלדג'ר בתוצאה
             */
            return
new JournalLinePM()
{
    Tenant = tenant,
    JournalId = _TheNewJournal.Id,
    Line = 1,


    DocumentDate = listOfpageLineList.First().ReferenceDate,
    DueDate = listOfpageLineList.First().ReferenceDate,
    AccountingDate = _TheNewJournal.AccountingDate,



    CurrencyId = bankGLAccountList.CurrencyId,



    ///if r.DebitAmount != 0 then credit else debit 
    ActionTypeCodeEnum = creditTheBank ? JournalActionTypeEnum.Credit : JournalActionTypeEnum.Debit,
    CreditAccountId = creditTheBank ? bankGLAccountList.Id : adjustGLAccountId,
    DebitAccountId = creditTheBank ? adjustGLAccountId : bankGLAccountList.Id,
    LocalAmount = creditTheBank ? (LocalAmount) : -1 * (LocalAmount),
    ForeignAmount = creditTheBank ? (ForeignAmount) : -1 * (ForeignAmount),
    Notes = screenNotes + Environment.NewLine + listOfpageLineList.First().Notes,
    Reference1 = listOfpageLineList.First().Reference,
    ChangeSetOp = ChangeSetOperation.Insert
};
        }

    }
}
