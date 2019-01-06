using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class WebHookTesterData
    {
        public string URL { get; set; }
        public string Operation { get; set; }
        public string AccessKey { get; set; }
        public string ContentToPush { get; set; } 
    }
}