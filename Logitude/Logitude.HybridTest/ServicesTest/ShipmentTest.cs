using System;
using System.Collections.Generic;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.HybridTest.WcfFactory;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
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
            shipmentPM.ShipmentNumber = HybridData.HouseShipmentCode;
            shipmentPM.ShipmentLevelCode = "H";
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
            HybridData.HouseShipmentId = serviceResponse.Result;
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
            object[] serviceParameters = new object[] { HybridData.DirectShipmentCode, TestEnvironmentGlobalParameters.Tenant1 };
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
            object[] serviceParameters = new object[] { HybridData.DirectShipmentCode, TestEnvironmentGlobalParameters.Tenant1 };
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
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            ShipmentList[] shipments = (ShipmentList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            Assert.AreEqual(shipments[0].Id, HybridData.DirectShipmentId, "Get Hybrid Direct Shipment From Shipments Failed!");
        }

        [TestMethod]
        public void Test_Shipment_CreateEvent()
        {
            Assert.Inconclusive("Not Implemented !");
            if (HybridData.HouseShipmentId == null)
                Test_HouseAirExportShipment_UPSERT();
            HybridData.customClearedExternalId = Guid.NewGuid().ToString();
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "CreateEvent",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { TestEnvironmentGlobalParameters.Tenant1, HybridData.customClearedExternalId, HybridData.HouseShipmentCode, HybridData.UserCodeHU, "CCD", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-2), "Testing hybrid custom cleared" };
            serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Create Event Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Create Event List Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Shipment_BuildEventsList()
        {
            if (HybridData.HouseShipmentId == null)
                Test_HouseAirExportShipment_UPSERT();
            HybridData.customClearedExternalId = Guid.NewGuid().ToString();
            List<TraceEventPM> events = new List<TraceEventPM>()
                 {
                     new TraceEventPM() { Tenant = TestEnvironmentGlobalParameters.Tenant1, ExternalId = Guid.NewGuid().ToString(), UserId = HybridData.UserCodeHU, EventTypeCode = "DEP", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid departed" },
                     new TraceEventPM() { Tenant = TestEnvironmentGlobalParameters.Tenant1, ExternalId = Guid.NewGuid().ToString(), UserId = HybridData.UserCodeHU, EventTypeCode = "ARR", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid arrived" },
                     new TraceEventPM() { Tenant = TestEnvironmentGlobalParameters.Tenant1, ExternalId = HybridData.customClearedExternalId, UserId = HybridData.UserCodeHU, EventTypeCode = "CCD", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid custom cleared" },

                };
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "BuildEventsList",
                ServiceType = typeof(TraceEventPM),
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { TestEnvironmentGlobalParameters.Tenant1, HybridData.HouseShipmentCode, events.ToArray() };
            serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Build Events List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Build Events List Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Shipment_DeleteShipmentEvent()
        {
            if (HybridData.customClearedExternalId == null)
                Test_Shipment_BuildEventsList();
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "DeleteShipmentEvent",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.HouseShipmentCode, HybridData.customClearedExternalId, TestEnvironmentGlobalParameters.Tenant1 };
            serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Build Events List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Build Events List Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Shipment_DeleteShipmentTraceEvent()
        {
            //LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }
    }
}
