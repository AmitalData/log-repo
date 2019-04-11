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
        [Column("OriginPortId")]
	    public string OriginPortId { get; set; }
    }
}
	 