using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class HtmlTemplateClass
    {

        public string HeaderHtml { get; set; }
        public string FooterHtml { get; set; }
        public string BodyHtml { get; set; }
        public int HeaderHeight { get; set; }
        public int FooterHeight { get; set; }

    }
}