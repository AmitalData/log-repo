using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.HybridTest.OpportunityServiceReference;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class OpportunityTest
    {
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Test_Opportunity_GetOpportunityList()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "Opportunity",
                    ServiceOperation = "GetOpportunityList",
                    ServiceResponseIndex = 6,
                    ServiceType = typeof(OpportunityList),
                    ServiceFilterType = typeof(OpportunityApiFilters),
                };
                OpportunityApiFilters filters = new OpportunityApiFilters();
                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { "hybrid@fnarsoft.com", "", EnvironmentGlobalParams.MainTenant, 0, 10, filters, serviceResponse };
                ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

                OpportunityList[] opportunities = (OpportunityList[])serviceOutcome.Result;
                Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.Result);
                if (opportunities.Length != 0)
                    //Assert.Inconclusive("There Isn't Opportunity With This Searchfield!");
                //else
                    HybridData.FirstOpportunityId = opportunities[0].Id;
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }

        [TestMethod]
        public void Test_Opportunity_GetCustomerListByOpportunityId()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                //if (HybridData.FirstOpportunityId == null)
                    //Test_Opportunity_GetOpportunityList();
                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "Opportunity",
                    ServiceOperation = "GetCustomerListByOpportunityId",
                    ServiceResponseIndex = 2,
                    ServiceType = typeof(CustomerList),
                    ServiceFilterType = null,
                };
                OpportunityApiFilters filters = new OpportunityApiFilters();
                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { HybridData.FirstOpportunityId, EnvironmentGlobalParams.MainTenant, serviceResponse };
                ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

                CustomerList customer = (CustomerList)serviceOutcome.Result;
                Assert.IsFalse(serviceOutcome.Response.HasError, "Get Customer List By Opportunity Id Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNull(serviceOutcome.Response.Result, "Get Customer List By Opportunity Id Failed! " + serviceOutcome.Response.Result);
                //if (customer == null)
                    //Assert.Inconclusive("There Isn't customer With This Opportunity Id!");
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }

        [TestMethod]
        public void Test_Opportunity_GetOpportunityListById()
        {
            Assert.Inconclusive("Missing Service Method!");
            if (HybridData.FirstOpportunityId == null)
                Test_Opportunity_GetOpportunityList();
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Opportunity",
                ServiceOperation = "GetOpportunityListById",
                ServiceResponseIndex = 2,
                ServiceType = typeof(OpportunityList),
                ServiceFilterType = null,
            };
            OpportunityApiFilters filters = new OpportunityApiFilters();
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.FirstOpportunityId, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            OpportunityList opportunity = (OpportunityList)serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Opportunity List By Id Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Opportunity List By Id Failed! " + serviceOutcome.Response.Result);
            //if (opportunity == null)
                //Assert.Inconclusive("There Isn't Opportunity With This Id!");
        }
    }
}
