using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.Helpers.DataProviderHelpers
{
    public class AutomationTestReportDataProvider: BaseDataProvider
    {
        public string TenantName { get; set; }
        public string PackageName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Notes { get; set; }

    }
}