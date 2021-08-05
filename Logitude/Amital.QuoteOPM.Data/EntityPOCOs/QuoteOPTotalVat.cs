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

namespace Amital.QuoteOPM.Data.EntityPOCOs
{
   
    public class QuoteOPTotalVAT
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("QuoteOP")]
        [Column("QuoteOPId")]
	    public string QuoteOPId { get; set; }
	      
        public virtual QuoteOP QuoteOP { get; set; }
        [Column("QuoteCurrencyVATAmount")]
	    public double? QuoteCurrencyVATAmount { get; set; }
        [Column("QuoteCurrencyVatableAmount")]
	    public double? QuoteCurrencyVatableAmount { get; set; }
        [Column("LocalCurrencyVATAmount")]
	    public double? LocalCurrencyVATAmount { get; set; }
        [Column("LocalCurrencyVatableAmount")]
	    public double? LocalCurrencyVatableAmount { get; set; }
        [Column("ProfitCurrencyVATAmount")]
	    public double? ProfitCurrencyVATAmount { get; set; }
        [Column("ProfitCurrencyVatableAmount")]
	    public double? ProfitCurrencyVatableAmount { get; set; }
        [Column("VatPercent")]
	    public double? VatPercent { get; set; }
        [Column("ExternalVATCard")]
	    public string ExternalVATCard { get; set; }
        [Column("ExternalTAXItemId")]
	    public string ExternalTAXItemId { get; set; }
        [Column("VatOPTypeId")]
	    public string VatOPTypeId { get; set; }
    }
}
	 