using System;
using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class CustomerAdditionalServiceDWTest
    {
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Test_CustomerAdditionalServiceDW_GetCustomerAdditionalServices()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "CustomerAdditionalServiceDW",
                    ServiceOperation = "GetCustomerAdditionalServices",
                    ServiceResponseIndex = 1,
                    ServiceType = typeof(CustomerAdditionalServiceDW),
                    ServiceFilterType = null,
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, serviceResponse };
                ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

                CustomerAdditionalServiceDW[] customerAdditionalServices = (CustomerAdditionalServiceDW[])serviceOutcome.Result;
                Assert.IsFalse(serviceOutcome.Response.HasError, "Get Customer Additional Services Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNull(serviceOutcome.Response.Result, "Get Customer Additional Services Failed! " + serviceOutcome.Response.ErrorMessage);
                //if (customerAdditionalServices.Length == 0)
                    //Assert.Inconclusive("There Isn't Customer Additional Services!");
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }
    }
}
