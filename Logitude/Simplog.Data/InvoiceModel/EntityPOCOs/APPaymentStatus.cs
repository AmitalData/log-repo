using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class APPaymentStatus
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        //public List<APPayment> APPayments { get; set; }
    }
}