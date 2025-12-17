using System;
using System.Collections.Generic;
using FakeItEasy;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.UnitTest.Utils
{
    /// <summary>
    /// Test-only environment fakes to avoid hitting real DB/global context.
    /// </summary>
    public static class EnvFakes
    {
        private static bool _initialized;

        /// <summary>
        /// Call once per test init to ensure GlobalDbHelper and CacheManager are stubbed.
        /// </summary>
        public static void EnsureInitialized()
        {
            if (_initialized) return;

            var stubGlobalDb = new GlobalDB();

            var fakeCache = A.Fake<ICacheWrapper>();
            // Any call returning object yields the stub DB, others default
            A.CallTo(fakeCache).WithReturnType<object>().Returns(stubGlobalDb);
            CacheManager.CacheWrapper = fakeCache;

            _initialized = true;
        }

    }
}

