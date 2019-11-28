using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
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
            object[] serviceParameters = new object[] { TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            HybridPartnerList[] customerAdditionalServices = (HybridPartnerList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Customer Additional Services Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Mislaka Partners Failed! " + serviceResponse.ErrorMessage);
            if (customerAdditionalServices.Length == 0)
                Assert.Inconclusive("There Isn't Mislaka Partners!");
        }
    }
}
