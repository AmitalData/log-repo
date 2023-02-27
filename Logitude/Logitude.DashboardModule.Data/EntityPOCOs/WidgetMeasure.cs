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
   
    public class WidgetMeasure
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("Widget")]
        [Column("WidgetId")]
	    public string WidgetId { get; set; }
	      
        public virtual Widget Widget { get; set; }
        [ForeignKey("Measure")]
        [Column("MeasureCode")]
	    public string MeasureCode { get; set; }
	      
        public virtual MeasureType Measure { get; set; }
        [ForeignKey("MeasureField")]
        [Column("MeasureFieldId")]
	    public string MeasureFieldId { get; set; }
	      
        public virtual AnalyticsFactsFieldsMetaData MeasureField { get; set; }
        [Column("RenderAs")]
	    public string RenderAs { get; set; }
        [Column("YAxisType")]
	    public string YAxisType { get; set; }
    }
}
	 