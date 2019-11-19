using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class OpportunityDWTest
    {

        [TestMethod]
        public void Test_OpportunityDW_GetOpportunitiesByDates()
        {
            LoginService.GetLoginTokenByCredentials();
            OpportunityDWServiceReference.OpportunityDWWcfServiceClient serviceClient = new OpportunityDWServiceReference.OpportunityDWWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                OpportunityDWServiceReference.OpportunityDW[] entityList = serviceClient.GetOpportunitiesByDates(TestEnvironmentGlobalParameters.Tenant, DateTime.Today.AddYears(-1), DateTime.Now, 0, 10, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Opportunities By Dates Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Opportunities By Dates Failed! " + serviceResponse.ErrorMessage);
                if (entityList.Length != 0)
                {
                    //
                }
                else
                {
                    Assert.Inconclusive("There Isn't Opportunity From Last Year!");
                }
            }
        }

        [TestMethod]
        public void Test_OpportunityDW_GetOpportunitiesCountByDates()
        {
            LoginService.GetLoginTokenByCredentials();
            OpportunityDWServiceReference.OpportunityDWWcfServiceClient serviceClient = new OpportunityDWServiceReference.OpportunityDWWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                int opportunityCount = serviceClient.GetOpportunitiesCountByDates(TestEnvironmentGlobalParameters.Tenant, DateTime.Today.AddYears(-1), DateTime.Now, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Opportunities Count By Dates Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Opportunities Count By Dates Failed! " + serviceResponse.ErrorMessage);
                if (opportunityCount == 0)
                {
                    Assert.Inconclusive("There Isn't Opportunity From Last Year!");
                }
            }
        }

        [TestMethod]
        public void Test_OpportunityDW_GetOpportunitiesByUpdateDate()
        {
            LoginService.GetLoginTokenByCredentials();
            OpportunityDWServiceReference.OpportunityDWWcfServiceClient serviceClient = new OpportunityDWServiceReference.OpportunityDWWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                OpportunityDWServiceReference.OpportunityDW[] entityList = serviceClient.GetOpportunitiesByUpdateDate(TestEnvironmentGlobalParameters.Tenant, DateTime.Today.AddYears(-1), 0, 10, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Opportunities By Update Date Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Opportunities By Update Date Failed! " + serviceResponse.ErrorMessage);
                if (entityList.Length != 0)
                {
                    //
                }
                else
                {
                    Assert.Inconclusive("There Isn't Opportunity From Last Year!");
                }
            }
        }

        [TestMethod]
        public void Test_OpportunityDW_GetOpportunitiesCountByUpdateDate()
        {
            LoginService.GetLoginTokenByCredentials();
            OpportunityDWServiceReference.OpportunityDWWcfServiceClient serviceClient = new OpportunityDWServiceReference.OpportunityDWWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                int opportunityCount = serviceClient.GetOpportunitiesCountByUpdateDate(TestEnvironmentGlobalParameters.Tenant, DateTime.Today.AddYears(-1), ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Opportunities Count By Update Date Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Opportunities Count By Update Date Failed! " + serviceResponse.ErrorMessage);
                if (opportunityCount == 0)
                {
                    Assert.Inconclusive("There Isn't Opportunity From Last Year!");
                }
            }
        }
    }
}
