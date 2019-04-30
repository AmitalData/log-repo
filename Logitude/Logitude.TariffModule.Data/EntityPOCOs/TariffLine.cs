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
	    public int? MinPrice { get; set; }
        [Column("Step1Price")]
	    public int? Step1Price { get; set; }
        [Column("Step2Price")]
	    public int? Step2Price { get; set; }
        [Column("Step3Price")]
	    public int? Step3Price { get; set; }
        [Column("Step4Price")]
	    public int? Step4Price { get; set; }
        [Column("Step5Price")]
	    public int? Step5Price { get; set; }
        [Column("Step6Price")]
	    public int? Step6Price { get; set; }
        [Column("Step7Price")]
	    public int? Step7Price { get; set; }
        [Column("Step8Price")]
	    public int? Step8Price { get; set; }
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
    }
}
	 