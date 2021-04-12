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
   
    public class GLAccountAgingData
    {
	 string dbms;

        [Key]
        [ForeignKey("GLAccount")]
        [Column("AccountId" ,Order = 1)]
	    public string AccountId { get; set; }
	      
        public virtual GLAccount GLAccount { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("PeriodPast")]
	    public decimal? PeriodPast { get; set; }
        [Column("Period0")]
	    public decimal? Period0 { get; set; }
        [Column("Period1")]
	    public decimal? Period1 { get; set; }
        [Column("Period2")]
	    public decimal? Period2 { get; set; }
        [Column("Period3")]
	    public decimal? Period3 { get; set; }
        [Column("Period4")]
	    public decimal? Period4 { get; set; }
        [Column("Period5")]
	    public decimal? Period5 { get; set; }
        [Column("PeriodFuture")]
	    public decimal? PeriodFuture { get; set; }
        [Column("TotalOpenTransactions")]
	    public int? TotalOpenTransactions { get; set; }
    }
}
	 