using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools.Mocks;
using Logitude.Server.Tools.Utils;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Resolvers
{
    public class IdCounterUtilResolver
    {
        public IdCounterUtilResolver()
        {
        }

        public static void RegisterMockIdCounterUtil()
        {
            ContainerAccessor.Container.RegisterType<IIdCounterUtil, MockIdCounterUtil>("IdCounterUtil", new InjectionFactory(c => new MockIdCounterUtil()));
        }

        public static void RegisterIdCounterUtil()
        {
            ContainerAccessor.Container.RegisterType<IIdCounterUtil, IdCounterUtil>("IdCounterUtil", new InjectionFactory(c => new IdCounterUtil()));
        }

        public static string GetNewIdCounter(string tableName,int tenant)
        {
            IIdCounterUtil idCounterUtil = ContainerAccessor.Container.Resolve(typeof(IIdCounterUtil), "IdCounterUtil", new ParameterOverride("", tenant)) as IIdCounterUtil;
            string idCounter = idCounterUtil.GetNumber(tableName, tenant);
            return idCounter;
        }
    }
}
