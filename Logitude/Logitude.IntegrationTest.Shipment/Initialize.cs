using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Logitude.IntegrationTest.Shipment;
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
            await ShipmentPreperationCalls.PrepareVariables();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Console.WriteLine("AssemblyCleanup");
        }
    }
}


