using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class HybridPartnerTest
    {
        [TestMethod]
        public void Test_HybridPartner_GetMislakaPartners()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "HybridPartner",
                ServiceOperation = "GetMislakaPartners",
                ServiceResponseIndex = 1,
                ServiceType = typeof(HybridPartnerList),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            HybridPartnerList[] customerAdditionalServices = (HybridPartnerList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Customer Additional Services Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Mislaka Partners Failed! " + serviceOutcome.Response.ErrorMessage);
            if (customerAdditionalServices.Length == 0)
                Assert.Inconclusive("There Isn't Mislaka Partners!");
        }
    }
}
