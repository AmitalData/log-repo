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
   
    public class Task
    {
	 string dbms;

        [Key]
        [ForeignKey("TaskExtended")]
        [Column("Id")]
	    public string Id { get; set; }
	      
        public virtual TaskExtended TaskExtended { get; set; }
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
        [Column("Subject")]
	    public string Subject { get; set; }
        [Column("DueDate")]
	    public DateTime DueDate { get; set; }
        [ForeignKey("Owner")]
        [Column("OwnerId")]
	    public string OwnerId { get; set; }
	      
        public virtual User Owner { get; set; }
        [ForeignKey("Priority")]
        [Column("PriorityId")]
	    public string PriorityId { get; set; }
	      
        public virtual TaskPriority Priority { get; set; }
        [ForeignKey("Status")]
        [Column("StatusId")]
	    public string StatusId { get; set; }
	      
        public virtual TaskStatus Status { get; set; }
        [Column("EntityId")]
	    public string EntityId { get; set; }
        [ForeignKey("TaskType")]
        [Column("TaskTypeId")]
	    public string TaskTypeId { get; set; }
	      
        public virtual TaskType TaskType { get; set; }
        [ForeignKey("EntityObjectTable")]
        [Column("EntityObjectTableId")]
	    public string EntityObjectTableId { get; set; }
	      
        public virtual ObjectTable EntityObjectTable { get; set; }
        [ForeignKey("ClosedByUser")]
        [Column("ClosedByUserId")]
	    public string ClosedByUserId { get; set; }
	      
        public virtual User ClosedByUser { get; set; }
        [Column("ClosedDate")]
	    public DateTime? ClosedDate { get; set; }
        [Column("IsClosed")]
	    public bool IsClosed { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [ForeignKey("CheckWith")]
        [Column("CheckWithId")]
	    public string CheckWithId { get; set; }
	      
        public virtual Card CheckWith { get; set; }
        [Column("EntityNumber")]
	    public string EntityNumber { get; set; }
        [Column("IsAssigned")]
	    public bool IsAssigned { get; set; }
        [Column("StartDate")]
	    public DateTime? StartDate { get; set; }
    }
}
	 