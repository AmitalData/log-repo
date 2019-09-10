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
   
    public class TariffSurchargesUpdate
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("TariffId")]
	    public string TariffId { get; set; }
        [Column("StartDate")]
	    public DateTime? StartDate { get; set; }
        [Column("LinesUpdated")]
	    public int? LinesUpdated { get; set; }
        [Column("From")]
	    public string From { get; set; }
        [Column("To")]
	    public string To { get; set; }
        [Column("Version")]
	    public int Version { get; set; }
        [Column("Surcharges")]
	    public string Surcharges { get; set; }
        [ForeignKey("UpdateMethod")]
        [Column("UpdateMethodCode")]
	    public string UpdateMethodCode { get; set; }
	      
        public virtual TariffSurchargesUpdateMethod UpdateMethod { get; set; }
    }
}
	 