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
using Logitude.DashboardModule.Data.EntityPOCOs;

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
        [ForeignKey("Measure1FieldsMetaData")]
        [Column("Measure1FieldId")]
	    public string Measure1FieldId { get; set; }
	      
        public virtual AnalyticsFactsFieldsMetaData Measure1FieldsMetaData { get; set; }
        [ForeignKey("Measure2FieldsMetaData")]
        [Column("Measure2FieldId")]
	    public string Measure2FieldId { get; set; }
	      
        public virtual AnalyticsFactsFieldsMetaData Measure2FieldsMetaData { get; set; }
        [Column("Measure1Operation")]
	    public string Measure1Operation { get; set; }
        [Column("Measure2Operation")]
	    public string Measure2Operation { get; set; }
    }
}
	 