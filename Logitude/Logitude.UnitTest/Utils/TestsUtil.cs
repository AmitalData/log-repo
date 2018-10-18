using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.UnitTest.Utils
{
    public static class TestsUtil
    {
        public static void AssertThrows<exception>(Action method, string expectedContainsMessage = null, string lineErrorIs = null)
                                    where exception : Exception
        {
            lineErrorIs = lineErrorIs ?? "";
            if (string.IsNullOrWhiteSpace(lineErrorIs))
            {
                lineErrorIs = "lineErrorIs:" + lineErrorIs + " ";
            }
            try
            {
                method.Invoke();
            }
            catch (exception mytypeexception)
            {
                if (!String.IsNullOrWhiteSpace(expectedContainsMessage))
                {
                    if (!mytypeexception.Message.ToLower().Contains(expectedContainsMessage.ToLower()))
                    {
                        Assert.AreEqual(expectedContainsMessage, mytypeexception.Message, lineErrorIs + "Exception.Message not contains ");
                    }
                }
                return; // Expected exception.
            }
            catch (Exception ex)
            {
                Assert.Fail(lineErrorIs + "Wrong exception thrown: " + ex.Message);
            }
            Assert.Fail(lineErrorIs + "No exception thrown");
        }
    }
}
