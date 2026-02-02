using System;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
// Note: MSTest Fakes types are generated at build time from .fakes files
#if FAKES_SUPPORTED
using Microsoft.QualityTools.Testing.Fakes;
#endif

namespace Logitude.Test.Infrastructure.Factories
{
    /// <summary>
    /// MSTest-compatible version of FakeFactory.
    /// Creates fakes using MSTest fakes and registers them in Unity container.
    /// </summary>
    public class FakeFactory_MSTest
    {
        private readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance with the specified container.
        /// </summary>
        public FakeFactory_MSTest(IUnityContainer container = null)
        {
            _container = container ?? ContainerAccessor.Container;
        }

        /// <summary>
        /// Registers a fake instance in the container.
        /// Note: MSTest fakes must be created using generated Fakes types.
        /// This method accepts a pre-created fake instance.
        /// </summary>
        public I Register<I>(I fakeInstance, Action<I> initAction = null) where I : class
        {
            if (fakeInstance == null)
            {
                throw new ArgumentNullException(nameof(fakeInstance));
            }

            _container.RegisterInstance<I>(fakeInstance);
            
            if (initAction != null)
            {
                initAction(fakeInstance);
            }
            
            return fakeInstance;
        }

        /// <summary>
        /// Creates and registers a fake using MSTest fakes.
        /// Note: This requires the assembly to have a .fakes file and generated Fakes namespace.
        /// For interfaces, use: new Fakes.ShimIYourInterface()
        /// </summary>
        public I RegisterFake<I>(Action<I> initAction = null) where I : class
        {
            // MSTest fakes are generated at compile time
            // The actual implementation depends on the generated Fakes types
            // This is a placeholder - tests should create fakes directly using Fakes namespace
            
            throw new NotImplementedException(
                "MSTest fakes must be created using generated Fakes types. " +
                "Use the Fakes namespace (e.g., new Fakes.ShimIYourInterface()) and call Register() with the instance.");
        }
    }
}

