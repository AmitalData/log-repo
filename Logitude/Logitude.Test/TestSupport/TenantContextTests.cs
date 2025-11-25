using System;

using Logitude.Test.TestSupport;
using Logitude.Test.Utilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;

namespace Logitude.Test.TestSupport
{
    [TestClass]
    public class TenantContextTests
    {
        [TestMethod]
        [Timeout(10000)] // 10 second timeout to prevent hangs
        public void FreezeClock_WhenApplied_ControlsTenantServerConfiguration()
        {
            var anchor = new DateTime(2024, 4, 15, 13, 45, 30, DateTimeKind.Utc);

            using (TenantContext.FreezeClock(anchor))
            {
                var todayStart = TenantServerConfigration.GetStartOfTodayDate(TenantContext.DefaultTenantId);
                Assert.AreEqual(anchor.Date, todayStart);
            }
        }

        [TestMethod]
        [Timeout(10000)] // 10 second timeout to prevent hangs
        public void OverrideGetContext_WithDelegate_CallsFactoryInsteadOfDefaultResolution()
        {
            int capturedTenant = 0;

            using (WebFreightContext.OverrideGetContext(tenant =>
                   {
                       capturedTenant = tenant;
                       throw new ExpectedContextAccessException();
                   }))
            {
                Assert.ThrowsException<ExpectedContextAccessException>(() =>
                    WebFreightContext.GetContext(TenantContext.DefaultTenantId));
            }

            Assert.AreEqual(TenantContext.DefaultTenantId, capturedTenant);
        }

        [TestMethod]
        [Timeout(10000)] // 10 second timeout to prevent hangs
        public void ClockEchoService_UsesFrozenClockAndReportsSuccess()
        {
            var anchor = new DateTime(2024, 7, 1, 9, 30, 0, DateTimeKind.Utc);

            using (TenantContext.FreezeClock(anchor))
            {
                var service = new ClockEchoService();
                var response = service.Execute(TenantContext.DefaultTenantId);

                ServiceResponseAssertions.AssertSuccess(response);
                Assert.AreEqual(anchor, response.TimestampUtc);
                Assert.AreEqual(TenantContext.DefaultTenantId, response.TenantId);
            }
        }

        private sealed class ClockEchoService
        {
            public ClockEchoResponse Execute(int tenantId)
            {
                return new ClockEchoResponse
                {
                    TenantId = tenantId,
                    TimestampUtc = TenantServerConfigration.GetCurrentDateTime(tenantId),
                    HasError = false,
                    ErrorsArray = Array.Empty<string>()
                };
            }
        }

        private sealed class ClockEchoResponse
        {
            public int TenantId { get; set; }
            public DateTime TimestampUtc { get; set; }
            public bool HasError { get; set; }
            public string[] ErrorsArray { get; set; }
        }

        private sealed class ExpectedContextAccessException : Exception
        {
        }
    }
}










