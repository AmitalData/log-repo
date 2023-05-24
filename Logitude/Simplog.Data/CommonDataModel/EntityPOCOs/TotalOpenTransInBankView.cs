using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class TotalOpenTransInBankView
    {
        public string Id { get; set; }
        [Key]
        public string GLAccountId { get; set; }
        public int Tenant { get; set; }
        public int LedgerTransactionsCount { get; set; }
        public int ReconcileExternalPageLinesCount { get; set; }
    }
}
