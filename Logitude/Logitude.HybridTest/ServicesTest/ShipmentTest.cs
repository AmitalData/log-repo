using System;
using System.Collections.Generic;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.HybridTest.WcfFactory;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
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
        }

        [TestMethod]
        public void Test_DirectShipmentWithDifferentToken_UPSERT()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPM();
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM, EnvironmentGlobalParams.SecondaryTenantToken);
            Assert.IsTrue(serviceResponse.HasError, "Must Be Not Authorized! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Must Be Not Authorized! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_HouseAirExportShipment_UPSERT()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            shipmentPM.ShipmentLevelCode = "H";
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Shipment_CANCEL()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
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
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Canceled Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Canceled Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Shipment_DELETE()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
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
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Deleted Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Deleted Failed! " + serviceOutcome.Response.ErrorMessage);
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
                SearchFields = "Hybrid",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            ShipmentList[] shipments = (ShipmentList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.Result);
            Assert.AreEqual(shipments[0].Id, HybridData.DirectShipmentId, "Get Hybrid Direct Shipment From Shipments Failed!");
        }

        [TestMethod]
        public void Test_Shipment_CreateEvent()
        {
            Assert.Inconclusive("Problem! status id");
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            Response upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "CreateEvent",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, null, shipmentPM.ShipmentNumber, HybridData.UserCodeHU, "CCD", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-2), "Testing hybrid custom cleared" };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Create Event Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Create Event List Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        //[TestMethod]
        //public void Test_Shipment_BuildEventsList()
        //{
        //    ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPM();
        //    Response upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
        //    List<TraceEventPM> events = new List<TraceEventPM>()
        //    {
        //        new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = null, UserId = HybridData.UserCodeHU, EventTypeCode = "DEP", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid departed" },
        //        new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = null, UserId = HybridData.UserCodeHU, EventTypeCode = "ARR", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid arrived" },
        //        new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = null, UserId = HybridData.UserCodeHU, EventTypeCode = "CCD", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid custom cleared" },
        //    };
        //    Shipment_BuildEventsList(shipmentPM.ShipmentNumber, events);
        //}

        [TestMethod]
        public void Test_Shipment_DeleteShipmentEvent()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
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
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
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

        [TestMethod]
        public void Test_Shipment_AddRemovePackages()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            ShipmentPackagePM shipmentPackage = new ShipmentPackagePM()
            {
                PackageTypeCode = "20BU",
                NumberOfInsidePackages = 1,
                Quantity = 1,
                Length = 10,
                Width = 10,
                Height = 10,
            };

            shipmentPM.ShipmentPackages.Add(shipmentPackage);

            Response upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            RestAPIService restAPIService = new RestAPIService();
            ShipmentPM shipment = restAPIService.GetEntityPMById<ShipmentPM>("Shipment", upsertResponse.Result);
            Assert.AreEqual(shipment.ShipmentPackages.Count, 1, "Add Shipment Package Failed!");

            shipmentPM.ShipmentPackages.Remove(shipmentPackage);
            upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            shipment = restAPIService.GetEntityPMById<ShipmentPM>("Shipment", upsertResponse.Result);
            Assert.AreEqual(shipment.ShipmentPackages.Count, 0, "Remove Shipment Package Failed!");
        }

        [TestMethod]
        public void Test_Shipment_ConvertFromDirectToHouse()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            Response upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            shipmentPM.ShipmentLevelCode = "H";
            upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);

            RestAPIService restAPIService = new RestAPIService();
            ShipmentPM shipment = restAPIService.GetEntityPMById<ShipmentPM>("Shipment", upsertResponse.Result);
            Assert.AreEqual(shipment.ShipmentLevelCode, "H", "Convert From Direct To House Failed!");

            //ShipmentPM MasterShipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            ////MasterShipmentPM.ShipmentNumber = "AAAAAAAAAA10000";
            //MasterShipmentPM.ShipmentLevelCode = "C";
            //upsertResponse = EntityWcfCaller.CallEntityUpsert(MasterShipmentPM);
            //shipmentPM.MasterShipmentDataId = upsertResponse.Result;
            //upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
        }

        [TestMethod]
        public void Test_Shipment_ConvertFromHouseToDirect()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            shipmentPM.ShipmentLevelCode = "H";
            Response upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            shipmentPM.ShipmentLevelCode = "D";
            upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);

            RestAPIService restAPIService = new RestAPIService();
            ShipmentPM shipment = restAPIService.GetEntityPMById<ShipmentPM>("Shipment", upsertResponse.Result);
            Assert.AreEqual(shipment.ShipmentLevelCode, "D", "Convert From House To Direct Failed!");
            Assert.AreEqual(shipment.ConvertFromHouseToDirect, true, "Convert From House To Direct Failed!");


            //ShipmentPM MasterShipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            //MasterShipmentPM.ShipmentLevelCode = "C";
            //upsertResponse = EntityWcfCaller.CallEntityUpsert(MasterShipmentPM);
            //shipmentPM.Master = MasterShipmentPM.ShipmentNumber;
            //upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
        }

        [TestMethod]
        public void Test_Shipment_HasExceptionTrue()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            Response upsertResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM);
            List<TraceEventPM> events = new List<TraceEventPM>()
            {
                new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = null, UserId = HybridData.UserCodeHU, EventTypeCode = "EXCE", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid Exception" },
            };

            Shipment_BuildEventsList(shipmentPM.ShipmentNumber, events);

            RestAPIService restAPIService = new RestAPIService();
            ShipmentPM shipment = restAPIService.GetEntityPMById<ShipmentPM>("Shipment", upsertResponse.Result);
            Assert.AreEqual(shipment.HasException, true, "Exception Must Be True!");
        }

        [TestMethod]
        public void Test_Shipment_CustomFileId()
        {
            ShipmentPM customShipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            customShipmentPM.ShipmentLevelCode = "A";
            Response customUpsertResponse = EntityWcfCaller.CallEntityUpsert(customShipmentPM);

            RestAPIService restAPIService = new RestAPIService();
            ShipmentPM shipment = restAPIService.GetEntityPMById<ShipmentPM>("Shipment", customUpsertResponse.Result);
            Assert.AreEqual(shipment.NoFreightFile, true, "Must be Not Connected Custom Shipments!");

            ShipmentPM firstShipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            firstShipmentPM.CustomFileNumber = customShipmentPM.ShipmentNumber;
            Response firstUpsertResponse = EntityWcfCaller.CallEntityUpsert(firstShipmentPM);
            ShipmentPM secondShipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            secondShipmentPM.CustomFileNumber = customShipmentPM.ShipmentNumber;
            Response secondUpsertResponse = EntityWcfCaller.CallEntityUpsert(secondShipmentPM);

            shipment = restAPIService.GetEntityPMById<ShipmentPM>("Shipment", firstUpsertResponse.Result);
            Assert.AreEqual(shipment.CustomFileId, customUpsertResponse.Result, "Custom File Id Failed!");

            shipment = restAPIService.GetEntityPMById<ShipmentPM>("Shipment", secondUpsertResponse.Result);
            Assert.AreEqual(shipment.CustomFileId, customUpsertResponse.Result, "Custom File Id Failed!");

            List<TraceEventPM> events = new List<TraceEventPM>()
            {
                new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = null, UserId = HybridData.UserCodeHU, EventTypeCode = "EXCE", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid Exception" },
            };

            Shipment_BuildEventsList(customShipmentPM.ShipmentNumber, events);

            shipment = restAPIService.GetEntityPMById<ShipmentPM>("Shipment", firstUpsertResponse.Result);
            Assert.AreEqual(shipment.HasException, true, "Exception Must Be True!");

            shipment = restAPIService.GetEntityPMById<ShipmentPM>("Shipment", secondUpsertResponse.Result);
            Assert.AreEqual(shipment.HasException, true, "Exception Must Be True!");
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
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Build Events List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Build Events List Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        private static void Shipment_DeleteShipmentEvent(string shipmentNumber, string externalId)
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "DeleteShipmentEvent",
            };
            
            object[] serviceParameters = new object[] { shipmentNumber, externalId, EnvironmentGlobalParams.MainTenant };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Build Events List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Build Events List Failed! " + serviceOutcome.Response.ErrorMessage);
        }

    }
}
