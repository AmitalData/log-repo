using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.FieldShortNameGetters;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class InfraRegistrationHelper
    {
        public static void Register()
        {
            InjectionContainer.Container.RegisterType<IObjectTablePropertyGetter, ObjectTablePropertyGetter>("ObjectTablePropertyGetter", new InjectionFactory(c => new ObjectTablePropertyGetter()));
            InjectionContainer.Container.RegisterType<FieldShortNameGetter, ObjectFieldsShortNamesGetter>("ObjectFieldsShortNamesGetter", new InjectionFactory(c => new ObjectFieldsShortNamesGetter()));
            InjectionContainer.Container.RegisterType<IObjectFieldPropertyGetter, ObjectFieldPropertyGetter>("ObjectFieldPropertyGetter", new InjectionFactory(c => new ObjectFieldPropertyGetter()));
        }
    }
}
