using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.QuoteModel.EntityLists
{
    public class QuoteTemplateSectionList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QuoteTemplateId { get; set; }
        public string SectionDocId { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string QuoteTemplateSectionTypeCode { get; set; }
        public bool IsCancel { get; set; }
    }
}