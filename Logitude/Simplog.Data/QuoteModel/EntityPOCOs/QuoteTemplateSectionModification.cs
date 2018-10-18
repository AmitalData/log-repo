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
    public class QuoteTemplateSectionModification
    {
        [Key]
        public string Id { get; set; }

        public string QuoteTemplateSectionId { get; set; }
        public string QuoteId { get; set; }
        public string SectionDocId { get; set; }
        public int Tenant { get; set; }


        [ForeignKey("QuoteTemplateSectionId")]
        public virtual QuoteTemplateSection QuoteTemplateSection { get; set; }

        [ForeignKey("SectionDocId")]
        public virtual Document SectionDoc { get; set; }
    }
}
