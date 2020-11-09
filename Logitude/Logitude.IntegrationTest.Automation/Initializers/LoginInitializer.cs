using System;
using System.Threading.Tasks;
using Logitude.IntegrationTest.Automation.EntitiesInitializer;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)] //0 means use as many workers as possible

namespace Logitude.IntegrationTest.Automation.Initializers
{
    [TestClass]
    public class LoginInitializer
    {
        [AssemblyInitialize]
        public static async Task AssemblyInitialize(TestContext context)
        {
            await LoginService.GetLoginTokenByUserEmailAndTenant();
            //ShipmentInitializer shipmentInitializer = new ShipmentInitializer();
            //shipmentInitializer.CreateShipment();
        }
    }
}
