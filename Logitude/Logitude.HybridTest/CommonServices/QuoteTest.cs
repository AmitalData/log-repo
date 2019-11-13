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
            Server.Tools.Response fromPortServiceResponse = PortTest.CallFromPortUpsert();
            Assert.IsFalse(fromPortServiceResponse.HasError, "From Port Upsert Failed! " + fromPortServiceResponse.ErrorMessage);
            Assert.IsNotNull(fromPortServiceResponse.Result, "From Port Upsert Failed! " + fromPortServiceResponse.ErrorMessage);
            Server.Tools.Response toPortServiceResponse = PortTest.CallToPortUpsert();
            Assert.IsFalse(toPortServiceResponse.HasError, "To Port Upsert Failed! " + toPortServiceResponse.ErrorMessage);
            Assert.IsNotNull(toPortServiceResponse.Result, "To Port Upsert Failed! " + toPortServiceResponse.ErrorMessage);
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
                    TransportModeId = "A", //A:Air, O:Occean, I:Inland
                    DirectionId = "I", //I:Import, E:Export, D:Drop, R:Drop, C:Customs Import
                    BranchId = HybridCodes.BranchCode,
                    DepartmentId = HybridCodes.DepartmentCode,
                    CustomerId = HybridCodes.AgentCode,
                    FromPortId = HybridCodes.FromPortCode,
                    ToPortId = HybridCodes.ToPortCode,
                    QuoteTypeCode = "A", //A:Spot Rate, P:Routing Rate
                    ExchangeRate = 1,
                    CreatedByUserId = HybridCodes.UserCode,
                    UpdatedByUserId = HybridCodes.UserCode,
                    OpenDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    QuoteCustomerTypeCode = "CON", //CON:Consignee, AGT:Agent, SHI:Shipper, NOT:Notify, OTH:Other
                    SaleCurrencyId = HybridCodes.CurrencyCode,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                    BusinessUnitId = TestEnvironmentGlobalParameters.Tenant.ToString(),
                };

                Logitude.Server.Tools.Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
