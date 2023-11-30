using Logitude.BL.QuoteModel.DataContracts;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using WebFreight.Web.ReportsWebServices.LogitudeReports.Quots.SpotRate;
using WebFreight.Web.Services;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Quotes.SpotRate
{
    public class SpotRateQuoteReportDataProviderService
    {
        private readonly SpotRateQuoteReportFilter spotRateQuoteReportFilter;
        private readonly SpotRateQuoteReportService spotRateQuoteReportService;
        private int tenant;
        public SpotRateQuoteReportDataProviderService(byte[] filters, int tenant)
        {
            spotRateQuoteReportFilter = new SpotRateQuoteReportFilterService(filters, tenant).Build();
            spotRateQuoteReportService = new SpotRateQuoteReportService(tenant, spotRateQuoteReportFilter);
            this.tenant = tenant;
        }

        public byte[] Load()
        {
            SpotRateQuoteReportDataProvider spotRateQuoteReportDataProvider = BuildDataProvider();
            return new ReportMemoryStreamService().Convert(spotRateQuoteReportDataProvider, typeof(SpotRateQuoteReportDataProvider), tenant);
        }

        private SpotRateQuoteReportDataProvider BuildDataProvider()
        {
            SpotRateQuoteReportDataProvider spotRateQuoteReportDataProvider = spotRateQuoteReportService.Build();
            return spotRateQuoteReportDataProvider;
        }
    }
}