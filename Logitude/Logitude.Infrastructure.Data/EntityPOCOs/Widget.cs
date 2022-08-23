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

namespace Logitude.Infrastructure.Data.EntityPOCOs
{
   
    public class Widget
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("Title")]
	    public string Title { get; set; }
        [Column("GroupBy")]
	    public string GroupBy { get; set; }
        [ForeignKey("Dashboard")]
        [Column("DashboardId")]
	    public string DashboardId { get; set; }
	      
        public virtual Dashboard Dashboard { get; set; }
        [Column("StartPotistion")]
	    public string StartPotistion { get; set; }
        [Column("EndPosition")]
	    public string EndPosition { get; set; }
        [ForeignKey("WidgetType")]
        [Column("TypeCode")]
	    public string TypeCode { get; set; }
	      
        public virtual WidgetType WidgetType { get; set; }
    }
}
	 