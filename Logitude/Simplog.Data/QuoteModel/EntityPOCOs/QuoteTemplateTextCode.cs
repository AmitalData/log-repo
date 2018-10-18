using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
   public class QuoteTemplateTextCode
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string TextCode { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string QuoteTemplateId { get; set; }
        public string Area { get; set; }
        public string OriginalEnglishName { get; set; }
        public string OriginalLocalName { get; set; }


        [ForeignKey("QuoteTemplateId")]
        public virtual QuoteTemplate QuoteTemplate { get; set; }

    }
}
