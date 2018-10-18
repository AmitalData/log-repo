using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Server.Tools;
using FakeItEasy;
using Logitude.Server.Tools;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Server.Tools.Counters;

namespace Logitude.UnitTest.Utils
{
    public class LogitudeServerToolsFactory
    {
        

        internal IIdCounter RegisterIIdCounter(Action<IIdCounter> init)
        {
            var fake = A.Fake<IIdCounter>();
            ContainerAccessor.Container.RegisterInstance<IIdCounter>(fake);
            init(fake);
            return fake;
        }

        
        
    }
}
