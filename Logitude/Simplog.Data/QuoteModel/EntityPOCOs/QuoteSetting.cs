using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class QuoteSetting
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool CopyShipper { get; set; }
        public bool CopyConsignee { get; set; }
        public bool CopyMainCarriage { get; set; }
        public bool CopyPickup { get; set; }
        public bool CopyDelivery { get; set; }
        public bool CopyChargesTypes { get; set; }
        public bool CopyChargesCost { get; set; }
        public bool CopyChargesSale { get; set; }
        public bool EditMainCarriage { get; set; }
        public bool CopyAgent{ get; set; }
        public bool CopyNotify { get; set; }
        public bool CopyExchangeRates { get; set; }
        public bool IsSaleAsCostCurrency { get; set; }
        public int AutomaticallyCloseDays { get; set; }
        public bool IsMultiCurrency { get; set; }
        public int? QuoteExpirationDays { get; set; }
    }
}
