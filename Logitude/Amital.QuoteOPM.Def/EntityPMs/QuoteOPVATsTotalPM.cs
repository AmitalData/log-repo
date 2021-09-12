using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.Def.EntityPMs
{
    public partial class QuoteOPVATsTotalPM
    {
        public string Id { get; set; }
        public string VatTypeId { get; set; }
        public string VatTypeName { get; set; }
        public double? VatPercentage { get; set; }
        public double? AmountInSaleCurrency { get; set; }
        public double? AmountInLocalCurrency { get; set; }
    }
}
