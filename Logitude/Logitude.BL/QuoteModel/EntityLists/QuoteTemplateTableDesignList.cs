using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.QuoteModel.EntityLists
{
    public class QuoteTemplateTableDesignList
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string BorderTypeCode { get; set; }
        public string BorderColor { get; set; }
        public int BorderThickness { get; set; }


        public string HeaderDesignId { get; set; }
        public string LinesDesignId { get; set; }
        public string GroupByDesignId { get; set; }
    }
}