using System;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Resolvers;
using Logitude.Test.Infrastructure.Container;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.Test.Infrastructure
{
    /// <summary>
    /// MSTest-compatible base class for unit tests.
    /// Provides isolated container scope and resolver registration.
    /// Note: Inheriting test classes must have [TestClass] attribute.
    /// </summary>
    public abstract class TestBase_MSTest
    {
        private TestContainerScope _containerScope;

        /// <summary>
        /// Gets the test container scope for this test class.
        /// </summary>
        protected TestContainerScope ContainerScope => _containerScope;

        /// <summary>
        /// Initializes the test with isolated container and resolver registration.
        /// </summary>
        [TestInitialize]
        public virtual void InitializeTests()
        {
            // Create isolated container scope
            _containerScope = new TestContainerScope();

            // Register mock resolvers in the isolated container
            // Note: These methods use ContainerAccessor.Container, which is now set to our isolated container
            LoggedContactResolver.RegisterMockLoggedContactUtil();
            DateTimeUtilResolver.RegisterMockDateTimeUtil();
            TranslateTextsClassUtilResolver.RegisterMockTranslateTextsClassUtil();
            IdCounterUtilResolver.RegisterMockIdCounterUtil();
        }

        /// <summary>
        /// Cleans up the test container scope.
        /// </summary>
        [TestCleanup]
        public virtual void CleanupTests()
        {
            _containerScope?.Dispose();
            _containerScope = null;
        }
    }
}

