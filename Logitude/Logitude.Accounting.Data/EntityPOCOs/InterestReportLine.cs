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
   
    public class InterestReportLine
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [Column("InterestReportId")]
	    public string InterestReportId { get; set; }
        [ForeignKey("InterestTransaction")]
        [Column("InterestTransactionId")]
	    public string InterestTransactionId { get; set; }
	      
        public virtual InterestTransaction InterestTransaction { get; set; }
    }
}
	 