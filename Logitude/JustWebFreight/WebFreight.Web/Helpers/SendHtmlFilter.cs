using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class SendHtmlFilter
    {
      
        public string Htmlstring { get; set; }
        public string HtmlPlainString { get; set; }
        public string InternalDocumentId { get; set; }
        public string ExternalDocumentId { get; set; }
        public int Tenant { get; set; }
        public string ToEmail { get; set; }

        public string Subject { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string UserId { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public string ChildObjectTableId { get; set; }
        public string ChildEntityId { get; set; }
        public string Attachments { get; set; }
        public string EntityReference { get; set; }
        public bool ExportQuotationsoIntegratedSys { get; set; }
        public string ObjectTableName { get; set; }

        public string CustomerId { get; set; }
        public string EventTypeCode { get; set; }
        public bool IsCRM { get; set; }
        public string DocumentTypeCode { get; set; }
        public string DocumentTypeId { get; set; }
        public string HeaderHtml { get; set; }
        public string FooterHtml { get; set; }

        public int HeaderHeight { get; set; }
        public int FooterHeight { get; set; }


        public string From { get; set; }
        public string ReplyTo { get; set; }

    }
}