using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.DataContracts
{
    public class ARInvoiceSATStatus
    {
        [Key]
        public string ARInvoiceId { get; set; }
        public string ARInvoiceNumber { get; set; }
        public string BillTo { get; set; }
        public bool IsSATValid { get; set; }
        public string SATStatusCode { get; set; }
        public string SATStatusName { get; set; }
        public string SATError { get; set; }


    }
}
