using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class BankAccountLite
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
        public string BranchAddress { get; set; }
        public bool Inactive { get; set; }
        public string SearchFields { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }
        public string VatNumber { get; set; }
    }
}
