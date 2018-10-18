using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
   public class QuoteTemplateDetailsField
    {

        [Key]
        public string Id { get; set; }
        
        public int Tenant { get; set; }

        public string FieldCode { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }

        public string QuoteTemplateId { get; set; }

        [ForeignKey("QuoteTemplateId")]
        public virtual QuoteTemplate QuoteTemplate { get; set; }


    }
}
