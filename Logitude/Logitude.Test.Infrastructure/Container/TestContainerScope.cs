using System;
using System.Reflection;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools;

namespace Logitude.Test.Infrastructure.Container
{
    /// <summary>
    /// Provides isolated Unity container scope for unit tests.
    /// Ensures each test class has its own container instance, preventing state pollution.
    /// Uses reflection to set the private static _container field in ContainerAccessor.
    /// </summary>
    public class TestContainerScope : IDisposable
    {
        private readonly IUnityContainer _originalContainer;
        private readonly IUnityContainer _testContainer;
        private static readonly FieldInfo _containerField;
        private bool _disposed = false;

        static TestContainerScope()
        {
            // Get the private static _container field using reflection
            _containerField = typeof(ContainerAccessor).GetField(
                "_container",
                BindingFlags.NonPublic | BindingFlags.Static);
            
            if (_containerField == null)
            {
                throw new InvalidOperationException(
                    "Could not find _container field in ContainerAccessor. " +
                    "The implementation may have changed.");
            }
        }

        /// <summary>
        /// Initializes a new instance of TestContainerScope.
        /// Creates an isolated container and replaces the global ContainerAccessor.Container.
        /// </summary>
        public TestContainerScope()
        {
            _originalContainer = ContainerAccessor.Container;
            _testContainer = new UnityContainer();
            
            // Set the private static field using reflection
            _containerField.SetValue(null, _testContainer);
        }

        /// <summary>
        /// Gets the isolated test container instance.
        /// </summary>
        public IUnityContainer Container => _testContainer;

        /// <summary>
        /// Restores the original container and disposes the test container.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                // Restore the original container
                _containerField.SetValue(null, _originalContainer);
                _testContainer?.Dispose();
                _disposed = true;
            }
        }
    }
}

