using System;

namespace Simplog.Server.Infrastructure.Helpers
{
    public class MockServiceProvider:IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            return null;
        }
    }
}