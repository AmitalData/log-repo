using System;
using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class CustomerCompetitorDWTest
    {
        [TestMethod]
        public void Test_CustomerCompetitorDW_GetCustomerCompetitors()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "CustomerCompetitorDW",
                ServiceOperation = "GetCustomerCompetitors",
                ServiceResponseIndex = 1,
                ServiceType = typeof(CustomerCompetitorDW),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            CustomerCompetitorDW[] customerAdditionalServices = (CustomerCompetitorDW[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Tenant Managements Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Tenant Managements Failed! " + serviceResponse.ErrorMessage);
            if (customerAdditionalServices.Length == 0)
                Assert.Inconclusive("There Isn't Competitiors!");
        }
    }
}
