using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CardExternalCodeByCurrencyList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CardId { get; set; }
        public string CurrencyId { get; set; }
        public string ExternalRecievableTableId { get; set; }
        public string ExternalPayableTableId { get; set; }
        public string CurrencyCode { get; set; }
        public string ExternalTableName { get; set; }
        public string ExternalTableCode { get; set; }
    }
}
