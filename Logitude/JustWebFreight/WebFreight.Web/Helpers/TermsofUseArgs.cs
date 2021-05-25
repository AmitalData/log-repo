using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class TermsofUseArgs
    {
        public bool IsTermOfUse { get; set; }
        public int VersionNumber { get; set; }
        public string VersionDocumentId { get; set; }
        public int Id { get; set; }

    }
}