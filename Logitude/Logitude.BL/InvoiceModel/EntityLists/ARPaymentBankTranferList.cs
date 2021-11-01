using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityLists
{
   public class ARPaymentBankTranferList
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }

        public string SearchFields { get; set; }

        public string PaymentId { get; set; }


        public int LineNumber { get; set; }
        public string PaymentRef { get; set; }
        public DateTime ValueDate { get; set; }
        public string BankAccountId { get; set; }
        public string CurrencyId { get; set; }

        public decimal LocalAmount { get; set; }
        public decimal ForeignAmount { get; set; }
        public decimal ExchageRate { get; set; }
    }
}
