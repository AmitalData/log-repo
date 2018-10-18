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
   
    public class AccountingPeriod
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("Year")]
	    public int Year { get; set; }
        [ForeignKey("PeriodType")]
        [Column("PeriodTypeCode")]
	    public string PeriodTypeCode { get; set; }
	      
        public virtual PeriodType PeriodType { get; set; }
        [Column("OpenMonth")]
	    public int OpenMonth { get; set; }
        [Column("ClosedMonth")]
	    public int? ClosedMonth { get; set; }
    }
}
	 