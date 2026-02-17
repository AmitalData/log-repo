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

namespace Logitude.CRM.Data.EntityPOCOs
{
   
    public class OpportunityStage
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("Opportunity")]
        [Column("OpportunityId")]
	    public string OpportunityId { get; set; }
	      
        public virtual Opportunity Opportunity { get; set; }
        [ForeignKey("FromStage")]
        [Column("FromStageId")]
	    public string FromStageId { get; set; }
	      
        public virtual Stage FromStage { get; set; }
        [ForeignKey("ToStage")]
        [Column("ToStageId")]
	    public string ToStageId { get; set; }
	      
        public virtual Stage ToStage { get; set; }
        [Column("StartDate")]
	    public DateTime? StartDate { get; set; }
        [Column("EndDate")]
	    public DateTime? EndDate { get; set; }
    }
}
	 