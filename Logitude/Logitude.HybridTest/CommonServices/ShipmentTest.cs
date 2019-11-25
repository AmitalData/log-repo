using System;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
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
            Response serviceResponse = ShipmentWcfCaller.CallDirectShipmentUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_HouseShipment_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = ShipmentWcfCaller.CallHouseShipmentUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Shipment_CANCEL()
        {
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = ShipmentWcfCaller.PrepareDirectShipment();
            Assert.IsFalse(prepareResponse.HasError, "Prepare Direct Shipment Failed! " + prepareResponse.ErrorMessage);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "Cancel",
                ServiceResponseIndex = 0,
                ServiceType = null,
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.DirectShipmentCode, TestEnvironmentGlobalParameters.Tenant };
            serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Canceled Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Canceled Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Shipment_DELETE()
        {
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = ShipmentWcfCaller.PrepareDirectShipment();
            Assert.IsFalse(prepareResponse.HasError, "Prepare Direct Shipment Failed! " + prepareResponse.ErrorMessage);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "Delete",
                ServiceResponseIndex = 0,
                ServiceType = null,
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.DirectShipmentCode, TestEnvironmentGlobalParameters.Tenant };
            serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Deleted Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Deleted Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Shipment_GetShipmentList()
        {
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = ShipmentWcfCaller.PrepareDirectShipment();
            Assert.IsFalse(prepareResponse.HasError, "Prepare Direct Shipment Failed! " + prepareResponse.ErrorMessage);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "GetShipmentList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(ShipmentList),
                ServiceFilterType = typeof(ShipmentServiceReference.ShipmentApiFilters),
            };
            ShipmentServiceReference.ShipmentApiFilters filters = new ShipmentServiceReference.ShipmentApiFilters
            {
                Take = 10,
                SearchFields = "Hybrid DShipment,OPOP,Created,HFP,Hybrid From Port,HC,Hybrid Country,HFP,Hybrid From Port,HC,Hybrid Country,HTP,Hybrid To Port,HC,Hybrid Country,HTP,Hybrid To Port,HC,Hybrid Country,Hybrid Agent",
                //ShipmentLevel = "H",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            ShipmentList[] shipments = (ShipmentList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            Assert.AreEqual(shipments[0].Id, HybridData.DirectShipmentId, "Get Hybrid Direct Shipment From Shipments Failed!");
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
