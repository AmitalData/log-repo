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
   
    public class TaskExtended
    {
	 string dbms;

        [Key]
        [ForeignKey("Task")]
        [Column("Id")]
	    public string Id { get; set; }
	      
        public virtual Task Task { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("Fields")]
	    public string Fields { get; set; }
        [Column("ToDoConditions")]
	    public string ToDoConditions { get; set; }
        [Column("DoneConditions")]
	    public string DoneConditions { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
    }
}
	 