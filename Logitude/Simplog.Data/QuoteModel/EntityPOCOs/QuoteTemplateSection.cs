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
   public class QuoteTemplateSection
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QuoteTemplateId { get; set; }
        public string SectionDocId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public string Description { get; set; }
        

        public bool IsCancel { get; set; }
        public string QuoteTemplateSectionTypeCode { get; set; }


        [ForeignKey("QuoteTemplateId")]
        public virtual QuoteTemplate QuoteTemplate { get; set; }

        [ForeignKey("SectionDocId")]
        public virtual Document SectionDoc { get; set; }

        [ForeignKey("QuoteTemplateSectionTypeCode")]
        public virtual QuoteTemplateSectionType QuoteTemplateSectionType { get; set; }


    }
}
