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
   
    public class WorkFlowVersion
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
        [Column("VersionNumber")]
	    public int VersionNumber { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
        [ForeignKey("Status")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual WorkFlowStatus Status { get; set; }
        [ForeignKey("WorkFlow")]
        [Column("WorkflowId")]
	    public string WorkflowId { get; set; }
	      
        public virtual WorkFlow WorkFlow { get; set; }
        [Column("FlowJson")]
	    public string FlowJson { get; set; }
        [Column("Entity")]
	    public string Entity { get; set; }
        [Column("Trigger")]
	    public string Trigger { get; set; }
        [Column("ActivatedDate")]
	    public DateTime? ActivatedDate { get; set; }
    }
}
	 