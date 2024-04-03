using System;

namespace Logitude.ReportTests.Models
{
    public class StimulReportResult<T> where T : BaseDataProvider
    {
        public int PageCount { get; set; }
        public string ReportKey { get; set; }
        public string StimulImageBase64 { get; set; }
        public T DataProvider { get; set; }
    }
}
