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

namespace Amital.QuoteOPM.Data.EntityPOCOs
{
   
    public class QuoteOPTemplateSection
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("Name")]
	    public string Name { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
        [Column("IsCancel")]
	    public bool IsCancel { get; set; }
        [ForeignKey("QuoteOPTemplate")]
        [Column("QuoteTemplateId")]
	    public string QuoteTemplateId { get; set; }
	      
        public virtual QuoteOPTemplate QuoteOPTemplate { get; set; }
        [ForeignKey("SectionDoc")]
        [Column("SectionDocId")]
	    public string SectionDocId { get; set; }
	      
        public virtual Document SectionDoc { get; set; }
        [Column("Order")]
	    public int Order { get; set; }
        [ForeignKey("QuoteOPTemplateSectionType")]
        [Column("QuoteOPTemplateSectionTypeCode")]
	    public string QuoteOPTemplateSectionTypeCode { get; set; }
	      
        public virtual QuoteOPTemplateSectionType QuoteOPTemplateSectionType { get; set; }
    }
}
	 