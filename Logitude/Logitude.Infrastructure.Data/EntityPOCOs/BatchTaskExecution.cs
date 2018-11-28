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

namespace Logitude.Infrastructure.Data.EntityPOCOs
{
   
    public class BatchTaskExecution
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("ClassName")]
	    public string ClassName { get; set; }
        [Column("PrametersXml")]
	    public string PrametersXml { get; set; }
        [ForeignKey("BatchTaskExecutionStatus")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual BatchTaskExecutionStatus BatchTaskExecutionStatus { get; set; }
        [Column("ErrorLog")]
	    public string ErrorLog { get; set; }
        [Column("StartDateTime")]
	    public DateTime? StartDateTime { get; set; }
        [Column("DoneDateTime")]
	    public DateTime? DoneDateTime { get; set; }
        [Column("ProgressMessage")]
	    public string ProgressMessage { get; set; }
        [Column("ProgressPercentage")]
	    public int ProgressPercentage { get; set; }
        [Column("Subject")]
	    public string Subject { get; set; }
        [Column("CallStack")]
	    public string CallStack { get; set; }
    }
}
	 