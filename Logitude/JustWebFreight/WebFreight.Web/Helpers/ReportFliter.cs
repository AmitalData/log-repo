using Logitude.CustomsMessaging.Common.RequestParams;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class ReportFliter
    {
        public string CurrentCurrencyCodeType { get; set; }
        public string DateType { get; set; }
        public bool IncludeOperationalyClosed { get; set; }
        public string ReportCode { get; set; }
        public string FilterControlName { get; set; }
        public int tenant { get; set; }
        public string ReportDocumentId { get; set; }
        public string CustomerId { get; set; }
        public string QuoteCustomerTypeCode { get; set; }
        public List<QueryFilterItem> QueryFilterItemLists { get; set; }
        public int NumberOfPage { get; set; }
        public string ProcessType { get; set; }
        public string ReportName { get; set; }
        public string ReportKey { get; set; }
        public string DefaultTemplateId { get; set; }
        public int DefaultTemplateVsersion { get; set; }
        public int PageCount { get; set; }
        public bool ReportsRunUsingWR { get; set; }
        public string UserId { get; set; }
        public string ReportId { get; set; }
        public int NumberOfRequests { get; set; }
        public bool DisablePreview { get; set; }
        public bool IsSchedulerReport { get; set; }
        public bool SendIfEmpty{ get; set; }
        public string FieldDataType { get; set; }
        public string InvoiceType { get; set; }
        public string Level { get; set; }

    }
    public class ReportTemplateEditorHtmlDataParams
    {
        [JsonProperty("ReportsTemplateId")]
        public string ReportsTemplateId { get; set; }

        [JsonProperty("Version")]
        public int? Version { get; set; }

        [JsonProperty("UserId")]
        public string UserId { get; set; }

        [JsonProperty("Subject")]
        public string Subject { get; set; }

        [JsonProperty("From")]
        public string From { get; set; }

        [JsonProperty("ReplyTo")]
        public string ReplyTo { get; set; }

        [JsonProperty("Cc")]
        public string Cc { get; set; }

        [JsonProperty("ReportFilter")]
        public ReportFliter ReportFilter { get; set; } // Note the correct property name
    }
  //  public class ReportFliter

}