using FluentAssertions;
using Logitude.ReportTests.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;

namespace Logitude.ReportTests.Services
{
    public class ReportAssertService<T> : ReportDataAssertService<T> where T : BaseDataProvider
    {
        public void Assert(ReportFliter reportFilter)
        {
            reportFilter.Should().NotBeNull();
            reportFilter.ReportKey.Should().NotBeNull();
            AssertReportExecutionLog(reportFilter);
            AssertStimulReportResult(reportFilter);
        }

        private void AssertReportExecutionLog(ReportFliter reportFilter)
        {
            var tryEvreySecound = 4;// add it to 
            var timeLifeInSecound = 60 * 3; // add it to 
            var seuccess = Waiter.RunAndWait(tryEvreySecound, timeLifeInSecound, () => GetExecutionLogResult(reportFilter.Tenant, reportFilter.ReportKey));
            seuccess.Should().BeTrue();
        }

        private bool GetExecutionLogResult(int tenant, string reportKey)
        {
            var reportReult = APICaller.CallGet<ReportBuildResult>(Urls.GetCheckIfStimulSoftReportIsBliud(reportKey, tenant), UserTenant.Token)?.Data;
            if (reportReult == null) return false;
            if (reportReult.HasError) throw new InvalidOperationException(reportReult.ExceptionMessage);
            if (reportReult.StatusCode != "D") return false;
            return true;
        }

        private void AssertStimulReportResult(ReportFliter reportFilter)
        {
            reportFilter.ProcessType = "ReportsRunUsingWR";
            StimulReportResult<T> stimulReportResult = APICaller.CallPut<StimulReportResult<T>>(reportFilter, Urls.ReportController, UserTenant.Token)?.Data;
            stimulReportResult.Should().NotBeNull();
            stimulReportResult.StimulImageBase64.Should().NotBeNull();
            Initialize(stimulReportResult.DataProvider);
        }
    }
}
