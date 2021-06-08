using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
   public class GLAccountChequesTransactionsRetreivingService
    {
        int tenant;
        IAccountingContext accountingContext;
        public GLAccountChequesTransactionsRetreivingService(int Tenant, IAccountingContext context)
        {
            tenant = Tenant;
            this.accountingContext = context;
        }

        public List<LedgerTransactionPM> GetAccountChequesTransactions(string accountId)
        {
            return (from transaction in accountingContext.LedgerTransactions
                    join journal in accountingContext.Journals on
                    transaction.JournalId equals journal.Id
                    join arpaymentcheque in accountingContext.ARPaymentCheques on journal.AccountingEntityId equals arpaymentcheque.PaymentId                    
                   join arpaymentchequeStatus in accountingContext.ARPaymentChequeStatuses on arpaymentcheque.StatusCode equals arpaymentchequeStatus.Code                   
                    where transaction.Tenant == tenant && journal.AccountingEntityId == "3" && transaction.AccountId == accountId
                    select new LedgerTransactionPM()
                    {
                        PaymentValueDate = arpaymentcheque.ValueDate,
                        PaymentChequeStatus = arpaymentchequeStatus.LocalName,
                        Source = journal.AccountingEntityReference,
                        LocalAmountCredit = transaction.LocalAmountCredit,
                        ForeignAmountCredit = transaction.ForeignAmountCredit,
                        Reference1 = transaction.Reference1,
                        Reference2 = transaction.Reference2,
                        Reference3 = transaction.Reference3,
                        JournalNumber = journal.JournalNumber,
                        Notes = transaction.Notes

                    }).ToList();

                  
        }
       
    }
}
