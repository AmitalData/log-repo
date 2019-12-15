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
        public void Test_DirectAirExportShipmentToken2_UPSERT()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPM();
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM, EnvironmentGlobalParams.SecondaryToken);
            Assert.IsTrue(serviceResponse.HasError, "Must Be Not Authorized! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Must Be Not Authorized! " + serviceResponse.ErrorMessage);
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
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPM();
            //shipmentPM.ShipmentNumber = CodeCounter.GetNumber();
            Response upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "Cancel",
                ServiceResponseIndex = 0,
                ServiceType = null,
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { shipmentPM.ShipmentNumber, EnvironmentGlobalParams.MainTenant };
            serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Canceled Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Canceled Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Shipment_DELETE()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPM();
            //shipmentPM.ShipmentNumber = CodeCounter.GetNumber();
            Response upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "Delete",
                ServiceResponseIndex = 0,
                ServiceType = null,
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { shipmentPM.ShipmentNumber, EnvironmentGlobalParams.MainTenant };
            serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Deleted Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Deleted Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Shipment_GetShipmentList()
        {
            //Assert.Inconclusive("Search Field Problem!");
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
                SearchFields = "Hybrid",
                //ShipmentLevel = "H",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ShipmentList[] shipments = (ShipmentList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            Assert.AreEqual(shipments[0].Id, HybridData.DirectShipmentId, "Get Hybrid Direct Shipment From Shipments Failed!");
        }

        [TestMethod]
        public void Test_Shipment_CreateEvent()
        {
            Assert.Inconclusive("Problem! status id");
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPM();
            //shipmentPM.ShipmentNumber = CodeCounter.GetNumber();
            Response upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "CreateEvent",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, null, shipmentPM.ShipmentNumber, HybridData.UserCodeHU, "CCD", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-2), "Testing hybrid custom cleared" };
            serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Create Event Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Create Event List Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Shipment_BuildEventsList()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPM();
            //shipmentPM.ShipmentNumber = CodeCounter.GetNumber();
            Response upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            List<TraceEventPM> events = new List<TraceEventPM>()
            {
                new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = null, UserId = HybridData.UserCodeHU, EventTypeCode = "DEP", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid departed" },
                new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = null, UserId = HybridData.UserCodeHU, EventTypeCode = "ARR", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid arrived" },
                new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = null, UserId = HybridData.UserCodeHU, EventTypeCode = "CCD", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid custom cleared" },

            };
            Shipment_BuildEventsList(shipmentPM.ShipmentNumber, events);
        }

        [TestMethod]
        public void Test_Shipment_DeleteShipmentEvent()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPM();
            //shipmentPM.ShipmentNumber = CodeCounter.GetNumber();
            Response upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            string departedExternalId = Guid.NewGuid().ToString();
            List<TraceEventPM> events = new List<TraceEventPM>()
            {
                new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = departedExternalId, UserId = HybridData.UserCodeHU, EventTypeCode = "DEP", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid departed" },
            };
            Shipment_BuildEventsList(shipmentPM.ShipmentNumber, events);

            Shipment_DeleteShipmentEvent(shipmentPM.ShipmentNumber, departedExternalId);
        }
        [TestMethod]
        public void Test_Shipment_ChangeStatusByEvents()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPM();
            //shipmentPM.ShipmentNumber = CodeCounter.GetNumber();
            Response upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            string customClearedExternalId = Guid.NewGuid().ToString();
            List<TraceEventPM> events = new List<TraceEventPM>()
            {
                new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = null, UserId = HybridData.UserCodeHU, EventTypeCode = "DEP", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid departed" },
                new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = null, UserId = HybridData.UserCodeHU, EventTypeCode = "ARR", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid arrived" },
                new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = customClearedExternalId, UserId = HybridData.UserCodeHU, EventTypeCode = "CCD", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid custom cleared" },
            };
            Shipment_BuildEventsList(shipmentPM.ShipmentNumber, events);
            RestAPIService restAPIService = new RestAPIService();
            ShipmentPM shipment = restAPIService.GetEntityPMById<ShipmentPM>("Shipment", upsertResponse.Result);
            Assert.AreEqual(shipment.StatusName, "Cleared", "Status Must Be Cleared!");
            Shipment_DeleteShipmentEvent(shipmentPM.ShipmentNumber, customClearedExternalId);
            shipment = restAPIService.GetEntityPMById<ShipmentPM>("Shipment", upsertResponse.Result);
            Assert.AreEqual(shipment.StatusName, "Arrived", "Status Must Be Arrived!");
        }
        
        private static void Shipment_BuildEventsList(string shipmentNumber, List<TraceEventPM> events)
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "BuildEventsList",
                ServiceType = typeof(TraceEventPM),
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, shipmentNumber, events.ToArray() };
            serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Build Events List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Build Events List Failed! " + serviceResponse.ErrorMessage);
        }

        private static void Shipment_DeleteShipmentEvent(string shipmentNumber, string externalId)
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "DeleteShipmentEvent",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { shipmentNumber, externalId, EnvironmentGlobalParams.MainTenant };
            serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Build Events List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Build Events List Failed! " + serviceResponse.ErrorMessage);
        }

    }
}
