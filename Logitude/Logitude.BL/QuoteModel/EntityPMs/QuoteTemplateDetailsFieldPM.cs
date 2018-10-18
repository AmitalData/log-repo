using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    public class QuoteTemplateDetailsFieldPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string FieldCode { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public string QuoteTemplateId { get; set; }

        public bool IsAdd { get; set; }
        public bool IsDelete { get; set; }
        public bool IsEdit { get; set; }
    }
}