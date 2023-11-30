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
        [Column("DateGroupCode")]
	    public string DateGroupCode { get; set; }
        [Column("MaximumGrouping")]
	    public int? MaximumGrouping { get; set; }
        [Column("SortBy")]
	    public int? SortBy { get; set; }
        [Column("SortDirection")]
	    public string SortDirection { get; set; }
        [Column("TimeOverTime")]
	    public bool TimeOverTime { get; set; }
        [Column("ComparisonPeriod")]
	    public int? ComparisonPeriod { get; set; }
        [Column("Increase")]
	    public string Increase { get; set; }
        [Column("ComparisonOperator")]
	    public string ComparisonOperator { get; set; }
        [Column("ComparisonDateGroup")]
	    public string ComparisonDateGroup { get; set; }
        [Column("FromDate")]
	    public DateTime? FromDate { get; set; }
        [Column("ToDate")]
	    public DateTime? ToDate { get; set; }
        [ForeignKey("SecondaryGroupFieldsMetaData")]
        [Column("SecondaryGroupById")]
	    public string SecondaryGroupById { get; set; }
	      
        public virtual AnalyticsFactsFieldsMetaData SecondaryGroupFieldsMetaData { get; set; }
        [Column("SecondaryDateGroupCode")]
	    public string SecondaryDateGroupCode { get; set; }
        [Column("Alignment")]
	    public string Alignment { get; set; }
        [Column("ThousandSeparator")]
	    public bool? ThousandSeparator { get; set; }
        [Column("UseNumberAbbreviation")]
	    public bool? UseNumberAbbreviation { get; set; }
        [Column("DecimalPlaces")]
	    public int? DecimalPlaces { get; set; }
        [Column("UseAbbreviationAfter")]
	    public string UseAbbreviationAfter { get; set; }
        [Column("LabelsPosition")]
	    public string LabelsPosition { get; set; }
    }
}
	 