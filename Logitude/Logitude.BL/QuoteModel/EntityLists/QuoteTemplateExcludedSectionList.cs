using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.QuoteModel.EntityLists
{
    public class QuoteTemplateExcludedSectionList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        public string QuoteTemplateSectionId { get; set; }
        public string QuoteId { get; set; }
        public string QuoteTemplateId { get; set; }
    }
}