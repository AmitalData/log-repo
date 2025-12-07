using FakeItEasy;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Logitude.Accounting.BL.EntityQueryServices;

namespace Logitude.UnitTest.Utils
{
    public  class FakeFactory
    {

        public I Register<I>(Action<I> InitAction) where I : class
        {
            var fake = A.Fake<I>();
            ContainerAccessor.Container.RegisterInstance<I>(fake);
            if (InitAction!=null)
            {
                InitAction(fake);    
            }
            return fake;
        }
    }

    
}
