using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.APIDataContract
{
    public class ARInvoiceJournalLT
    {
        public string Id { get; set; }
        public string JournalId { get; set; }
        public bool IsLedgerCreated { get; set; }

    }
}
