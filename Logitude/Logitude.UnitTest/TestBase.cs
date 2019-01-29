using System;
using System.Collections.Generic;
using FakeItEasy;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.UnitTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;
using Logitude.Server.Tools;
using Logitude.BL.Interfaces;
using Logitude.BL.Security;
using Microsoft.Practices.Unity;

namespace Logitude.UnitTest
{
    [TestClass]
    public class TestBase
    {
        [TestInitialize]
        public void InitializeTests()
        {
            ContainerAccessor.Container.RegisterType<ILoggedContactUtil, MockLoggedContactUtil>("LoggedContactUtil", new InjectionFactory(c => new MockLoggedContactUtil()));
        }
    }
}
