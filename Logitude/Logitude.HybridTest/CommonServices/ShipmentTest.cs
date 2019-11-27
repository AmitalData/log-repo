using System;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.HybridTest.WcfFactory;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class ShipmentTest
    {
        
        [TestMethod]
        public void Test_DirectAirExportShipment_UPSERT()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPM();
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
            HybridData.DirectShipmentId = serviceResponse.Result;
        }

        [TestMethod]
        public void Test_HouseAirExportShipment_UPSERT()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPM();
            shipmentPM.ShipmentLevelCode = "H";
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Shipment_CANCEL()
        {
            if (HybridData.DirectShipmentId == null)
                Test_DirectAirExportShipment_UPSERT();
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
            if (HybridData.DirectShipmentId == null)
                Test_DirectAirExportShipment_UPSERT();
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
            Assert.Inconclusive("Search Field Problem!");
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
            Assert.Inconclusive("Not Implemented !");
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "CreateEvent",
                ServiceResponseIndex = 0,
                ServiceType = null,
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { TestEnvironmentGlobalParameters.Tenant, null, HybridData.DirectShipmentCode, HybridData.UserId, "CREV", DateTime.Now, DateTime.Now, "Hybrid Test Event" };
            serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Create Event Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Create Event Failed! " + serviceResponse.Result);
        }

        [TestMethod]
        public void Test_Shipment_BuildEventsList()
        {
            //LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Shipment_DeleteShipmentEvent()
        {
           // LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Shipment_DeleteShipmentTraceEvent()
        {
            //LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }
    }
}
