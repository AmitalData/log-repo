using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.QuoteModel.EntityLists
{
    public class QuoteTemplateTextCodeList
    {


        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string TextCode { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string OriginalEnglishName { get; set; }
        public string OriginalLocalName { get; set; }



        public string QuoteTemplateId { get; set; }
        public string Area { get; set; }
        
    }
}