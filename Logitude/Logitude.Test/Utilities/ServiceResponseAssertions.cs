using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.Test.Utilities
{
    /// <summary>
    /// Provides reflection-based assertions for service response objects without forcing a concrete dependency.
    /// </summary>
    public static class ServiceResponseAssertions
    {
        /// <summary>
        /// Asserts that a response object represents success (HasError == false and no errors collection).
        /// </summary>
        public static void AssertSuccess(object response, string because = null)
        {
            Assert.IsNotNull(response, because ?? "Expected service response to be populated.");

            var hasError = ReadBooleanProperty(response, "HasError");
            if (hasError.HasValue)
            {
                Assert.IsFalse(hasError.Value, because ?? "Expected service response to be successful.");
            }

            var errors = ReadErrors(response);
            if (errors != null && errors.Any())
            {
                Assert.Fail(because ?? $"Expected service response to contain no errors, but received: {string.Join("; ", errors)}");
            }
        }

        /// <summary>
        /// Asserts that a response object represents failure and optionally validates the error payload.
        /// </summary>
        public static void AssertFailure(object response, params string[] expectedErrorFragments)
        {
            Assert.IsNotNull(response, "Expected service response to be populated.");

            var hasError = ReadBooleanProperty(response, "HasError");
            if (hasError.HasValue)
            {
                Assert.IsTrue(hasError.Value, "Expected HasError to be true for a failing response.");
            }

            var errors = ReadErrors(response)?.ToArray() ?? Array.Empty<string>();
            if (errors.Length == 0)
            {
                Assert.Fail("Expected failure response to contain errors, but none were found.");
            }

            if (expectedErrorFragments == null || expectedErrorFragments.Length == 0)
            {
                return;
            }

            foreach (var fragment in expectedErrorFragments.Where(f => !string.IsNullOrWhiteSpace(f)))
            {
                var match = errors.Any(error =>
                    error?.IndexOf(fragment, StringComparison.OrdinalIgnoreCase) >= 0);

                Assert.IsTrue(match,
                    $"Expected error containing '{fragment}', but errors were: {string.Join("; ", errors)}");
            }
        }

        private static bool? ReadBooleanProperty(object instance, string propertyName)
        {
            var property = instance.GetType().GetProperty(propertyName);
            if (property == null || property.PropertyType != typeof(bool) && property.PropertyType != typeof(bool?))
            {
                return null;
            }

            var value = property.GetValue(instance);
            return value == null ? (bool?)null : Convert.ToBoolean(value);
        }

        private static IEnumerable ReadErrorsRaw(object instance)
        {
            var candidates = new[] { "Errors", "ErrorsArray", "ErrorMessages", "Messages" };
            foreach (var candidate in candidates)
            {
                var property = instance.GetType().GetProperty(candidate);
                if (property == null)
                {
                    continue;
                }

                var value = property.GetValue(instance);
                if (value is IEnumerable enumerable)
                {
                    return enumerable;
                }
            }

            return null;
        }

        private static IEnumerable<string> ReadErrors(object instance)
        {
            var raw = ReadErrorsRaw(instance);
            if (raw == null)
            {
                return Enumerable.Empty<string>();
            }

            return raw.Cast<object>()
                .Where(item => item != null)
                .Select(item => item.ToString());
        }
    }
}

