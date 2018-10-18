using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class BankAccountLiteList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string LocalName { get; set; }
        public string EnglishName { get; set; }
        public string BranchNumber { get; set; }
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }
        public string SwiftCode { get; set; }
        public string BankCode { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string BranchAddress { get; set; }
        public bool Inactive { get; set; }
        public string SearchFields { get; set; }
        public string VatNumber { get; set; }
    }
}
