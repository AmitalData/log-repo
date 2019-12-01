using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.FullAccounting
{
    [TestClass]
    public class Initialize
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            Task.Run(async () =>
            {
                string email = "angular@fnarsoft.com";
                string pass = "1";
                var token = await LoginService.GetLoginTokenByUserEmailAndTenant(email, pass);
                Assert.IsNotNull(token);

            }).GetAwaiter().GetResult();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Console.WriteLine("AssemblyCleanup");
        }
    }
}
