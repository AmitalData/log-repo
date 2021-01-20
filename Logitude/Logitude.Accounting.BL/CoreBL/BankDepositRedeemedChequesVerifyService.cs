using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class BankDepositRedeemedChequesVerifyService
    {
        int tenant;
        public BankDepositRedeemedChequesVerifyService(int tenant)
        {
            this.tenant = tenant;
        }

        public List<ARPaymentChequePM> GetNotRedeemedReconciledCheques()
        {
            var context = AccountingContext.GetContext(tenant);

            //var externallyReconciledChequeDepositTransactions
            //    = from ledger in context.LedgerTransactions
            //      join journal in context.Journals on ledger.JournalId equals journal.Id
            //      join cheque in context.ARPaymentCheques on journal.AccountingEntityId equals cheque.Id
            //      where ledger.IsExternalReconcile == true
            //             && journal.AccountingEntityCode == "6"
            //      select ledger;

            List<string> depositIds
                                = (from ledger in context.LedgerTransactions
                                  join journal in context.Journals on ledger.JournalId equals journal.Id
                                  join cheque in context.ARPaymentCheques on journal.AccountingEntityId equals cheque.Id
                                  where ledger.IsExternalReconcile == true
                                         && journal.AccountingEntityCode == "6"
                                  select journal.AccountingEntityId).ToList();

            List<string> paymentChequeIds = (from a in context.BankDepositLines
                                             where depositIds.Contains(a.DepositId)  && a.Tenant == tenant
                                             select a.ARPaymentChequeId).ToList();

            List<ARPaymentChequePM> paymentChequesNotRedeemed = (from cheque in context.ARPaymentCheques
                                                      where paymentChequeIds.Contains(cheque.Id) && cheque.Tenant == tenant
                                                            && cheque.StatusCode != "6" 
                                                      select new ARPaymentChequePM()
                                                      {
                                                          Id = cheque.Id,
                                                          Tenant = cheque.Tenant,
                                                          CurrencyCode = cheque.Currency.Code,
                                                          SearchFields = cheque.SearchFields,
                                                          LineNumber = cheque.LineNumber,
                                                          ChequeNumber = cheque.ChequeNumber,
                                                          ValueDate = cheque.ValueDate,
                                                          LocalAmount = cheque.LocalAmount,
                                                          ForeignAmount = cheque.ForeignAmount,
                                                          BankId = cheque.BankId,
                                                          BankBranch = cheque.BankBranch,
                                                          BankAccount = cheque.BankAccount,
                                                          StatusName = cheque.ARPaymentChequeStatus != null ? cheque.ARPaymentChequeStatus.EnglishName : "",
                                                          PaymentId = cheque.PaymentId,
                                                          ExchangeRate = cheque.ExchangeRate,
                                                          StatusCode = cheque.StatusCode,
                                                          CurrencyId = cheque.CurrencyId,
                                                      }).ToList();


            return paymentChequesNotRedeemed;






        }



    }
}
