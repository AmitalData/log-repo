using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.InterestService.HelperClasses
{
    public class InterestReportArguments
    {

        public bool AllSelected { get; set; }
        public List<string> SelectedIds { get; set; }
        public List<SelectItem> SelectedItems { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<string> ExcludedIds { get; set; }
        public int Tenant { get; set; }
        public string Email { get; set; }
        public bool ShowPrintedInvoice { get; set; }
        public bool AttachReportWithEachInvoice { get; set; }
        public DateTime InvoiceDate { get; set; }
        public bool CloseWithoutInvoice{get;set;}

    }

    public class SelectItem
    {
        public string Id { get; set; }
        public string SecurityId { get; set; }
        public string TempId { get; set; }
    }
 
}
