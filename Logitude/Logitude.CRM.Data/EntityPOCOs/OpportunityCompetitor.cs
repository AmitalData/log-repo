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
   
    public class OpportunityCompetitor
    {
	 string dbms;

        [Key]
        [ForeignKey("Opportunity")]
        [Column("OpportunityId" ,Order = 1)]
	    public string OpportunityId { get; set; }
	      
        public virtual Opportunity Opportunity { get; set; }
     [Key]
        [ForeignKey("Competitor")]
        [Column("CompetitorId" ,Order = 2)]
	    public string CompetitorId { get; set; }
	      
        public virtual Competitor Competitor { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
    }
}
	 