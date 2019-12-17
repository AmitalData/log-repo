using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.InterestTrans
{
    public class RegularJournalInterestTransactionMapping
    {
     
        public  void CreatelInterestTransactions(JournalPM regularJournal)
        {
            if (regularJournal.TypeCode != JournalTypeValues.Regular)//0	Regular	רגיל	0,רגיל,False,Regular,	0
            {
                return;
            }
            if (regularJournal.AccountingEntityCode != "1")//1	פקודת יומן	Journal
            {
                return;
            }
            var repoInterestTransactionFastFetch = new InterestTransactionRepository(regularJournal.Tenant);
            var AlreadyExist =repoInterestTransactionFastFetch.AlreadyExist("3",//3 - “Journal”
                regularJournal.Id,
                regularJournal.Tenant
                );
            if (AlreadyExist)
            {
                throw new Exception($"AlreadyExist InterestTransaction 4 regularJournal {regularJournal.Id}  - journal repushed ?!?!");
            }

            var CreditAccountIdS = regularJournal.JournalLines
                .Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit || r.ActionTypeCodeEnum == MyJournalActionTypeEnum.DebitAndCredit /*|| r.ActionTypeCodeEnum == MyJournalActionTypeEnum.DebitCreditAndVatdeduction*/)
                .Select(r => r.CreditAccountId)
                .Distinct()
                .ToList();

            var DebitAccountIdS = regularJournal.JournalLines
                .Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit || r.ActionTypeCodeEnum == MyJournalActionTypeEnum.DebitAndCredit /*|| r.ActionTypeCodeEnum == MyJournalActionTypeEnum.DebitCreditAndVatdeduction*/)
                .Select(r => r.DebitAccountId)
                .Distinct()
                .ToList();
            var myPartnerIds = DebitAccountIdS.Union(CreditAccountIdS);
            var repoGLAccountFastFetch = new GLAccountRepository(regularJournal.Tenant);
            var myPartners = repoGLAccountFastFetch.GetByGLAccountsIdList(myPartnerIds.ToList(), regularJournal.Tenant);
            var ClientIds = myPartners
                .Where(r => r.AccountTypeCode == GLAccountTypeEnum.Client.ToIntString())
                .Select(r => r.Id);


            var creditLines =
            regularJournal.JournalLines
                .Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit || r.ActionTypeCodeEnum == MyJournalActionTypeEnum.DebitAndCredit /*|| r.ActionTypeCodeEnum == MyJournalActionTypeEnum.DebitCreditAndVatdeduction*/)
                .Where(r => ClientIds.Contains(r.CreditAccountId))
                .Select(r =>
             new InterestTransactionPM()
             {
                 GLAccountId = r.CreditAccountId,
                 InterestEntityTypeCode = "3",//3 - “Journal”
                 EntityId = regularJournal.Id,
                 OriginalEntityLineNumber = r.Line,
                 LocalAmount = (decimal)r.LocalAmount,
                 ForeignAmount = (decimal?)r.ForeignAmount,
                 CurrencyId = r.CurrencyId,
                 InterestValueDate = (DateTime)r.DueDate,
                 Tenant = r.Tenant,
                 ChangeSetOp = ChangeSetOperation.Insert
             }
             ).ToList();


            var debitLines =
            regularJournal.JournalLines
                .Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit || r.ActionTypeCodeEnum == MyJournalActionTypeEnum.DebitAndCredit /*|| r.ActionTypeCodeEnum == MyJournalActionTypeEnum.DebitCreditAndVatdeduction*/)
                .Where(r => ClientIds.Contains(r.DebitAccountId))
                .Select(r =>
             new InterestTransactionPM()
             {
                 GLAccountId = r.DebitAccountId,
                 InterestEntityTypeCode = "3",//3 - “Journal”
                 EntityId = regularJournal.Id,
                 OriginalEntityLineNumber = r.Line,
                 LocalAmount = (decimal)r.LocalAmount,
                 ForeignAmount = (decimal?)r.ForeignAmount,
                 CurrencyId = r.CurrencyId,
                 InterestValueDate = (DateTime)r.DueDate,
                 Tenant = r.Tenant,
                 ChangeSetOp = ChangeSetOperation.Insert
             }
             ).ToList();

            var allInterestTransactions = creditLines.Concat(debitLines);
            IAccountingContext context = AccountingContext.GetContext(regularJournal.Tenant);
            var service = new InterestTransactionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), regularJournal.Tenant);
            service.UpdateMulti(allInterestTransactions.ToList(), new List<InterestTransactionPM>(), regularJournal, true);
        }
    }
}
