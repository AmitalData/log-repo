using System;
using System.Globalization;

using Simplog.Data.Helpers;

namespace Logitude.Test.TestSupport
{
    public static class TenantContext
    {
        public const int DefaultTenantId = 9999;
        public const string DefaultUserId = "UNIT-TEST-USER";
        public const string DefaultBranchId = "UNIT-TEST-BRANCH";

        public static string NewEntityId(string prefix = "UT")
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Prefix must be provided.", nameof(prefix));
            }

            var guid = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            return $"{prefix}-{guid.Substring(0, 9)}";
        }

        public static TenantClockOverride OverrideClock(Func<int, DateTime> clock)
        {
            return TenantClockOverride.Create(clock);
        }

        public static TenantClockOverride FreezeClock(DateTime? now = null)
        {
            return TenantClockOverride.Create(_ => now ?? DateTime.UtcNow);
        }
    }

    public sealed class TenantClockOverride : IDisposable
    {
        private readonly IDisposable _scope;
        private bool _disposed;

        private TenantClockOverride(IDisposable scope)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        internal static TenantClockOverride Create(Func<int, DateTime> clock)
        {
            if (clock == null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

            return new TenantClockOverride(TenantServerConfigration.OverrideGetCurrentDateTime(clock));
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _scope.Dispose();
            _disposed = true;
        }
    }
}


