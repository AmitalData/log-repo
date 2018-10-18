using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
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