using System;
using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class CustomerAdditionalServiceDWTest
    {
        [TestMethod]
        public void Test_CustomerAdditionalServiceDW_GetCustomerAdditionalServices()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "CustomerAdditionalServiceDW",
                ServiceOperation = "GetCustomerAdditionalServices",
                ServiceResponseIndex = 1,
                ServiceType = typeof(CustomerAdditionalServiceDW),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            CustomerAdditionalServiceDW[] customerAdditionalServices = (CustomerAdditionalServiceDW[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Customer Additional Services Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Customer Additional Services Failed! " + serviceResponse.ErrorMessage);
            if(customerAdditionalServices.Length == 0)
                Assert.Inconclusive("There Isn't Customer Additional Services!");
        }
    }
}
