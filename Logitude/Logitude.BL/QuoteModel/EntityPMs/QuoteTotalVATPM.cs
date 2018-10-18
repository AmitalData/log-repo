using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    public class QuoteTotalVATPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QuoteId { get; set; }
        public string VatTypeId { get; set; }
        public double? VatPercent { get; set; }
        public string VatTypeName { get; set; }
        public string ExternalVATCard { get; set; }
        public string ExternalTAXItemId { get; set; }
        public string VatTypeCell { get; set; }
        public double? QuoteCurrencyVatableAmount { get; set; }
        public double? LocalCurrencyVatableAmount { get; set; }
        public double? ProfitCurrencyVatableAmount { get; set; }
        public double? QuoteCurrencyVATAmount { get; set; }
        public double? LocalCurrencyVATAmount { get; set; }
        public double? ProfitCurrencyVATAmount { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
