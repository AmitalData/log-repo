using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class MessageArgs
    {
        public string Subject { get; set; }
        public string From { get; set; }
        public string ReplyTo { get; set; }
        public string HtmlTemplate { get; set; }

        public string CC { get; set; }
        public string BCC { get; set; }


    }
}