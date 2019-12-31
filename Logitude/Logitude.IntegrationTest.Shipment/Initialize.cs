using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipmen
{
    [TestClass]
    public class Initialize
    {
        [AssemblyInitialize]
        public static async Task AssemblyInitialize(TestContext context)
        {
            await LoginService.GetLoginTokenByUserEmailAndTenant();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Console.WriteLine("AssemblyCleanup");
        }
    }
}


