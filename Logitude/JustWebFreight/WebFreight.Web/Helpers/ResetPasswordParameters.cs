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
        public bool IsChampLogin { get; set; }
        public bool IsMobile { get; set; }
        public string ClientType { get; set; }
        public string CaptchaKey { get; set; }
        public string CaptchaCode { get; set; }
        public string PageName { get; set; }
        public string AppEnvironment { get; set; }
        public string BrandingTenant { get; set; }
        public string Domain { get; set; }
        public string TemplateName { get; set; }
        public string DocumentTypeCode { get; set; }
        public bool IsCargoTracking { get; set; }

    }
}