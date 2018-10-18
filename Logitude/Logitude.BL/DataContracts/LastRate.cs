using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.DataContracts
{
    public class LastRate
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ForeignCurrencyId { get; set; }        
        public string ForeignCurrencyCode { get; set; }
        public string ForeignCurrencyName { get; set; }
        public string BaseCurrencyId { get; set; }
        public string BaseCurrencyCode { get; set; }       
        public DateTime? ValueDate { get; set; }        
        public double? Rate { get; set; }
        public DateTime? LogDateTime { get; set; }
        public int? HistoryCount { get; set; }
    }
}
