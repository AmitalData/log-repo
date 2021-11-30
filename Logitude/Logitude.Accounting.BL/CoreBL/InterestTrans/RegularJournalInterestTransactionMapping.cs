using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using AccountingEntityValues = Logitude.Accounting.BL.CloseTables.AccountingEntityValues;

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
            if (regularJournal.AccountingEntityCode != AccountingEntityValues.Journal 
                && regularJournal.AccountingEntityCode != AccountingEntityValues.Adjustment)
            {
                return;
            }
            string interestEntityType = GetInterestEntityType(regularJournal);
            if (interestEntityType != null) {
                var repoInterestTransactionFastFetch = new InterestTransactionRepository(regularJournal.Tenant);
                var AlreadyExist = repoInterestTransactionFastFetch.AlreadyExist(interestEntityType,//3 - “Journal”
                    regularJournal.Id,
                    regularJournal.Tenant
                    );
                if (AlreadyExist)
                {
                    throw new Exception($"AlreadyExist InterestTransaction 4 regularJournal {regularJournal.Id}  - journal repushed ?!?!");
                }
                var allInterestTransactions = new List<InterestTransactionPM>();
                if (regularJournal.ExternalSystem == "AMITAL" && !string.IsNullOrWhiteSpace(regularJournal.ExternalNo))//Task 62801: ריבית - מיפוי תנועות - למפות רק פקודות שאינן חיצוניות - R5
                {
                    var pmFullAccountingSetting = FullAccountingSettingQueryService.Get(regularJournal.Tenant);
                    if (pmFullAccountingSetting == null && pmFullAccountingSetting.AccountingActivationDate.HasValue)
                    {
                        allInterestTransactions = GetInterestTransactionListExternal(regularJournal, pmFullAccountingSetting.AccountingActivationDate.GetValueOrDefault(), interestEntityType);

                    }

                }
                else
                {
                    allInterestTransactions = GetInterestTransactionListRegular(regularJournal, interestEntityType);
                }
                if (allInterestTransactions.Count == 0)
                {
                    return;
                }
                IAccountingContext context = AccountingContext.GetContext(regularJournal.Tenant);
                var service = new InterestTransactionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), regularJournal.Tenant);
                service.UpdateMulti(allInterestTransactions.ToList(), new List<InterestTransactionPM>(), regularJournal, true);
            }
        }

        private string GetInterestEntityType(JournalPM journal)
        {
            if (journal.AccountingEntityCode == AccountingEntityValues.Journal)
            {
                return InterestEntityTypes.Journal;
            }
            else if (journal.AccountingEntityCode == AccountingEntityValues.Adjustment) {
                return InterestEntityTypes.OpenBalance;
            }
            return null;
        }

        private static List<InterestTransactionPM> GetInterestTransactionListRegular(JournalPM regularJournal, string interestEntityType)
        {
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
                 InterestEntityTypeCode = interestEntityType,
                 EntityId = regularJournal.Id,
                 OriginalEntityLineNumber = r.Line,
                 LocalAmount = (decimal)r.LocalAmount * -1,//Credit = 1,
                 ForeignAmount = (decimal?)r.ForeignAmount * -1,//Credit = 1,
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
                 InterestEntityTypeCode = interestEntityType,
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

            var allInterestTransactions = creditLines.Concat(debitLines).ToList();
            return allInterestTransactions;
        }


        private static List<InterestTransactionPM> GetInterestTransactionListExternal(JournalPM externalJournal,DateTime AccountingActivationDate, string interestEntityType)
        {

            var creditJournalLines = externalJournal.JournalLines
                .Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit || r.ActionTypeCodeEnum == MyJournalActionTypeEnum.DebitAndCredit /*|| r.ActionTypeCodeEnum == MyJournalActionTypeEnum.DebitCreditAndVatdeduction*/)
                .Where(r => r.DueDate >= AccountingActivationDate);
            var CreditAccountIdS = creditJournalLines
                .Select(r => r.CreditAccountId)
                .Distinct()
                .ToList();

          
            var myPartnerIds = CreditAccountIdS;
            var repoGLAccountFastFetch = new GLAccountRepository(externalJournal.Tenant);
            var myPartners = repoGLAccountFastFetch.GetByGLAccountsIdList(myPartnerIds.ToList(), externalJournal.Tenant);
            var ClientIds = myPartners
                .Where(r => r.AccountTypeCode == GLAccountTypeEnum.Client.ToIntString())
                .Select(r => r.Id);


            var creditLines =
            creditJournalLines
                .Where(r => ClientIds.Contains(r.CreditAccountId))
                .Select(r =>
             new InterestTransactionPM()
             {
                 GLAccountId = r.CreditAccountId,
                 InterestEntityTypeCode = interestEntityType,
                 EntityId = externalJournal.Id,
                 OriginalEntityLineNumber = r.Line,
                 LocalAmount = (decimal)r.LocalAmount * -1,//Credit = 1,
                 ForeignAmount = (decimal?)r.ForeignAmount * -1,//Credit = 1,
                 CurrencyId = r.CurrencyId,
                 InterestValueDate = (DateTime)r.DueDate,
                 Tenant = r.Tenant,
                 ChangeSetOp = ChangeSetOperation.Insert
             }
             ).ToList();


         

            var allInterestTransactions = creditLines.ToList();
            return allInterestTransactions;
        }
    }
}
