using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.Helpers
{
    public sealed class InjectionContainer
    {
        private static readonly InjectionContainer instance = new InjectionContainer();
        private static readonly IUnityContainer _container = new UnityContainer();

        private InjectionContainer()
        {

        }

        public InjectionContainer Instance { get { return instance; } }

        public static IUnityContainer Container
        {
            get
            {
                return _container;
            }
        }
      
    }
}
