using System;
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
            Server.Tools.Response serviceResponse = CallDirectShipmentUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        [TestMethod]
        public void Test_HouseShipment_UPSERT()
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
            Server.Tools.Response serviceResponse = CallHouseShipmentUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        public static Server.Tools.Response CallDirectShipmentUpsert()
        {

            ShipmentServiceReference.ShipmentWcfServiceClient serviceClient = new ShipmentServiceReference.ShipmentWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                ShipmentServiceReference.ShipmentPM entityPM = new ShipmentServiceReference.ShipmentPM()
                {
                    ShipmentNumber = "Hybrid Shipment",
                    TransportModeId = "A",
                    DirectionId = "E",
                    FreightPrepaidCollectId = "P",
                    OtherPrepaidCollectId = "P",
                    BranchId = HybridCodes.BranchCode,
                    DepartmentId = HybridCodes.DepartmentCode,
                    ConsigneeId = HybridCodes.AgentCode,
                    ShipmentLevelCode = "D",
                    FromPortId = HybridCodes.PortCode,
                    ToPortId = HybridCodes.PortCode,
                    MainCarriageFromPortId = HybridCodes.PortCode,
                    MainCarriageToPortId = HybridCodes.PortCode,
                    CreateDateTime = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                Logitude.Server.Tools.Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }

        public static Server.Tools.Response CallHouseShipmentUpsert()
        {

            ShipmentServiceReference.ShipmentWcfServiceClient serviceClient = new ShipmentServiceReference.ShipmentWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                ShipmentServiceReference.ShipmentPM entityPM = new ShipmentServiceReference.ShipmentPM()
                {
                    ShipmentNumber = "Hybrid Shipment",
                    TransportModeId = "A",
                    DirectionId = "E",
                    FreightPrepaidCollectId = "P",
                    OtherPrepaidCollectId = "P",
                    BranchId = HybridCodes.BranchCode,
                    DepartmentId = HybridCodes.DepartmentCode,
                    ConsigneeId = HybridCodes.AgentCode,
                    ShipmentLevelCode = "H",
                    FromPortId = HybridCodes.PortCode,
                    ToPortId = HybridCodes.PortCode,
                    MainCarriageFromPortId = HybridCodes.PortCode,
                    MainCarriageToPortId = HybridCodes.PortCode,
                    CreateDateTime = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                Logitude.Server.Tools.Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
