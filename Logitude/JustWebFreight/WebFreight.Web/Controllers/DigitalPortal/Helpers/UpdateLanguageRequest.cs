using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Controllers.DigitalPortal.Helpers
{
    public class UpdateLanguageRequest
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DigitalPortalLanguage { get; set; }
    }
}