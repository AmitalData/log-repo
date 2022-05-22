using Logitude.BL.QuoteModel.DataContracts;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using WebFreight.Web.ReportsWebServices.LogitudeReports.Quots.SpotRate;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Quotes.SpotRate
{
    public class SpotRateQuoteReportDataProviderService
    {
        private readonly SpotRateQuoteReportFilter spotRateQuoteReportFilter;
        private readonly SpotRateQuoteReportService spotRateQuoteReportService;

        public SpotRateQuoteReportDataProviderService(byte[] filters, int tenant)
        {
            spotRateQuoteReportFilter = new SpotRateQuoteReportFilterService(filters, tenant).Build();
            spotRateQuoteReportService = new SpotRateQuoteReportService(tenant, spotRateQuoteReportFilter);
        }

        public byte[] Load()
        {
            SpotRateQuoteReportDataProvider spotRateQuoteReportDataProvider = BuildDataProvider();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SpotRateQuoteReportDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, spotRateQuoteReportDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            byte[] bytearray = memoryStream.ToArray();
            //File.WriteAllText(@"D:\path.json", JsonConvert.SerializeObject(spotRateQuoteReportDataProvider));
            return bytearray;
        }

        private SpotRateQuoteReportDataProvider BuildDataProvider()
        {
            SpotRateQuoteReportDataProvider spotRateQuoteReportDataProvider = spotRateQuoteReportService.Build();
            return spotRateQuoteReportDataProvider;
        }
    }
}