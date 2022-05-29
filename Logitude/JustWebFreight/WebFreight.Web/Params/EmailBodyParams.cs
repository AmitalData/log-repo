using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Params
{
    public class EmailBodyParams
    {
        public EmailMessageParams EmailMessageParams { get; set; }
        public string PagePath { get; set; }
        public string Email { get; set; }
    }
}