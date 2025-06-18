using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using Microsoft.Practices.Unity;

namespace AmitalCloud.Infrastructure.Data.Helpers
{
    public class LoggedContactResolver
    {
        public LoggedContactResolver()
        {

        }
        public static ContactPM GetLoggedContact(int tenant)
        {
            ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);
            return loggedcontact;
        }

        public static void RegisterLoggedContactUtil()
        {
            ContainerAccessor.Container.RegisterType<ILoggedContactUtil, LoggedContactUtil>("LoggedContactUtil", new InjectionFactory(c => new LoggedContactUtil()));
        }
    }
}
