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
   
    public class GLAccountTotalByMonth
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("GLAccount")]
        [Column("AccountId" ,Order = 1)]
	    public string AccountId { get; set; }
	      
        public virtual GLAccount GLAccount { get; set; }
     [Key]
        [ForeignKey("GLAccountTotalDateType")]
        [Column("DateTypeCode" ,Order = 2)]
	    public string DateTypeCode { get; set; }
	      
        public virtual GLAccountTotalDateType GLAccountTotalDateType { get; set; }
     [Key]
        [Column("Year" ,Order = 3)]
	    public int Year { get; set; }
     [Key]
        [Column("Month" ,Order = 4)]
	    public int Month { get; set; }
     [Key]
        [ForeignKey("Currency")]
        [Column("CurrencyId" ,Order = 5)]
	    public string CurrencyId { get; set; }
	      
        public virtual Currency Currency { get; set; }
        [Column("LocalAmountDebit")]
	    public decimal LocalAmountDebit { get; set; }
        [Column("LocalAmountCredit")]
	    public decimal LocalAmountCredit { get; set; }
        [Column("ForeignAmountDebit")]
	    public decimal ForeignAmountDebit { get; set; }
        [Column("ForeignAmountCredit")]
	    public decimal ForeignAmountCredit { get; set; }
    }
}
	 