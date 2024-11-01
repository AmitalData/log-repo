using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using AmitalCloud.Infrastructure.Data.Mocks;
using AmitalCloud.Infrastructure.Data.Security;
using Microsoft.Practices.Unity;

namespace AmitalCloud.Infrastructure.Data.Helpers
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
            ContainerAccessor.Container.RegisterType<ILoggedContactUtil, LoggedContactUtil>("LoggedContactUtil", new InjectionFactory(c => new LoggedContactUtil()));
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
