using System;
using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class CustomerCompetitorDWTest
    {
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Test_CustomerCompetitorDW_GetCustomerCompetitors()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "CustomerCompetitorDW",
                    ServiceOperation = "GetCustomerCompetitors",
                    ServiceResponseIndex = 1,
                    ServiceType = typeof(CustomerCompetitorDW),
                    ServiceFilterType = null,
                };
                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, serviceResponse };
                ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

                CustomerCompetitorDW[] customerAdditionalServices = (CustomerCompetitorDW[])serviceOutcome.Result;
                Assert.IsFalse(serviceOutcome.Response.HasError, "Get Tenant Managements Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNull(serviceOutcome.Response.Result, "Get Tenant Managements Failed! " + serviceOutcome.Response.ErrorMessage);
                //if (customerAdditionalServices.Length == 0)
                    //Assert.Inconclusive("There Isn't Competitiors!");
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }
    }
}
