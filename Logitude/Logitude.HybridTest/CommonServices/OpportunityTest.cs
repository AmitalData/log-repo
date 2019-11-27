using System;
using Logitude.HybridTest.OpportunityServiceReference;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class OpportunityTest
    {

        [TestMethod]
        public void Test_Opportunity_GetOpportunityList()
        {
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
            object[] serviceParameters = new object[] { "hybrid@fnarsoft.com", "", TestEnvironmentGlobalParameters.Tenant, 0, 10, filters, serviceResponse };
            OpportunityList[] opportunities = (OpportunityList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            if (opportunities.Length == 0)
                Assert.Inconclusive("There Isn't Opportunity With This Searchfield!");
            else
                HybridData.SomeOpportunityId = opportunities[0].Id;
        }

        [TestMethod]
        public void Test_Opportunity_GetCustomerListByOpportunityId()
        {
            if (HybridData.SomeOpportunityId == null)
                Test_Opportunity_GetOpportunityList();
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
            object[] serviceParameters = new object[] { HybridData.SomeOpportunityId, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            CustomerList customer = (CustomerList)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Customer List By Opportunity Id Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Customer List By Opportunity Id Failed! " + serviceResponse.Result);
            if (customer == null)
                Assert.Inconclusive("There Isn't customer With This Opportunity Id!");
        }

        [TestMethod]
        public void Test_Opportunity_GetOpportunityListById()
        {
            Assert.Inconclusive("Missing Service Method!");
            if (HybridData.SomeOpportunityId == null)
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
            object[] serviceParameters = new object[] { HybridData.SomeOpportunityId, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            OpportunityList opportunity = (OpportunityList)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Opportunity List By Id Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Opportunity List By Id Failed! " + serviceResponse.Result);
            if (opportunity == null)
                Assert.Inconclusive("There Isn't Opportunity With This Id!");
        }
    }
}
