using FluentAssertions;
using Logitude.ReportTests.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;

namespace Logitude.ReportTests.Services
{
    public class ReportAssertService
    {
        public void Assert(ReportFliter reportFilter)
        {
            reportFilter.Should().NotBeNull();
            reportFilter.ReportKey.Should().NotBeNull();
            AssertReport(reportFilter);
        }

        private void AssertReport(ReportFliter reportFilter)
        {
            var tryEvreySecound = 4;
            var timeLifeInSecound = 60 * 3;
            var isDone = Waiter.RunAndWait(tryEvreySecound, timeLifeInSecound, () => AssertStimulSoftReportIsBliud(reportFilter.Tenant, reportFilter.ReportKey));
            isDone.Should().BeTrue();
            AssertBuildStimulReportResult(reportFilter);
        }

        private bool AssertStimulSoftReportIsBliud(int tenant, string reportKey)
        {
            var reportReult = APICaller.CallGet<ReportBuildResult>(Urls.GetCheckIfStimulSoftReportIsBliud(reportKey, tenant), UserTenant.Token)?.Data;
            if (reportReult == null) return false;
            if (reportReult.HasError) throw new InvalidOperationException(reportReult.ExceptionMessage);
            if (reportReult.StatusCode != "D") return false;
            return true;
        }

        private void AssertBuildStimulReportResult(ReportFliter reportFilter)
        {
            reportFilter.ProcessType = "ReportsRunUsingWR";
            StimulReportResult stimulReportResult = APICaller.CallPut<StimulReportResult>(reportFilter, Urls.ReportController, UserTenant.Token)?.Data;
            stimulReportResult.Should().NotBeNull();
            stimulReportResult.StimulImageBase64.Should().NotBeNull();
        }
    }
}
