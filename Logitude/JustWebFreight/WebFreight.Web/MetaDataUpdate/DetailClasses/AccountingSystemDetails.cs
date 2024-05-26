using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class AccountingSystemDetails
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public bool IsExternalCodesFromTable { get; set; }
        public bool IsExternalCodesFromAPI { get; set; }
        public bool IsSingleTaxPerInvoice { get; set; }
        public bool IsSingleCurrencyAccount { get; set; }
        public bool AllowManuallyDueDate { get; set; }
        public bool IsJournalMode { get; set; }
        public bool IsTaxItemManaged { get; set; }
        public bool AllowMinusInvoiceLines { get; set; }
        public bool ShowDownloadScreen { get; set; }
        public bool AllowPositiveAmountsInTheCreditNote { get; set; }
        public bool AllowARInvoicesTransfer { get; set; }
    }
}