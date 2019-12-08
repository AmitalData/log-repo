using System;
using Logitude.CRM.BL.EntityDws;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class OpportunityDWTest
    {

        [TestMethod]
        public void Test_OpportunityDW_GetOpportunitiesByDates()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "OpportunityDW",
                ServiceOperation = "GetOpportunitiesByDates",
                ServiceResponseIndex = 5,
                ServiceType = typeof(OpportunityDW),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, DateTime.Today.AddYears(-1), DateTime.Now, 0, 10, serviceResponse };
            OpportunityDW[] opportunities = (OpportunityDW[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Opportunities By Dates Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Opportunities By Dates Failed! " + serviceResponse.Result);
            if(opportunities.Length == 0)
                Assert.Inconclusive("There Isn't Opportunity From Last Year!");
        }

        [TestMethod]
        public void Test_OpportunityDW_GetOpportunitiesCountByDates()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "OpportunityDW",
                ServiceOperation = "GetOpportunitiesCountByDates",
                ServiceResponseIndex = 3,
                ServiceType = typeof(OpportunityDW),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, DateTime.Today.AddYears(-1), DateTime.Now, serviceResponse };
            int opportunitiescount = (int)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Opportunities Count By Dates Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Opportunities Count By Dates Failed! " + serviceResponse.Result);
            if (opportunitiescount == 0)
                Assert.Inconclusive("There Isn't Opportunity From Last Year!");
        }

        [TestMethod]
        public void Test_OpportunityDW_GetOpportunitiesByUpdateDate()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "OpportunityDW",
                ServiceOperation = "GetOpportunitiesByUpdateDate",
                ServiceResponseIndex = 4,
                ServiceType = typeof(OpportunityDW),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, DateTime.Today.AddYears(-1), 0, 10, serviceResponse };
            OpportunityDW[] opportunities = (OpportunityDW[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Opportunities By Update Date Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Opportunities By Update Date Failed! " + serviceResponse.Result);
            if (opportunities.Length == 0)
                Assert.Inconclusive("There Isn't Opportunity From Last Year!");
        }

        [TestMethod]
        public void Test_OpportunityDW_GetOpportunitiesCountByUpdateDate()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "OpportunityDW",
                ServiceOperation = "GetOpportunitiesCountByUpdateDate",
                ServiceResponseIndex = 2,
                ServiceType = typeof(OpportunityDW),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, DateTime.Today.AddYears(-1), serviceResponse };
            int opportunitiescount = (int)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Opportunities Count By Update Date Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Opportunities Count By Update Date Failed! " + serviceResponse.Result);
            if (opportunitiescount == 0)
                Assert.Inconclusive("There Isn't Opportunity From Last Year!");
        }
    }
}
