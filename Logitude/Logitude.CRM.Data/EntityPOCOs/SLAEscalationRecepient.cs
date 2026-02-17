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
   
    public class SLAEscalationRecepient
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("SLAEscalation")]
        [Column("SLAEscalationId")]
	    public string SLAEscalationId { get; set; }
	      
        public virtual SLAEscalation SLAEscalation { get; set; }
        [ForeignKey("EscalationPreDefinition")]
        [Column("PreDefinitionId")]
	    public string PreDefinitionId { get; set; }
	      
        public virtual EscalationPreDefinition EscalationPreDefinition { get; set; }
        [ForeignKey("User")]
        [Column("UserId")]
	    public string UserId { get; set; }
	      
        public virtual User User { get; set; }
    }
}
	 