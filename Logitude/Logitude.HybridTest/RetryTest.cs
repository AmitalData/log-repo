using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest
{
    public class RetryTest
    {
        public static Dictionary<string, int> AllTests = new Dictionary<string, int>();
        public static void InsertTestMethodToDictionary(string testName)
        {
            if (!AllTests.ContainsKey(testName))
            {
                AllTests.Add(testName, 0);
            }
        }
        public static void RetryFailTestRun(TestContext testContext, object testInstance, string exceptionMessage)
        {
            Type type = testInstance.GetType();
            int retryNumber = AllTests[testContext.TestName];
            if (retryNumber < 3)
            {
                AllTests[testContext.TestName] = retryNumber + 1;
                if (type != null)
                {
                    MethodInfo testMethodInfo = type.GetMethod(testContext.TestName);
                    try
                    {
                        testMethodInfo.Invoke(testInstance, null);
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail(ex.InnerException?.Message);
                    }
                }
            }
            else Assert.Fail(exceptionMessage);
        }
    }
}
