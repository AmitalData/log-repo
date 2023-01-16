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
   
    public class DashboardGlobalPresetFilter
    {
	 string dbms;

        [Key]
        [Column("Code")]
	    public string Code { get; set; }
        [Column("DisplayName")]
	    public string DisplayName { get; set; }
        [ForeignKey("FieldDataType")]
        [Column("DataTypeCode")]
	    public string DataTypeCode { get; set; }
	      
        public virtual FieldDataType FieldDataType { get; set; }
        [Column("IsDisabled")]
	    public bool IsDisabled { get; set; }
        [Column("IsMultiSelect")]
	    public bool IsMultiSelect { get; set; }
        [Column("JoinedTableName")]
	    public string JoinedTableName { get; set; }
        [Column("Sort")]
	    public int? Sort { get; set; }
        [Column("JoinedTableDisplayField")]
	    public string JoinedTableDisplayField { get; set; }
        [Column("CanSearch")]
	    public bool CanSearch { get; set; }
    }
}
	 