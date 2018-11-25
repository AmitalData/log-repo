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
   
    public class JournalAdditionalData
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("Journal")]
        [Column("JournalId")]
	    public string JournalId { get; set; }
	      
        public virtual Journal Journal { get; set; }
        [ForeignKey("TaxReport")]
        [Column("TaxReportId")]
	    public string TaxReportId { get; set; }
	      
        public virtual TaxReport TaxReport { get; set; }
        [ForeignKey("TaxReportStatus")]
        [Column("TaxReportStatusCode")]
	    public string TaxReportStatusCode { get; set; }
	      
        public virtual TaxReportStatus TaxReportStatus { get; set; }
    }
}
	 