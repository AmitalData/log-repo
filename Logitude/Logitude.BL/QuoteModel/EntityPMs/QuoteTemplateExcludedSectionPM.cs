using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    public class QuoteTemplateExcludedSectionPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        public string QuoteTemplateSectionId { get; set; }
        public string QuoteId { get; set; }
        public string QuoteTemplateId { get; set; }
    }
}

