using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    public class QuoteTemplatePM
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

        public bool IsLastQuoteTemplateDocumentVersion { get; set; }
        public bool IsCopiedAtSignup { get; set; }
        public bool IsEnabledForCustomers { get; set; }





        private List<QuoteTemplateSectionPM> templateSections;
        [Include]
        [Association("QuoteTemplateQuoteTemplateSection", "Id", "QuoteTemplateId")]
        public virtual List<QuoteTemplateSectionPM> TemplateSections
        {
            get
            {
                if (templateSections == null)
                {
                    templateSections = new List<QuoteTemplateSectionPM>();
                }

                return this.templateSections;
            }
            set
            {
                if (value != null)
                {
                    templateSections = value;
                }
            }
        }






    }
}