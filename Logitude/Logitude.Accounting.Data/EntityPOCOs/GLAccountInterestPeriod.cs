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
   
    public class GLAccountInterestPeriod
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [Column("LineNumber")]
	    public int LineNumber { get; set; }
     [Key]
        [ForeignKey("GLAccount")]
        [Column("GLAccountId")]
	    public string GLAccountId { get; set; }
	      
        public virtual GLAccount GLAccount { get; set; }
        [Column("PeriodStartDate")]
	    public DateTime PeriodStartDate { get; set; }
        [ForeignKey("StandardInterestBasesType")]
        [Column("StandardInterestRateBaseId")]
	    public string StandardInterestRateBaseId { get; set; }
	      
        public virtual InterestBasesType StandardInterestBasesType { get; set; }
        [Column("StandardAddInterestPercent")]
	    public decimal? StandardAddInterestPercent { get; set; }
        [ForeignKey("ExceptionalInterestBasesType")]
        [Column("ExceptionalInterestRateBaseId")]
	    public string ExceptionalInterestRateBaseId { get; set; }
	      
        public virtual InterestBasesType ExceptionalInterestBasesType { get; set; }
        [Column("ExceptionalAddInterestPercent")]
	    public decimal? ExceptionalAddInterestPercent { get; set; }
        [ForeignKey("CreditInterestBasesType")]
        [Column("CreditInterestRateBaseId")]
	    public string CreditInterestRateBaseId { get; set; }
	      
        public virtual InterestBasesType CreditInterestBasesType { get; set; }
        [Column("CreditAddInterestPercent")]
	    public decimal? CreditAddInterestPercent { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("UpdateDateTime")]
	    public DateTime UpdateDateTime { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("CreateDateTime")]
	    public DateTime CreateDateTime { get; set; }
    }
}
	 