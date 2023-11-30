using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class RatesTableList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ForeignCurrencyId { get; set; }
        public string ForeignCurrencyCode { get; set; }
        public string ForeignCurrencyName { get; set; }
        public string BaseCurrencyId { get; set; }
        public double? Rate { get; set; }
        public DateTime? ValueDate { get; set; }
        public DateTime? LogDateTime { get; set; }
        public string UpdatedByUserId { get; set; }
        public string UpdatedByUserName { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}