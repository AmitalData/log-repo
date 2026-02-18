using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class Translation
    {
        [Key]
        public string Id { get; set; }
        public string TranslationHeaderCode { get; set; }
        public string TextCodeId { get; set; }
        public string TranslatedText { get; set; }
        public string TranslatedTextPlural { get; set; }
        public int Tenant { get; set; }
        public DateTime? TranslateDate { get; set; }
        public string TranslatedByUserId { get; set; }
        public DateTime? UpdateDateGMT { get; set; }
        public string TextCodeCode { get; set; }

        [ForeignKey("TranslatedByUserId")]
        public virtual User TranslatedByUser { get; set; }

        //[Include]
        //[Association("TranslationTranslationHeader", "TranslationHeaderCode", "Code", IsForeignKey = true)]
        [ForeignKey("TranslationHeaderCode")]
        public virtual TranslationHeader TranslationHeader { get; set; }
        //[Include]
        //[Association("TextCodeTranslation", "TextCodeId", "Id", IsForeignKey = true)]
        [ForeignKey("TextCodeId")]
        public virtual TextCode TextCode { get; set; }

    }
}