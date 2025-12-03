using System;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Test.Infrastructure.Container;
using Microsoft.Practices.Unity;
// Note: MSTest Fakes types are generated at build time from .fakes files
#if FAKES_SUPPORTED
using Microsoft.QualityTools.Testing.Fakes;
using Logitude.Server.Tools.Counters.Fakes;
#endif

namespace Logitude.Test.Infrastructure.Factories
{
    /// <summary>
    /// MSTest-compatible version of LogitudeServerToolsFactory.
    /// Creates fakes for Logitude.Server.Tools interfaces using MSTest fakes.
    /// </summary>
    public class LogitudeServerToolsFactory_MSTest
    {
        private readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance with the specified container.
        /// </summary>
        public LogitudeServerToolsFactory_MSTest(IUnityContainer container = null)
        {
            _container = container ?? ContainerAccessor.Container;
        }

        /// <summary>
        /// Registers an IIdCounter fake in the container.
        /// Note: MSTest fakes must be created using generated Fakes types.
        /// </summary>
        internal IIdCounter RegisterIIdCounter(Action<IIdCounter> init)
        {
#if FAKES_SUPPORTED
            // MSTest fakes approach:
            // 1. Create a shim for IIdCounter using the generated Fakes type
            // 2. Configure the shim's methods
            // 3. Register the shim instance in the container
            
            // Example (requires Logitude.Server.Tools.fakes):
            // using (ShimsContext.Create())
            // {
            //     var shim = new Fakes.ShimIIdCounter();
            //     shim.GetNumberStringInt32 = (string key, int tenant) => "1";
            //     init?.Invoke(shim.Instance);
            //     _container.RegisterInstance<IIdCounter>(shim.Instance);
            //     return shim.Instance;
            // }
            
            throw new NotImplementedException(
                "MSTest fakes support requires Logitude.Server.Tools.fakes to be configured and built. " +
                "This will be implemented when .fakes files are properly set up.");
#else
            throw new NotImplementedException(
                "MSTest fakes support requires .fakes files to be configured and built.");
#endif
        }
    }
}

