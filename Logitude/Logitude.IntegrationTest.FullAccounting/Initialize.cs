using Logitude.IntegrationTest.Core.Login;
using Logitude.IntegrationTest.FullAccounting.Preparation;
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
                await LoginService.GetLoginTokenByUserEmailAndTenant();
                await PreparationCalls.PrepareVariables();

            }).GetAwaiter().GetResult();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Console.WriteLine("AssemblyCleanup");
        }
    }
}
