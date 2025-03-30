using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
   
    public class CurrencyRate
    {
        [Key]
        [Column("Id")]
        public string Id { get; set; }

        [Column("Tenant")]
	    public int Tenant { get; set; }

        [ForeignKey("ExchangeRate")]
        [Column("ExchangeRateId")]
	    public string ExchangeRateId { get; set; }
        public virtual RatesTable ExchangeRate { get; set; }

        [ForeignKey("AdditionalCurrencyRate")]
        [Column("AdditionalCurrencyRateId")]
	    public string AdditionalCurrencyRateId { get; set; }
        public virtual AdditionalCurrencyRate AdditionalCurrencyRate { get; set; }

        [Column("Rate")]
	    public double Rate { get; set; }
    }
}
	 