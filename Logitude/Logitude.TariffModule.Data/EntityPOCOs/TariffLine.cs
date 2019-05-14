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
   
    public class TariffLine
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("StartDate")]
	    public DateTime? StartDate { get; set; }
        [Column("ExpirationDate")]
	    public DateTime? ExpirationDate { get; set; }
        [Column("TariffId")]
	    public string TariffId { get; set; }
        [Column("Version")]
	    public int Version { get; set; }
        [Column("MinPrice")]
	    public decimal? MinPrice { get; set; }
        [Column("Step1Price")]
	    public decimal? Step1Price { get; set; }
        [Column("Step2Price")]
	    public decimal? Step2Price { get; set; }
        [Column("Step3Price")]
	    public decimal? Step3Price { get; set; }
        [Column("Step4Price")]
	    public decimal? Step4Price { get; set; }
        [Column("Step5Price")]
	    public decimal? Step5Price { get; set; }
        [Column("Step6Price")]
	    public decimal? Step6Price { get; set; }
        [Column("Step7Price")]
	    public decimal? Step7Price { get; set; }
        [Column("Step8Price")]
	    public decimal? Step8Price { get; set; }
        [Column("Surcharge1Price")]
	    public decimal? Surcharge1Price { get; set; }
        [Column("Surcharge2Price")]
	    public decimal? Surcharge2Price { get; set; }
        [Column("Surcharge3Price")]
	    public decimal? Surcharge3Price { get; set; }
        [Column("Surcharge4Price")]
	    public decimal? Surcharge4Price { get; set; }
        [Column("Surcharge5Price")]
	    public decimal? Surcharge5Price { get; set; }
        [Column("Surcharge6Price")]
	    public decimal? Surcharge6Price { get; set; }
        [Column("Surcharge7Price")]
	    public decimal? Surcharge7Price { get; set; }
        [Column("Surcharge8Price")]
	    public decimal? Surcharge8Price { get; set; }
        [Column("Surcharge9Price")]
	    public decimal? Surcharge9Price { get; set; }
        [Column("Surcharge10Price")]
	    public decimal? Surcharge10Price { get; set; }
        [ForeignKey("OriginPort")]
        [Column("OriginPortId")]
	    public string OriginPortId { get; set; }
	      
        public virtual Port OriginPort { get; set; }
        [ForeignKey("DestinationPort")]
        [Column("DestinationPortId")]
	    public string DestinationPortId { get; set; }
	      
        public virtual Port DestinationPort { get; set; }
        [Column("OriginPortText")]
	    public string OriginPortText { get; set; }
        [Column("DestinationPortText")]
	    public string DestinationPortText { get; set; }
        [Column("MinPriceText")]
	    public string MinPriceText { get; set; }
        [Column("Step1PriceText")]
	    public string Step1PriceText { get; set; }
        [Column("Step2PriceText")]
	    public string Step2PriceText { get; set; }
        [Column("Step3PriceText")]
	    public string Step3PriceText { get; set; }
        [Column("Step4PriceText")]
	    public string Step4PriceText { get; set; }
        [Column("Step5PriceText")]
	    public string Step5PriceText { get; set; }
        [Column("Step6PriceText")]
	    public string Step6PriceText { get; set; }
        [Column("Step7PriceText")]
	    public string Step7PriceText { get; set; }
        [Column("Step8PriceText")]
	    public string Step8PriceText { get; set; }
        [Column("Surcharge1PriceText")]
	    public string Surcharge1PriceText { get; set; }
        [Column("Surcharge2PriceText")]
	    public string Surcharge2PriceText { get; set; }
        [Column("Surcharge3PriceText")]
	    public string Surcharge3PriceText { get; set; }
        [Column("Surcharge4PriceText")]
	    public string Surcharge4PriceText { get; set; }
        [Column("Surcharge5PriceText")]
	    public string Surcharge5PriceText { get; set; }
        [Column("Surcharge6PriceText")]
	    public string Surcharge6PriceText { get; set; }
        [Column("Surcharge7PriceText")]
	    public string Surcharge7PriceText { get; set; }
        [Column("Surcharge8PriceText")]
	    public string Surcharge8PriceText { get; set; }
        [Column("Surcharge9PriceText")]
	    public string Surcharge9PriceText { get; set; }
        [Column("Surcharge10PriceText")]
	    public string Surcharge10PriceText { get; set; }
        [Column("HasErrors")]
	    public bool HasErrors { get; set; }
        [Column("ErrorText")]
	    public string ErrorText { get; set; }
    }
}
	 