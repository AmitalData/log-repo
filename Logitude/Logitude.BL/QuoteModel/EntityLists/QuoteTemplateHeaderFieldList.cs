using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.QuoteModel.EntityLists
{
    public class QuoteTemplateHeaderFieldList
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }

        public string FieldCode { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }

        public string QuoteTemplateId { get; set; }
    }
}