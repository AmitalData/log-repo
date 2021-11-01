using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
   public class ARPaymentBankTranfer
    {
        [Key]
        public string Id { get; set; }
     
        public int Tenant { get; set; }
     
        public string SearchFields { get; set; }
      
        public string PaymentId { get; set; }

        public virtual ARPayment Payment { get; set; }
      
        public int LineNumber { get; set; }    
        public string PaymentRef { get; set; }      
        public DateTime ValueDate { get; set; }
        public string BankAccountId { get; set; }
        public string CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }     
        public decimal LocalAmount { get; set; }     
        public decimal ForeignAmount { get; set; }      
        public decimal ExchageRate { get; set; }
    }
}
