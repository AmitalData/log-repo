using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Logitude.IntegrationTest.FullAccounting;
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
        public async Task AssemblyInitialize(TestContext context)
        {
                await LoginService.GetLoginTokenByUserEmailAndTenant();
                await CorePreparationCalls.PrepareVariables();
                await FullAccountingPreparationCalls.PrepareVariables();
         }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Console.WriteLine("AssemblyCleanup");
        }
    }
}
