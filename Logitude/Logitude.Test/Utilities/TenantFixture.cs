using System;
using System.Reflection;
using Simplog.Data.Helpers;
using Telerik.JustMock;

namespace Logitude.Test.Utilities
{
    /// <summary>
    /// Provides common tenant-aware defaults and helpers for backend unit tests.
    /// </summary>
    public static class TenantFixture
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
        /// Overrides <see cref="TenantServerConfigration.GetCurrentDateTime(int)"/> for deterministic tests.
        /// </summary>
        public static void ArrangeTenantClock(Func<int, DateTime> clock)
        {
            if (clock == null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>()))
                .Returns((int tenant) => clock(tenant));
        }

        /// <summary>
        /// Uses a fixed point in time for all tenants (defaults to UTC now).
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

