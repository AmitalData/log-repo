using FluentAssertions;
using Logitude.ReportTests.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.ReportTests.Services
{
    public class ReportAssertService : ReportDataAssertService
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
            StimulReportResult stimulReportResult = APICaller.CallPut<StimulReportResult>(reportFilter, Urls.ReportController, UserTenant.Token)?.Data;
            stimulReportResult.Should().NotBeNull();
            stimulReportResult.StimulImageBase64.Should().NotBeNull();
            Initialize(stimulReportResult.DataProvider);
        }

    }
}
