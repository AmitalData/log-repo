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
   
    public class DashboardGlobalFilter
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("Dashboard")]
        [Column("DashboardId")]
	    public string DashboardId { get; set; }
	      
        public virtual Dashboard Dashboard { get; set; }
        [Column("IsCommonFilter")]
	    public bool IsCommonFilter { get; set; }
        [Column("CommonFilterField")]
	    public string CommonFilterField { get; set; }
        [ForeignKey("DataSet")]
        [Column("DataSetId")]
	    public string DataSetId { get; set; }
	      
        public virtual AnalyticsFactsMetaData DataSet { get; set; }
        [ForeignKey("DataSetField")]
        [Column("DataSetFieldId")]
	    public string DataSetFieldId { get; set; }
	      
        public virtual AnalyticsFactsFieldsMetaData DataSetField { get; set; }
        [Column("FilterOperator")]
	    public string FilterOperator { get; set; }
        [Column("DataTypeCode")]
	    public string DataTypeCode { get; set; }
        [Column("LineNumber")]
	    public int LineNumber { get; set; }
    }
}
	 