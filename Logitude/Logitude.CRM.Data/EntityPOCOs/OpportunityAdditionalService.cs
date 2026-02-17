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
   
    public class OpportunityAdditionalService
    {
	 string dbms;

        [Key]
        [ForeignKey("Opportunity")]
        [Column("OpportunityId" ,Order = 1)]
	    public string OpportunityId { get; set; }
	      
        public virtual Opportunity Opportunity { get; set; }
     [Key]
        [ForeignKey("AdditionalService")]
        [Column("AdditionalServiceId" ,Order = 2)]
	    public string AdditionalServiceId { get; set; }
	      
        public virtual AdditionalService AdditionalService { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [Column("NotesRightToLeft")]
	    public bool NotesRightToLeft { get; set; }
    }
}
	 