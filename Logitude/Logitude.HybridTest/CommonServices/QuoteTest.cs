using System;
using Logitude.Server.Tools;
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
            //Response branchServiceResponse = BranchTest.CallBranchUpsert();
            //Assert.IsFalse(branchServiceResponse.HasError, "Branch Upsert Failed! " + branchServiceResponse.ErrorMessage);
            //Assert.IsNotNull(branchServiceResponse.Result, "Branch Upsert Failed! " + branchServiceResponse.ErrorMessage);
            //Response departmentServiceResponse = DepartmentTest.CallDepartmentUpsert();
            //Assert.IsFalse(departmentServiceResponse.HasError, "Departmnet Upsert Failed! " + departmentServiceResponse.ErrorMessage);
            //Assert.IsNotNull(departmentServiceResponse.Result, "Department Upsert Failed! " + departmentServiceResponse.ErrorMessage);
            //Response agentServiceResponse = AgentTest.CallAgentUpsert();
            //Assert.IsFalse(agentServiceResponse.HasError, "Agent Upsert Failed! " + agentServiceResponse.ErrorMessage);
            //Assert.IsNotNull(agentServiceResponse.Result, "Agent Upsert Failed! " + agentServiceResponse.ErrorMessage);
            //Response fromPortServiceResponse = PortTest.CallFromPortUpsert();
            //Assert.IsFalse(fromPortServiceResponse.HasError, "From Port Upsert Failed! " + fromPortServiceResponse.ErrorMessage);
            //Assert.IsNotNull(fromPortServiceResponse.Result, "From Port Upsert Failed! " + fromPortServiceResponse.ErrorMessage);
            //Response toPortServiceResponse = PortTest.CallToPortUpsert();
            //Assert.IsFalse(toPortServiceResponse.HasError, "To Port Upsert Failed! " + toPortServiceResponse.ErrorMessage);
            //Assert.IsNotNull(toPortServiceResponse.Result, "To Port Upsert Failed! " + toPortServiceResponse.ErrorMessage);
            //Response userServiceResponse = UserTest.CallUserUpsert();
            //Assert.IsFalse(userServiceResponse.HasError, "User Upsert Failed! " + userServiceResponse.ErrorMessage);
            //Assert.IsNotNull(userServiceResponse.Result, "User Upsert Failed! " + userServiceResponse.ErrorMessage);
            Response serviceResponse = CallQuoteUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        public static Response CallQuoteUpsert()
        {
            QuoteServiceReference.QuoteWcfServiceClient serviceClient = new QuoteServiceReference.QuoteWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                QuoteServiceReference.QuotePM entityPM = new QuoteServiceReference.QuotePM()
                {
                    QuoteNumber = "Hybrid Quote",
                    TransportModeId = "A", //A:Air, O:Occean, I:Inland
                    DirectionId = "I", //I:Import, E:Export, D:Drop, R:Drop, C:Customs Import
                    BranchId = HybridData.BranchCode,
                    DepartmentId = HybridData.DepartmentCode,
                    CustomerId = HybridData.AgentCode,
                    FromPortId = HybridData.FromPortCode,
                    ToPortId = HybridData.ToPortCode,
                    QuoteTypeCode = "A", //A:Spot Rate, P:Routing Rate
                    ExchangeRate = 1,
                    CreatedByUserId = HybridData.UserCode,
                    UpdatedByUserId = HybridData.UserCode,
                    OpenDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    QuoteCustomerTypeCode = "CON", //CON:Consignee, AGT:Agent, SHI:Shipper, NOT:Notify, OTH:Other
                    SaleCurrencyId = HybridData.CurrencyCode,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                    BusinessUnitId = TestEnvironmentGlobalParameters.Tenant.ToString(),
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }

        [TestMethod]
        public void Test_Quote_GetQuoteList()
        {
            Test_Quote_UPSERT();
            QuoteServiceReference.QuoteWcfServiceClient serviceClient = new QuoteServiceReference.QuoteWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                QuoteServiceReference.QuoteApiFilters filters = new QuoteServiceReference.QuoteApiFilters();
                filters.Take = 10;
                filters.Skip = 0;
                filters.SearchFields = HybridData.FromPortCode;
                QuoteServiceReference.QuoteList[] serviceResult = serviceClient.GetQuoteList(filters, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Quote List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Quote List Failed! " + serviceResponse.ErrorMessage);
                if (serviceResult.Length != 0)
                {
                    string fromPortCode = serviceResult[0].FromPortId;
                    Assert.AreEqual(fromPortCode, HybridData.FromPortCode, "Hybrid Quote Doesn't Exist!");
                }
                else
                {
                    Assert.Inconclusive("There Isn't Quote With This From Port Code!");
                }
            }
        }

        [TestMethod]
        public void Test_Quote_UploadQuotationDocument()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Quote_CreateEvent()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Quote_BuildEventsList()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Quote_DeleteQuoteEvent()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }
    }
}
