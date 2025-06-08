using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.EntityPOCOs
{
    public class LedgerTransactionsDeductionView
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string AccountId { get; set; }          // whAccount or bankAccount
        public string OppositeAccountId { get; set; }  // Supplier
        public string JournalId { get; set; }
        public int JournalLineNumber { get; set; }
        public decimal? LocalAmountDebit { get; set; }
        public decimal? LocalAmountCredit { get; set; }
        public string Reference1 { get; set; }
        public DateTime AccountingDate { get; set; }
        public string ChartOfAccountsTypeCode { get; set; }
    }
}
