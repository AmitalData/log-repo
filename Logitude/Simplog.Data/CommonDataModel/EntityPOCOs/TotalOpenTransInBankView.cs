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
        public int Tenant { get; set; }
        public int TotalLedgerTransactionsCount { get; set; }
        public int TotalReconcileExternalPageLinesCount { get; set; }
    }
}
