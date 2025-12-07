using System;
using System.Reflection;
using Simplog.Data.Helpers;
// Note: MSTest Fakes types are generated at build time from .fakes files
// The Fakes namespace will be available after the first build with .fakes files
#if FAKES_SUPPORTED
using Microsoft.QualityTools.Testing.Fakes;
using Simplog.Data.Helpers.Fakes;
#endif

namespace Logitude.Test.Infrastructure.Utilities
{
    /// <summary>
    /// MSTest-compatible version of TenantFixture.
    /// Provides common tenant-aware defaults and helpers for backend unit tests.
    /// </summary>
    public static class TenantFixture_MSTest
    {
        public const int DefaultTenantId = 9999;
        public const string DefaultUserId = "UNIT-TEST-USER";
        public const string DefaultBranchId = "UNIT-TEST-BRANCH";

        /// <summary>
        /// Creates a predictable unique identifier for use in tests.
        /// </summary>
        public static string NewEntityId(string prefix = "UT")
        {
            return $"{prefix}-{Guid.NewGuid():N}".Substring(0, prefix.Length + 9);
        }

        /// <summary>
        /// Attaches a tenant/branch identity to an entity PM or POCO using reflection.
        /// Properties named Tenant, TenantId, BranchId, or Branch will be populated when writable.
        /// </summary>
        public static T WithTenant<T>(T entity, int tenant = DefaultTenantId, string branchId = DefaultBranchId)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var type = entity.GetType();

            SetPropertyIfWritable(type, entity, "Tenant", tenant);
            SetPropertyIfWritable(type, entity, "TenantId", tenant);
            SetPropertyIfWritable(type, entity, "BranchId", branchId, onlyWhenNullOrEmpty: true);
            SetPropertyIfWritable(type, entity, "Branch", branchId, onlyWhenNullOrEmpty: true);

            return entity;
        }

        /// <summary>
        /// Overrides <see cref="TenantServerConfigration.GetCurrentDateTime(int)"/> for deterministic tests using MSTest fakes.
        /// Must be called within a ShimsContext.
        /// Note: Requires Simplog.Data.Helpers.fakes file to be configured.
        /// </summary>
        public static void ArrangeTenantClock(Func<int, DateTime> clock)
        {
            if (clock == null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

#if FAKES_SUPPORTED
            using (ShimsContext.Create())
            {
                Fakes.ShimTenantServerConfigration.GetCurrentDateTimeInt32 = (int tenant) => clock(tenant);
            }
#else
            throw new NotImplementedException(
                "MSTest fakes support requires .fakes files to be configured and built. " +
                "Add Simplog.Data.Helpers.fakes to the project and rebuild.");
#endif
        }

        /// <summary>
        /// Uses a fixed point in time for all tenants (defaults to UTC now).
        /// Must be called within a ShimsContext.
        /// </summary>
        public static void ArrangeTenantClock(DateTime? now = null)
        {
            var anchor = now ?? DateTime.UtcNow;
            ArrangeTenantClock(_ => anchor);
        }

        private static void SetPropertyIfWritable(Type type, object instance, string propertyName, object value, bool onlyWhenNullOrEmpty = false)
        {
            var property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            if (property == null || !property.CanWrite)
            {
                return;
            }

            if (onlyWhenNullOrEmpty)
            {
                var currentValue = property.GetValue(instance);
                if (currentValue is string currentString)
                {
                    if (!string.IsNullOrWhiteSpace(currentString))
                    {
                        return;
                    }
                }
                else if (currentValue != null)
                {
                    return;
                }
            }

            property.SetValue(instance, value);
        }
    }
}

