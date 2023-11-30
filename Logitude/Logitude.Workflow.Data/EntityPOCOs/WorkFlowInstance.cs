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

namespace Logitude.Workflow.Data.EntityPOCOs
{
   
    public class WorkFlowInstance
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
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("Status")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual WorkFlowInstanceStatus Status { get; set; }
        [Column("StartTime")]
	    public DateTime? StartTime { get; set; }
        [Column("EndTime")]
	    public DateTime? EndTime { get; set; }
        [Column("BusinessKey")]
	    public string BusinessKey { get; set; }
        [Column("Duration")]
	    public decimal? Duration { get; set; }
        [ForeignKey("WorkFlowVersion")]
        [Column("WorkFlowVersionId")]
	    public string WorkFlowVersionId { get; set; }
	      
        public virtual WorkFlowVersion WorkFlowVersion { get; set; }
        [Column("RetryAttemptsNumber")]
	    public int RetryAttemptsNumber { get; set; }
        [Column("WorkflowId")]
	    public string WorkflowId { get; set; }
        [Column("NumberOfActivities")]
	    public int NumberOfActivities { get; set; }
    }
}
	 