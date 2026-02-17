using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Interfaces;
using Logitude.BL.Mocks;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Resolvers
{
    public class LoggedContactResolver
    {
        public LoggedContactResolver()
        {
            
        }

        public static void RegisterMockLoggedContactUtil()
        {
            ContainerAccessor.Container.RegisterType<ILoggedContactUtil, MockLoggedContactUtil>("LoggedContactUtil", new InjectionFactory(c => new MockLoggedContactUtil()));
        }
        public static void RegisterLoggedContactUtil()
        {
            ContainerAccessor.Container.RegisterType<ILoggedContactUtil, Logitude.BL.Security.LoggedContactUtil>("LoggedContactUtil", new InjectionFactory(c => new Logitude.BL.Security.LoggedContactUtil()));
        }

        public static ContactPM GetLoggedContact(int tenant)
        {
            ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);
            return loggedcontact;
        }

        public static bool GetLoggedContactShowLocal(int tenant)
        {
            ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            bool showLocal = !(bool)loggedcontact?.DontShowLocal;
            return showLocal;
        }
    }
}
