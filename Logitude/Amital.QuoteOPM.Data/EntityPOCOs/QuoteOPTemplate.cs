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
   
    public class QuoteOPTemplate
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [ForeignKey("CurrentTenant")]
        [Column("Tenant")]
	    public int Tenant { get; set; }
	      
        public virtual Tenant CurrentTenant { get; set; }
        [ForeignKey("HeaderDoc")]
        [Column("HeaderDocId")]
	    public string HeaderDocId { get; set; }
	      
        public virtual Document HeaderDoc { get; set; }
        [ForeignKey("FooterDoc")]
        [Column("FooterDocId")]
	    public string FooterDocId { get; set; }
	      
        public virtual Document FooterDoc { get; set; }
        [ForeignKey("QuoteOPTemplateSetting")]
        [Column("QuoteOPTemplateSettingId")]
	    public string QuoteOPTemplateSettingId { get; set; }
	      
        public virtual QuoteOPTemplateSetting QuoteOPTemplateSetting { get; set; }
        [Column("Name")]
	    public string Name { get; set; }
        [Column("IsTemplate")]
	    public bool IsTemplate { get; set; }
        [ForeignKey("OriginalQuoteOPTemplate")]
        [Column("OriginalQuoteOPTemplateId")]
	    public string OriginalQuoteOPTemplateId { get; set; }
	      
        public virtual QuoteOPTemplate OriginalQuoteOPTemplate { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("QuoteOPType")]
        [Column("TemplateTypeCode")]
	    public string TemplateTypeCode { get; set; }
	      
        public virtual QuoteOPType QuoteOPType { get; set; }
        [Column("IsDefault")]
	    public bool IsDefault { get; set; }
        [Column("InActive")]
	    public bool InActive { get; set; }
        [Column("IsCopiedAtSignup")]
	    public bool IsCopiedAtSignup { get; set; }
        [Column("IsEnabledForCustomers")]
	    public bool IsEnabledForCustomers { get; set; }
    }
}
	 