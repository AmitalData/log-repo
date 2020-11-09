using System;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.IntegrationTest.Automation.Tests.Shipments
{
    [TestClass]
    public class OnCreate_Email
    {
        [TestMethod]
        public void TestMethod1()
        {
            string testToken = IntegrationTestLoginParameters.Token;
            Console.WriteLine(testToken);
        }
    }
}
