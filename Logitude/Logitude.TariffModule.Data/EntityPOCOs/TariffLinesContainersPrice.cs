using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.TariffModule.Data.EntityPOCOs
{
   
    public class TariffLinesContainersPrice
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("Tariff")]
        [Column("TariffId")]
	    public string TariffId { get; set; }
	      
        public virtual Tariff Tariff { get; set; }
        [ForeignKey("TariffLine")]
        [Column("TariffLineId")]
	    public string TariffLineId { get; set; }
	      
        public virtual TariffLine TariffLine { get; set; }
        [ForeignKey("Surcharge")]
        [Column("SurchargeId")]
	    public string SurchargeId { get; set; }
	      
        public virtual ChargesType Surcharge { get; set; }
        [Column("Price1")]
	    public decimal? Price1 { get; set; }
        [Column("Price2")]
	    public decimal? Price2 { get; set; }
        [Column("Price3")]
	    public decimal? Price3 { get; set; }
        [Column("Price4")]
	    public decimal? Price4 { get; set; }
        [Column("Price5")]
	    public decimal? Price5 { get; set; }
        [Column("CostPrice")]
	    public decimal? CostPrice { get; set; }
        [ForeignKey("Currency")]
        [Column("CurrencyId")]
	    public string CurrencyId { get; set; }
	      
        public virtual Currency Currency { get; set; }
    }
}
	 