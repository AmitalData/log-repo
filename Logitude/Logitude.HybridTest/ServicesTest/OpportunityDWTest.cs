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
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            OpportunityDW[] opportunities = (OpportunityDW[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Opportunities By Dates Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Opportunities By Dates Failed! " + serviceOutcome.Response.Result);
            if (opportunities.Length == 0)
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
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            int opportunitiescount = (int)serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Opportunities Count By Dates Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Opportunities Count By Dates Failed! " + serviceOutcome.Response.Result);
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
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            OpportunityDW[] opportunities = (OpportunityDW[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Opportunities By Update Date Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Opportunities By Update Date Failed! " + serviceOutcome.Response.Result);
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
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            int opportunitiescount = (int)serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Opportunities Count By Update Date Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Opportunities Count By Update Date Failed! " + serviceOutcome.Response.Result);
            if (opportunitiescount == 0)
                Assert.Inconclusive("There Isn't Opportunity From Last Year!");
        }
    }
}
