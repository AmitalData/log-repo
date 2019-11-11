using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class QuoteTest
    {
        [TestMethod]
        public void Test_Quote_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Server.Tools.Response branchServiceResponse = BranchTest.CallBranchUpsert();
            Assert.IsFalse(branchServiceResponse.HasError, "Branch Upsert Failed! " + branchServiceResponse.ErrorMessage);
            Assert.IsNotNull(branchServiceResponse.Result, "Branch Upsert Failed! " + branchServiceResponse.ErrorMessage);
            Server.Tools.Response departmentServiceResponse = DepartmentTest.CallDepartmentUpsert();
            Assert.IsFalse(departmentServiceResponse.HasError, "Departmnet Upsert Failed! " + departmentServiceResponse.ErrorMessage);
            Assert.IsNotNull(departmentServiceResponse.Result, "Department Upsert Failed! " + departmentServiceResponse.ErrorMessage);
            Server.Tools.Response agentServiceResponse = AgentTest.CallAgentUpsert();
            Assert.IsFalse(agentServiceResponse.HasError, "Agent Upsert Failed! " + agentServiceResponse.ErrorMessage);
            Assert.IsNotNull(agentServiceResponse.Result, "Agent Upsert Failed! " + agentServiceResponse.ErrorMessage);
            Server.Tools.Response portServiceResponse = PortTest.CallPortUpsert();
            Assert.IsFalse(portServiceResponse.HasError, "Port Upsert Failed! " + portServiceResponse.ErrorMessage);
            Assert.IsNotNull(portServiceResponse.Result, "Port Upsert Failed! " + portServiceResponse.ErrorMessage);
            Server.Tools.Response userServiceResponse = UserTest.CallUserUpsert();
            Assert.IsFalse(userServiceResponse.HasError, "User Upsert Failed! " + userServiceResponse.ErrorMessage);
            Assert.IsNotNull(userServiceResponse.Result, "User Upsert Failed! " + userServiceResponse.ErrorMessage);
            Server.Tools.Response serviceResponse = CallQuoteUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        public static Server.Tools.Response CallQuoteUpsert()
        {

            QuoteServiceReference.QuoteWcfServiceClient serviceClient = new QuoteServiceReference.QuoteWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                QuoteServiceReference.QuotePM entityPM = new QuoteServiceReference.QuotePM()
                {
                    QuoteNumber = "Hybrid Quote",
                    TransportModeId = "A",
                    DirectionId = "I",
                    BranchId = HybridCodes.BranchCode,
                    DepartmentId = HybridCodes.DepartmentCode,
                    CustomerId = HybridCodes.AgentCode,
                    FromPortId = HybridCodes.PortCode,
                    ToPortId = HybridCodes.PortCode,
                    QuoteTypeCode = "A",
                    ExchangeRate = 1,
                    CreatedByUserId = HybridCodes.UserCode,
                    UpdatedByUserId = HybridCodes.UserCode,
                    OpenDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    QuoteCustomerTypeCode = "CON",
                    SaleCurrencyId = HybridCodes.CurrencyCode,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                Logitude.Server.Tools.Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
