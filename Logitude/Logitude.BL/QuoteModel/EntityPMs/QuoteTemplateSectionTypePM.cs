using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    public class QuoteTemplateSectionTypePM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    
    }
}