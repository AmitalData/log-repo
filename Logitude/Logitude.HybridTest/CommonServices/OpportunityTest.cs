using System;
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
            LoginService.GetLoginTokenByCredentials();
            OpportunityServiceReference.OpportunityWcfServiceClient serviceClient = new OpportunityServiceReference.OpportunityWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                OpportunityServiceReference.OpportunityApiFilters filters = new OpportunityServiceReference.OpportunityApiFilters();
                OpportunityServiceReference.OpportunityList[] entityList = serviceClient.GetOpportunityList("hybrid@fnarsoft.com", "",TestEnvironmentGlobalParameters.Tenant, 0, 10, filters, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Opportunity List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Opportunity List Failed! " + serviceResponse.ErrorMessage);
                if (entityList.Length != 0)
                {
                    //
                }
                else
                {
                    Assert.Inconclusive("There Isn't Opportunity With This Searchfield!");
                }
            }
        }

        [TestMethod]
        public void Test_Opportunity_GetCustomerListByOpportunityId()
        {
            LoginService.GetLoginTokenByCredentials();
            OpportunityServiceReference.OpportunityWcfServiceClient serviceClient = new OpportunityServiceReference.OpportunityWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                OpportunityServiceReference.CustomerList entityPM = serviceClient.GetCustomerListByOpportunityId("?", TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Customer List By Opportunity Id Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Customer List By Opportunity Id Failed! " + serviceResponse.ErrorMessage);
                if (entityPM != null)
                {
                    //
                }
                else
                {
                    Assert.Inconclusive("There Isn't Opportunity With This Id!");
                }
            }
        }

        [TestMethod]
        public void Test_Opportunity_GetOpportunityListById()
        {
            Assert.Inconclusive("!");
            LoginService.GetLoginTokenByCredentials();
            OpportunityServiceReference.OpportunityWcfServiceClient serviceClient = new OpportunityServiceReference.OpportunityWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                //OpportunityServiceReference.OpportunityList entityPM = serviceClient.GetOpportunityListById("?", TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Opportunity List By Id Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Opportunity List By Id Failed! " + serviceResponse.ErrorMessage);
                //if (entityPM != null)
                //{
                //    //
                //}
                //else
                {
                    Assert.Inconclusive("There Isn't Opportunity From Last Year!");
                }
            }
        }
    }
}
