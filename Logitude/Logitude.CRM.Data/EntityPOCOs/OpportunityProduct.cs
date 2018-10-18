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
   
    public class OpportunityProduct
    {
	 string dbms;

        [Key]
        [ForeignKey("Opportunity")]
        [Column("OpportunityId" ,Order = 1)]
	    public string OpportunityId { get; set; }
	      
        public virtual Opportunity Opportunity { get; set; }
     [Key]
        [ForeignKey("OpportunityProductType")]
        [Column("OpportunityProductTypeCode" ,Order = 2)]
	    public string OpportunityProductTypeCode { get; set; }
	      
        public virtual ProductType OpportunityProductType { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [Column("ChargeableWeight")]
	    public decimal? ChargeableWeight { get; set; }
        [Column("TEU")]
	    public decimal? TEU { get; set; }
        [Column("NumberOfShipments")]
	    public int? NumberOfShipments { get; set; }
        [Column("Revenue")]
	    public decimal? Revenue { get; set; }
        [ForeignKey("PrepaidCollect")]
        [Column("PrepaidCollectId")]
	    public string PrepaidCollectId { get; set; }
	      
        public virtual PrepaidCollect PrepaidCollect { get; set; }
        [Column("NotesRightToLeft")]
	    public bool NotesRightToLeft { get; set; }
    }
}
	 