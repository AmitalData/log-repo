using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class QuoteTemplate
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string HeaderDocId { get; set; }
        public string FooterDocId { get; set; }
        public string QuoteTemplateSettingId { get; set; }
        public string Name { get; set; }
        public bool IsTemplate { get; set; }
        public string OriginalQuoteTemplateId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string SearchFields { get; set; }
        public bool IsDefault { get; set; }
        public string TemplateTypeCode { get; set; }
        public bool InActive { get; set; }

        public bool IsCopiedAtSignup { get; set; }
        public bool IsEnabledForCustomers { get; set; }
  
        

        [ForeignKey("QuoteTemplateSettingId")]
        public virtual QuoteTemplateSetting QuoteTemplateSetting { get; set; }
        [ForeignKey("HeaderDocId")]
        public virtual Document HeaderDoc { get; set; }
        [ForeignKey("FooterDocId")]
        public virtual Document FooterDoc { get; set; }

        [ForeignKey("OriginalQuoteTemplateId")]
        public virtual QuoteTemplate OriginalQuoteTemplate { get; set; }


        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }


        [ForeignKey("TemplateTypeCode")]
        public virtual QuoteType QuoteType { get; set; }


    }
}
