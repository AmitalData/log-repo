using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class ResetPasswordParameters
    {
         
        public string RequestNumber { get; set; }
        public string NewPassword { get; set; }

        public string OldPassword { get; set; }
        public bool IsResetRequest { get; set; }

        public string Email { get; set; }
        public string MobilePageType { get; set; }
        public string VerificationCode { get; set; }

        
    }
}