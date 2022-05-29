using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Params
{
    public class EmailCommunicationLogBuilderArgs
    {
        public MessageArgs Result { get; set; }
        public ResetPasswordParameters ResetPasswordParameters { get; set; }
        public string AppMobileEnvironment { get; set; }
    }
}