using Logitude.Server.Tools.Mocks;
using Logitude.Server.Tools.Utils;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Resolvers
{
    public class DateTimeUtilResolver
    {
        public DateTimeUtilResolver()
        {
        }

        public static void RegisterMockDateTimeUtil()
        {
            ContainerAccessor.Container.RegisterType<IDateTimeUtil, MockDateTimeUtil>("DateTimeUtil", new InjectionFactory(c => new MockDateTimeUtil()));
        }

        public static void RegisterDateTimeUtil()
        {
            ContainerAccessor.Container.RegisterType<IDateTimeUtil, DateTimeUtil>("DateTimeUtil", new InjectionFactory(c => new DateTimeUtil()));
        }

        public static DateTime GetDateCurrentDateTime(int tenant)
        {
            IDateTimeUtil currentDateTimeUtil = ContainerAccessor.Container.Resolve(typeof(IDateTimeUtil), "DateTimeUtil", new ParameterOverride("", tenant)) as IDateTimeUtil;
            DateTime currentDateTime = currentDateTimeUtil.GetCurrentDateTime(tenant);
            return currentDateTime;
        }
    }
}
