using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class TranslationHeader
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }


        //[Include]
        //[Association("TranslationTranslationHeader", "Code", "TranslationHeaderCode")]
        //public virtual List<Translation> Translations { get; set; }

    }
}