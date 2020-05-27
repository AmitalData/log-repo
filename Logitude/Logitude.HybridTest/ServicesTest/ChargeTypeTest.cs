using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class ChargeTypeTest
    {
        [TestMethod]
        public void Test_ChargeType_GetChargesTypes()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "ChargeType",
                ServiceOperation = "GetChargesTypes",
                ServiceResponseIndex = 3,
                ServiceType = typeof(ChargesTypeList),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, 0, 10, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            ChargesTypeList[] chargesTypes = (ChargesTypeList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Charge Types Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Charge Types Failed! " + serviceOutcome.Response.ErrorMessage);
            if (chargesTypes.Length == 0)
                Assert.Inconclusive("There Isn't Charge Types!");
        }
    }
}
