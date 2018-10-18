using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class QuoteTotalVAT
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QuoteId { get; set; }
        public string VatTypeId { get; set; }
        public double? VatPercent { get; set; }
        public string ExternalVATCard { get; set; }
        public string ExternalTAXItemId { get; set; }

        public double? QuoteCurrencyVATAmount { get; set; }
        public double? QuoteCurrencyVatableAmount { get; set; }
        public double? LocalCurrencyVATAmount { get; set; }
        public double? LocalCurrencyVatableAmount { get; set; }
        public double? ProfitCurrencyVATAmount { get; set; }
        public double? ProfitCurrencyVatableAmount { get; set; }

        [ForeignKey("QuoteId")]
        public virtual Quote Quote { get; set; }

        [ForeignKey("VatTypeId")]
        public virtual VatType VatType { get; set; }
    }
}
