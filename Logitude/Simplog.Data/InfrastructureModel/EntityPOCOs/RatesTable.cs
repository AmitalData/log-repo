using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class RatesTable
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string BaseCurrencyId { get; set; }
        public string ForeignCurrencyId { get; set; }
        public double? Rate { get; set; }
        public int? Unit { get; set; }
        public DateTime? ValueDate { get; set; }
        public DateTime LogDateTime { get; set; }


        //[ExternalReference]
        //[Association("BaseRatesTableCurrency", "BaseCurrencyId", "Id", IsForeignKey = true)]
        [ForeignKey("BaseCurrencyId")]
        public virtual Currency BaseCurrency { get; set; }
        //[ExternalReference]
        //[Association("ForeignRatesTableCurrency", "ForeignCurrencyId", "Id", IsForeignKey = true)]
        [ForeignKey("ForeignCurrencyId")]
        public virtual Currency ForeignCurrency { get; set; }

        //public List<ShipmentReceivable> ShipmentReceivables { get; set; }
        public string UpdatedByUserId { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }


}