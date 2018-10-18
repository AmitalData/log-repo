using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    public class QuoteTemplateSectionPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QuoteTemplateId { get; set; }
        public string SectionDocId { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string QuoteTemplateSectionTypeCode { get; set; }
        public bool IschangeBodySection { get; set; }
        public bool IsSettingTypeCodeS { get; set; }
        public bool IsSettingTypeCodeP { get; set; }
        public bool IsCancel { get; set; }
        public byte[] Templatedata { get; set; }
        public string Description { get; set; }

        public string QuoteId { get; set; }
        public bool IsQuoteEdited { get; set; }
        public bool IsExcluded { get; set; }
        
    }
}