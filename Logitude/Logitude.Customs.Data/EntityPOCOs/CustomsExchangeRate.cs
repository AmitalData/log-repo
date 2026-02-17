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

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class CustomsExchangeRate
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("CurrencyType")]
        [Column("CurrencyTypeCode")]
	    public string CurrencyTypeCode { get; set; }
	      
        public virtual CurrencyType CurrencyType { get; set; }
        [Column("ExchangeRate")]
	    public decimal? ExchangeRate { get; set; }
        [Column("RateDate")]
	    public DateTime? RateDate { get; set; }
        [Column("UpdateDateTime")]
	    public DateTime? UpdateDateTime { get; set; }
    }
}
	 