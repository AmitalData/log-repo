using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class ShipmentTest
    {
        [TestMethod]
        public void Test_DirectShipment_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response branchServiceResponse = BranchTest.CallBranchUpsert();
            Assert.IsFalse(branchServiceResponse.HasError, "Branch Upsert Failed! " + branchServiceResponse.ErrorMessage);
            Assert.IsNotNull(branchServiceResponse.Result, "Branch Upsert Failed! " + branchServiceResponse.ErrorMessage);
            Response departmentServiceResponse = DepartmentTest.CallDepartmentUpsert();
            Assert.IsFalse(departmentServiceResponse.HasError, "Departmnet Upsert Failed! " + departmentServiceResponse.ErrorMessage);
            Assert.IsNotNull(departmentServiceResponse.Result, "Department Upsert Failed! " + departmentServiceResponse.ErrorMessage);
            Response agentServiceResponse = AgentTest.CallAgentUpsert();
            Assert.IsFalse(agentServiceResponse.HasError, "Agent Upsert Failed! " + agentServiceResponse.ErrorMessage);
            Assert.IsNotNull(agentServiceResponse.Result, "Agent Upsert Failed! " + agentServiceResponse.ErrorMessage);
            Response fromPortServiceResponse = PortTest.CallFromPortUpsert();
            Assert.IsFalse(fromPortServiceResponse.HasError, "From Port Upsert Failed! " + fromPortServiceResponse.ErrorMessage);
            Assert.IsNotNull(fromPortServiceResponse.Result, "From Port Upsert Failed! " + fromPortServiceResponse.ErrorMessage);
            Response toPortServiceResponse = PortTest.CallToPortUpsert();
            Assert.IsFalse(toPortServiceResponse.HasError, "To Port Upsert Failed! " + toPortServiceResponse.ErrorMessage);
            Assert.IsNotNull(toPortServiceResponse.Result, "To Port Upsert Failed! " + toPortServiceResponse.ErrorMessage);
            Response serviceResponse = CallDirectShipmentUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        [TestMethod]
        public void Test_HouseShipment_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response branchServiceResponse = BranchTest.CallBranchUpsert();
            Assert.IsFalse(branchServiceResponse.HasError, "Branch Upsert Failed! " + branchServiceResponse.ErrorMessage);
            Assert.IsNotNull(branchServiceResponse.Result, "Branch Upsert Failed! " + branchServiceResponse.ErrorMessage);
            Response departmentServiceResponse = DepartmentTest.CallDepartmentUpsert();
            Assert.IsFalse(departmentServiceResponse.HasError, "Departmnet Upsert Failed! " + departmentServiceResponse.ErrorMessage);
            Assert.IsNotNull(departmentServiceResponse.Result, "Department Upsert Failed! " + departmentServiceResponse.ErrorMessage);
            Response agentServiceResponse = AgentTest.CallAgentUpsert();
            Assert.IsFalse(agentServiceResponse.HasError, "Agent Upsert Failed! " + agentServiceResponse.ErrorMessage);
            Assert.IsNotNull(agentServiceResponse.Result, "Agent Upsert Failed! " + agentServiceResponse.ErrorMessage);
            Response portServiceResponse = PortTest.CallFromPortUpsert();
            Assert.IsFalse(portServiceResponse.HasError, "Port Upsert Failed! " + portServiceResponse.ErrorMessage);
            Assert.IsNotNull(portServiceResponse.Result, "Port Upsert Failed! " + portServiceResponse.ErrorMessage);
            Response serviceResponse = CallHouseShipmentUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        public static Response CallDirectShipmentUpsert()
        {
            ShipmentServiceReference.ShipmentWcfServiceClient serviceClient = new ShipmentServiceReference.ShipmentWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                ShipmentServiceReference.ShipmentPM entityPM = new ShipmentServiceReference.ShipmentPM()
                {
                    ShipmentNumber = "Hybrid Shipment",
                    TransportModeId = "A", //A:Air, O:Occean, I:Inland
                    DirectionId = "E", //I:Import, E:Export, D:Drop, R:Drop, C:Customs Import
                    FreightPrepaidCollectId = "P", //P:Prepaid, C:Collect, B:Both
                    OtherPrepaidCollectId = "P", //P:Prepaid, C:Collect, B:Both
                    BranchId = HybridCodes.BranchCode,
                    DepartmentId = HybridCodes.DepartmentCode,
                    ConsigneeId = HybridCodes.AgentCode,
                    ShipmentLevelCode = "D", //D:Direct, H: House, C:Consol, A:Customs
                    FromPortId = HybridCodes.FromPortCode,
                    ToPortId = HybridCodes.ToPortCode,
                    MainCarriageFromPortId = HybridCodes.FromPortCode,
                    MainCarriageToPortId = HybridCodes.ToPortCode,
                    CreateDateTime = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    AgentContactId = HybridCodes.AgentCode,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }

        public static Response CallHouseShipmentUpsert()
        {

            ShipmentServiceReference.ShipmentWcfServiceClient serviceClient = new ShipmentServiceReference.ShipmentWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                ShipmentServiceReference.ShipmentPM entityPM = new ShipmentServiceReference.ShipmentPM()
                {
                    ShipmentNumber = "Hybrid Shipment",
                    TransportModeId = "A", //A:Air, O:Occean, I:Inland
                    DirectionId = "E", //I:Import, E:Export, D:Drop, R:Drop, C:Customs Import
                    FreightPrepaidCollectId = "P", //P:Prepaid, C:Collect, B:Both
                    OtherPrepaidCollectId = "P", //P:Prepaid, C:Collect, B:Both
                    BranchId = HybridCodes.BranchCode,
                    DepartmentId = HybridCodes.DepartmentCode,
                    ConsigneeId = HybridCodes.AgentCode,
                    ShipmentLevelCode = "H", //D:Direct, H: House, C:Consol, A:Customs
                    FromPortId = HybridCodes.FromPortCode,
                    ToPortId = HybridCodes.ToPortCode,
                    MainCarriageFromPortId = HybridCodes.FromPortCode,
                    MainCarriageToPortId = HybridCodes.ToPortCode,
                    CreateDateTime = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    AgentContactId = HybridCodes.AgentCode,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }

        [TestMethod]
        public void Test_Shipment_CANCEL()
        {
            Test_DirectShipment_UPSERT();
            ShipmentServiceReference.ShipmentWcfServiceClient serviceClient = new ShipmentServiceReference.ShipmentWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                Response serviceResponse = serviceClient.Cancel("Hybrid Shipment", TestEnvironmentGlobalParameters.Tenant);
                Assert.IsFalse(serviceResponse.HasError, "Canceled Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Canceled Failed! " + serviceResponse.ErrorMessage);

            }
        }

        [TestMethod]
        public void Test_Shipment_DELETE()
        {
            Test_DirectShipment_UPSERT();
            ShipmentServiceReference.ShipmentWcfServiceClient serviceClient = new ShipmentServiceReference.ShipmentWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                Response serviceResponse = serviceClient.Delete("Hybrid Shipment", TestEnvironmentGlobalParameters.Tenant);
                Assert.IsFalse(serviceResponse.HasError, "Canceled Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Canceled Failed! " + serviceResponse.ErrorMessage);

            }
        }

        [TestMethod]
        public void Test_Shipment_GetShipmentList()
        {
            Test_DirectShipment_UPSERT();
            ShipmentServiceReference.ShipmentWcfServiceClient serviceClient = new ShipmentServiceReference.ShipmentWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                ShipmentServiceReference.ShipmentApiFilters filters = new ShipmentServiceReference.ShipmentApiFilters
                {
                    Take = 10,
                    SearchFields = "Hybrid Shipment"
                };
                ShipmentServiceReference.ShipmentList[] serviceResult = serviceClient.GetShipmentList(filters, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Shipment List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Shipment List Failed! " + serviceResponse.ErrorMessage);
                if (serviceResult.Length != 0)
                {
                    string userCode = serviceResult[0].ShipmentNumber;
                    Assert.AreEqual(userCode, HybridCodes.UserCode, "Shipment List Doesn't Exist!");
                }
                else
                {
                    Assert.Inconclusive("Doesn't Exist Any Shipment!");
                }
            }
        }

        [TestMethod]
        public void Test_Shipment_CreateEvent()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Shipment_BuildEventsList()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Shipment_DeleteShipmentEvent()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Shipment_DeleteShipmentTraceEvent()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }
    }
}
