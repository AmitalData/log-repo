using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class ARInvoiceStatus
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        ////[Include]
        ////[Association("InvoiceInvoiceStatus", "Code", "ARInvoiceStatusCode")]
        //public List<ARInvoice> ARInvoices { get; set; }


    }
}