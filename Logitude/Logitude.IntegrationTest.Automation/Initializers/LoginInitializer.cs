using System;
using System.Threading.Tasks;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)] //0 means use as many workers as possible

namespace Logitude.IntegrationTest.Automation.Initializers
{
    [TestClass]
    public class LoginInitializer
    {
        [AssemblyInitialize]
        public static async Task PrepareAutomationTest(TestContext context)
        {
            string email = IntegrationTestLoginParameters.Email;
            await LoginService.GetLoginTokenByUserEmailAndTenant();
            string testToken = IntegrationTestLoginParameters.Token;
        }
    }
}
