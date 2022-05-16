using Logitude.ReportTests.Models;
using Logitude.ReportTests.Services;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using TechTalk.SpecFlow;
using Logitude.ReportTests.Services.Mapps.Mappers;
using Logitude.ReportTests.Models.DataProviders;

namespace Logitude.ReportTests.Steps
{
    [Binding]
    public class OpenShipmentsByCustomerReportSteps
    {
        private readonly ReportFilterService<OpenShipmentsByCustomerReportFilterMapper> reportFilterService;
        private readonly ReportContext reportContext;
        private readonly ReportAssertService<OpenShipmentsByCustomerDataProvider> reportAssertService;

        public OpenShipmentsByCustomerReportSteps(ReportFilterService<OpenShipmentsByCustomerReportFilterMapper> reportFilterService, ReportContext reportContext, ReportAssertService<OpenShipmentsByCustomerDataProvider> reportAssertService)
        {
            this.reportFilterService = reportFilterService;
            this.reportContext = reportContext;
            this.reportAssertService = reportAssertService;
        }

        [Given(@"report with the following properties")]
        public void GivenReportWithTheFollowingProperties(Table table)
        {
            reportContext.ReportFilter = reportFilterService.Create(table);
        }

        [Given(@"filter fields")]
        public void GivenFilterFields(Table table)
        {
            reportContext.ReportFilter.QueryFilterItemLists = reportFilterService.BuildFilterItems(table);
        }

        [When(@"run report")]
        public void WhenRunReport()
        {
            reportContext.ReportFilter = APICaller.CallPut<ReportFliter>(reportContext.ReportFilter, Urls.ReportController, UserTenant.Token)?.Data;
        }

        [Then(@"the report should run successfully")]
        public void ThenTheReportShouldRunSuccessfully()
        {
            reportAssertService.Assert(reportContext.ReportFilter);
        }

        [Then(@"with values")]
        public void ThenWithValues(Table table)
        {
            // reportAssertService.Initialize(new OpenShipmentsByCustomerDataProvider().GetTestData());
            reportAssertService.AssertFields(table);
        }
    }
}
