using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class DocumentTypeTemplateFilter
    {
       public List<EditableFieldPosition> EditableFieldLists { get; set; }

        public int PageIndex { get; set; }
        public string  Id { get; set; }
        public string Body { get; set; }
        public int Tenant { get; set; }
        public string TemplateType { get; set; }
        public bool InActive { get; set; }
        public string DocumentOutId { get; set; }
        public string Processtype { get; set; }
        public string Subject { get; set; }
        public DocumentTypeTemplatePM TemplatePM { get; set; }
        public bool IsUpdate { get; set; }
        public string ReportKey { get; set; }
        


    }
}