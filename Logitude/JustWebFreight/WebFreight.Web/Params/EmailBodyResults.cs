using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Params
{
    public class EmailBodyResults
    {
        public StringBuilder HtmlTemplate { get; set; }
        public MessageArgs Result { get; set; }
    }
}