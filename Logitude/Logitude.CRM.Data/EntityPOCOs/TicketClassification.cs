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
using Logitude.Infrastructure.Data.EntityPOCOs;
namespace Logitude.CRM.Data.EntityPOCOs
{
   
    public class TicketClassification
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("Name")]
	    public string Name { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("Inactive")]
	    public bool Inactive { get; set; }
        [Column("ParentId")]
	    public string ParentId { get; set; }
        [ForeignKey("DefaultSeverity")]
        [Column("DefaultSeverityId")]
	    public string DefaultSeverityId { get; set; }
	      
        public virtual TicketSeverity DefaultSeverity { get; set; }
        [ForeignKey("EmployeeGroup")]
        [Column("EmployeeGroupId")]
	    public string EmployeeGroupId { get; set; }
	      
        public virtual EmployeeGroup EmployeeGroup { get; set; }
        [ForeignKey("EscalationUser")]
        [Column("ManagerUserId")]
	    public string ManagerUserId { get; set; }
	      
        public virtual User EscalationUser { get; set; }
        [Column("EscalationNotify")]
	    public string EscalationNotify { get; set; }
    }
}
	 