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
   
    public class WorkFlowInstanceActivity
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
        [Column("Sequence")]
	    public int Sequence { get; set; }
        [Column("ActionName")]
	    public string ActionName { get; set; }
        [Column("StartTime")]
	    public DateTime? StartTime { get; set; }
        [Column("Duration")]
	    public decimal? Duration { get; set; }
        [ForeignKey("Status")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual WorkFlowInstanceActivityStatus Status { get; set; }
        [ForeignKey("WorkFlowInstance")]
        [Column("WorkflowInstanceId")]
	    public string WorkflowInstanceId { get; set; }
	      
        public virtual WorkFlowInstance WorkFlowInstance { get; set; }
        [Column("EndTime")]
	    public DateTime? EndTime { get; set; }
        [Column("ErrorMessage")]
	    public string ErrorMessage { get; set; }
        [Column("Result")]
	    public string Result { get; set; }
        [Column("ActionType")]
	    public string ActionType { get; set; }
    }
}
	 