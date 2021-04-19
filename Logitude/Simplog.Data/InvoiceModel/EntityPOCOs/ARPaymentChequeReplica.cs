using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
   public class ARPaymentChequeReplica
    {
        [Key]
        public string Id { get; set; }
       
        public int Tenant { get; set; }
        
        public string SearchFields { get; set; }
       
        public string PaymentId { get; set; }
       
       
        public int LineNumber { get; set; }
       
        public string ChequeNumber { get; set; }
       
        public DateTime ValueDate { get; set; }
       
        public string CurrencyId { get; set; }
       
       
       
        public decimal LocalAmount { get; set; }
       
        public decimal ForeignAmount { get; set; }
       
        public string BankId { get; set; }
       
       
       
        public string BankBranch { get; set; }
       
        public string BankAccount { get; set; }
       
        public string StatusCode { get; set; }
       
        public decimal? ExchangeRate { get; set; }






    }
}
