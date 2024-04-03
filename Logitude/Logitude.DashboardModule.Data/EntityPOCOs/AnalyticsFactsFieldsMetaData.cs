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
   
    public class AnalyticsFactsFieldsMetaData
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("AnalyticsFactsMetaData")]
        [Column("AnalyticsFactsMetaDataId")]
	    public string AnalyticsFactsMetaDataId { get; set; }
	      
        public virtual AnalyticsFactsMetaData AnalyticsFactsMetaData { get; set; }
        [ForeignKey("FieldDataType")]
        [Column("DataTypeCode")]
	    public string DataTypeCode { get; set; }
	      
        public virtual FieldDataType FieldDataType { get; set; }
        [Column("CanMeasure")]
	    public bool CanMeasure { get; set; }
        [Column("CanGroup")]
	    public bool CanGroup { get; set; }
        [Column("FieldCode")]
	    public string FieldCode { get; set; }
        [Column("DisplayName")]
	    public string DisplayName { get; set; }
        [Column("DisplayNamePlural")]
	    public string DisplayNamePlural { get; set; }
        [Column("JoinedTableName")]
	    public string JoinedTableName { get; set; }
        [Column("JoinedTableKey")]
	    public string JoinedTableKey { get; set; }
        [Column("JoinedTableDisplayField")]
	    public string JoinedTableDisplayField { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("JoinedTableDBName")]
	    public string JoinedTableDBName { get; set; }
        [Column("HasUnit")]
	    public bool HasUnit { get; set; }
        [Column("Unit")]
	    public string Unit { get; set; }
        [Column("CommonFilterCode")]
	    public string CommonFilterCode { get; set; }
        [Column("CanSecondaryGroup")]
	    public bool CanSecondaryGroup { get; set; }
        [Column("AllowTenantZeroFilter")]
	    public bool AllowTenantZeroFilter { get; set; }
    }
}
	 