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
   
    public class OpportunityProductLocation
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
     [Key]
        [Column("LineNumber" ,Order = 3)]
	    public int LineNumber { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("Country")]
        [Column("CountryId")]
	    public string CountryId { get; set; }
	      
        public virtual Country Country { get; set; }
        [Column("TEU")]
	    public decimal? TEU { get; set; }
        [Column("NumberOfShipments")]
	    public int? NumberOfShipments { get; set; }
        [Column("ChargeableWeight")]
	    public decimal? ChargeableWeight { get; set; }
        [Column("Revenue")]
	    public decimal? Revenue { get; set; }
    }
}
	 