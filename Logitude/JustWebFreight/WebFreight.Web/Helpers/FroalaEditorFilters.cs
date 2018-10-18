using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class FroalaEditorFilters
    {


        public int Tenant { get; set; }
        public string DocumentTypeCopyId { get; set; }
        public string DocumentOutId { get; set; }
        public string HtmlString { get; set; }

        public string DocumentTemplateId { get; set; }
        public int HeaderHeight { get; set; }
        public int FooterHeight { get; set; }
        public string DocumentTypeId { get; set; }
        public string EntityId { get; set; }
        public string ChildEntityId { get; set; }
    }
}