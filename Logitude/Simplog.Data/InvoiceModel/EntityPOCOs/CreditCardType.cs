using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class CreditCardType
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public int Tenant { get; set; }
        public bool InActive { get; set; }

        public string BankAccountId { get; set; }

        //public List<ARPayment> ARPayments { get; set; }
        //public List<APPayment> APPayments { get; set; }
    }
}
