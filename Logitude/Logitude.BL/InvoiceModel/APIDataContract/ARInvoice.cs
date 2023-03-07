using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
    public partial class ARInvoice
    {
        public string ExistingExternalJournal { get; set; }

        public bool? DoNotCreateJournal { get; set; }
    }
}
