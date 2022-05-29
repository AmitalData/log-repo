using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Params
{
    public class EmailParameters
    {
        public string Subject { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}