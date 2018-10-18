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
   
    public class TicketEscalation
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime? CreateDate { get; set; }
        [Column("TicketId")]
	    public string TicketId { get; set; }
        [Column("LineNumber")]
	    public int LineNumber { get; set; }
        [Column("EscalationFor")]
	    public string EscalationFor { get; set; }
        [Column("Recepients")]
	    public string Recepients { get; set; }
        [Column("IsClose")]
	    public bool IsClose { get; set; }
        [Column("IsSLAViolated")]
	    public bool IsSLAViolated { get; set; }
        [Column("DueDate")]
	    public DateTime? DueDate { get; set; }
        [Column("CloseDate")]
	    public DateTime? CloseDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
    }
}
	 