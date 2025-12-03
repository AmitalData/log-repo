using System;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
// Note: MSTest Fakes types are generated at build time from .fakes files
#if FAKES_SUPPORTED
using Microsoft.QualityTools.Testing.Fakes;
using Simplog.Data.InfrastructureModel.Repositories.Fakes;
#endif

namespace Logitude.Test.Infrastructure.Factories
{
    /// <summary>
    /// MSTest-compatible version of SimplogDataFakeFactory.
    /// Creates fakes for Simplog.Data interfaces using MSTest fakes.
    /// </summary>
    public class SimplogDataFakeFactory_MSTest
    {
        /// <summary>
        /// Returns an action to configure an IObjectTableRepository fake.
        /// Note: MSTest fakes must be created using generated Fakes types.
        /// </summary>
        public Action<IObjectTableRepository> IObjectTableRepository_ReturnObjectTable_Id1
        {
            get
            {
#if FAKES_SUPPORTED
                // MSTest fakes approach:
                // 1. Create a shim for IObjectTableRepository using the generated Fakes type
                // 2. Configure the shim's methods to return the desired ObjectTable
                
                // Example (requires Simplog.Data.fakes):
                // return (fake) =>
                // {
                //     using (ShimsContext.Create())
                //     {
                //         var shim = fake as Fakes.ShimIObjectTableRepository;
                //         shim.GetObjectTableByIdStringInt32 = (string id, int tenant) => 
                //             new ObjectTable() { Id = "journal" };
                //     }
                // };
                
                return (fake) =>
                {
                    // Placeholder - will be implemented when .fakes files are configured
                    throw new NotImplementedException(
                        "MSTest fakes support requires Simplog.Data.fakes to be configured and built.");
                };
#else
                return (fake) =>
                {
                    throw new NotImplementedException(
                        "MSTest fakes support requires .fakes files to be configured and built.");
                };
#endif
            }
        }
    }
}

