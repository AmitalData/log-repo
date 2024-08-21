using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.EntityLists
{
    public class LedgerTransactionJournalLineLT
    {
        // LedgerTransaction table
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string AccountId { get; set; }
        public string JournalId { get; set; }
        public int JournalLineNumber { get; set; }
        public decimal LocalAmountDebit { get; set; }
        public decimal LocalAmountCredit { get; set; }
        public decimal ForeignAmountDebit { get; set; }
        public decimal ForeignAmountCredit { get; set; }
        public string CurrencyId { get; set; }
        public decimal OpenAmount { get; set; }
        public bool InReconcileProgress { get; set; }
        public bool IsReconciled { get; set; }
        public string OpenAmountCurrencyId { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
        public string RecoNumber { get; set; }
        public decimal? PaymentReconciledAmount { get; set; }

        // Journal table
        public string SourceTypeCode { get; set; }
        public string SourceId { get; set; }
        public string SourceNumber { get; set; }
        public string OriginalJournalId { get; set; }

        // JournalLine table
        public string ActionCode { get; set; }



    }
}
