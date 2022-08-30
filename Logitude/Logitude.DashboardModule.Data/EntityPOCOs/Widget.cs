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

namespace Logitude.DashboardModule.Data.EntityPOCOs
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
        [ForeignKey("GroupFieldsMetaData")]
        [Column("GroupById")]
	    public string GroupById { get; set; }
	      
        public virtual AnalyticsFactsFieldsMetaData GroupFieldsMetaData { get; set; }
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
        [ForeignKey("EntityMetaData")]
        [Column("EntityId")]
	    public string EntityId { get; set; }
	      
        public virtual AnalyticsFactsMetaData EntityMetaData { get; set; }
        [Column("Filters")]
	    public string Filters { get; set; }
    }
}
	 