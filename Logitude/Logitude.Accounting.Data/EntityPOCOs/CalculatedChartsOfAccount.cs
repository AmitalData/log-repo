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
   
    public class CalculatedChartsOfAccount
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDateTime")]
	    public DateTime CreateDateTime { get; set; }
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
        [Column("UpdatedDateTime")]
	    public DateTime UpdatedDateTime { get; set; }
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
        [Column("UserDefinedReportId")]
	    public string UserDefinedReportId { get; set; }
        [Column("LocalName")]
	    public string LocalName { get; set; }
        [Column("EnglishName")]
	    public string EnglishName { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [Column("ChartOfAccountTypeCode")]
	    public string ChartOfAccountTypeCode { get; set; }
    }
}
	 