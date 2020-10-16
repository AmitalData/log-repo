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

namespace Logitude.Accounting.Data.EntityPOCOs
{
   
    public class CalculatedChartsOfAccountsLine
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [Column("CreateDateTime")]
	    public DateTime CreateDateTime { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("UpdatedDateTime")]
	    public DateTime UpdatedDateTime { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("IsDetailedGLAccount")]
	    public bool IsDetailedGLAccount { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [ForeignKey("GLAccount")]
        [Column("GLAccountId")]
	    public string GLAccountId { get; set; }
	      
        public virtual GLAccount GLAccount { get; set; }
        [ForeignKey("ChartOfAccount")]
        [Column("ChartOfAccountId")]
	    public string ChartOfAccountId { get; set; }
	      
        public virtual ChartOfAccount ChartOfAccount { get; set; }
        [ForeignKey("CalculatedChartsLineType")]
        [Column("LineTypeCode")]
	    public string LineTypeCode { get; set; }
	      
        public virtual CalculatedChartsLineType CalculatedChartsLineType { get; set; }
        [ForeignKey("CalculatedChartsOfAccount")]
        [Column("CalculatedChartsOfAccountsId")]
	    public string CalculatedChartsOfAccountsId { get; set; }
	      
        public virtual ChartOfAccount CalculatedChartsOfAccount { get; set; }
    }
}
	 