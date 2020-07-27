using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.HybridTest;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.LogboxIntegrationTest.Senarios
{
    [TestClass]
    public class OngoingShipmentsUNItoLogboxViaCloud
    {
        [TestMethod]
        public void Senario9_OpenShipmentInCloud_ShipmentOpenedInLogbox()
        {
            RestAPIService restAPIService = new RestAPIService();
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            CloudVariables.TempShipmentNumber = shipmentPM.ShipmentNumber;
            ServiceOutcome serviceOutcome = UpsertShipmentInCloud(shipmentPM);
            string queueMessagesId = GetQueueMessageId(serviceOutcome);
            CheckStatusOfQueueMessage(restAPIService, queueMessagesId);
            CheckIfShipmentOpenedInLogbox(restAPIService, shipmentPM);
        }

        [TestMethod]
        public void Senario91_IsCustomsClearance_ArchivedShipment()
        {
            RestAPIService restAPIService = new RestAPIService();
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPM();
            shipmentPM.ShipmentNumber = "81bfe0757ac2";//CloudVariables.TempShipmentNumber;
            shipmentPM.CustomsClearanceDate = DateTime.Now;
            shipmentPM.IsOperationalClosed = false;
            shipmentPM.StatusId = "CCD";
            ServiceOutcome serviceOutcome = UpsertShipmentInCloud(shipmentPM);
            string queueMessagesId = GetQueueMessageId(serviceOutcome);
            CheckStatusOfQueueMessage(restAPIService, queueMessagesId);
            ShipmentPM logboxShipmentPM = GetShipmentPMByForwarderShipmentNumber(restAPIService, shipmentPM.ShipmentNumber);
            Assert.AreEqual(false, logboxShipmentPM.IsRequestedDocuments);
            Assert.AreEqual(0, logboxShipmentPM.RequestedDocumentsCount);
            Assert.AreEqual(false, logboxShipmentPM.IsMissingDocuments);
            Assert.AreEqual(0, logboxShipmentPM.MissingDocumentsCount);
        }

        [TestMethod]
        public void Senario92_IsAutoArchiveOnInvoice_IsOperationalClosed()
        {
            RestAPIService restAPIService = new RestAPIService();
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            shipmentPM.ShipmentLevelCode = "A";
            shipmentPM.CustomsClearanceDate = DateTime.Now;
            shipmentPM.IsOperationalClosed = false;
            shipmentPM.StatusId = "INPR";
            ServiceOutcome serviceOutcome = UpsertShipmentInCloud(shipmentPM);
            string queueMessagesId = GetQueueMessageId(serviceOutcome);
            CheckStatusOfQueueMessage(restAPIService, queueMessagesId);
            ShipmentPM logboxShipmentPM = GetShipmentPMByForwarderShipmentNumber(restAPIService, shipmentPM.ShipmentNumber);
            Assert.AreEqual(true, logboxShipmentPM.IsOperationalClosed);
        }

        [TestMethod]
        public void Senario93_IsCustomsClearanceAndHasException_CustomsClearedShipments()
        {
            RestAPIService restAPIService = new RestAPIService();
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            shipmentPM.ShipmentNumber = "TestWesam";//"81bfe0757ac2";//CloudVariables.TempShipmentNumber;
            shipmentPM.CustomsClearanceDate = null;
            shipmentPM.ExceptionDescription = "Test";
            ServiceOutcome serviceOutcome = UpsertShipmentInCloud(shipmentPM);
            shipmentPM.CustomsClearanceDate = DateTime.Now;
            shipmentPM.HasException = true;
            serviceOutcome = UpsertShipmentInCloud(shipmentPM);
            string queueMessagesId = GetQueueMessageId(serviceOutcome);
            CheckStatusOfQueueMessage(restAPIService, queueMessagesId);
            ShipmentPM logboxShipmentPM = GetShipmentPMByForwarderShipmentNumber(restAPIService, shipmentPM.ShipmentNumber);
            Assert.AreEqual(false, logboxShipmentPM.HasException);
            Assert.AreEqual(null, logboxShipmentPM.ExceptionDescription);
            Assert.AreEqual(null, logboxShipmentPM.ExceptionDate);
            Assert.AreEqual("Customs Clearance", logboxShipmentPM.ExceptionResolvedDescription);
        }

        private ServiceOutcome UpsertShipmentInCloud(ShipmentPM shipmentPM)
        {
            AdditionalIncludedData includedData = new AdditionalIncludedData
            {
                URL = EnvironmentParams.CloudServerURL,
                Token = EnvironmentParams.CloudTenantToken
            };
            ServiceOutcome serviceOutcome = EntityWcfCaller.CallEntityUpsert(shipmentPM, null, includedData);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            return serviceOutcome;
        }

        private static string GetQueueMessageId(ServiceOutcome serviceOutcome)
        {
            Assert.IsNotNull(serviceOutcome.QueueMessagesDetails, "The queue did not open");
            string queueMessagesId = serviceOutcome.QueueMessagesDetails.FirstOrDefault(q => q.Value.Contains("ImporterTenant")).Key;
            Assert.IsNotNull(queueMessagesId, "There is no queue with name 'ImporterTenant'");
            return queueMessagesId;
        }

        private static void CheckStatusOfQueueMessage(RestAPIService restAPIService, string queueMessagesId)
        {
            QueueMessageMoreDetailsList queueMessageMoreDetailsList = null;
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(new TimeSpan(0, 0, 5));
                queueMessageMoreDetailsList = restAPIService.GetEntityByUrl<QueueMessageMoreDetailsList>("QueueMessageMoreDetailsViews/GetSingle?id=" + queueMessagesId, EnvironmentParams.CloudTenantToken);
                if(queueMessageMoreDetailsList != null && queueMessageMoreDetailsList.Status == 1)
                {
                    break;
                }
            }

            Assert.IsNotNull(queueMessageMoreDetailsList, "There is no queue with id:" + queueMessagesId);
            Assert.AreEqual(1, queueMessageMoreDetailsList.Status, "The queue message has not been completed");
        }

        private static void CheckIfShipmentOpenedInLogbox(RestAPIService restAPIService, ShipmentPM shipmentPM)
        {
            string url = "ImporterShipments/GetIfShipmentExists?importertenant=" + EnvironmentParams.LogboxTenant + "&Tenant=" + EnvironmentParams.CloudTenant + "&shipmentnumber=" + shipmentPM.ShipmentNumber;
            bool isCreateShipment = restAPIService.GetEntityByUrl<bool>(url, EnvironmentParams.CloudTenantToken);
            Assert.AreEqual(true, isCreateShipment, "The shipment did not open in logbox");
        }

        private static ShipmentPM GetShipmentPMByForwarderShipmentNumber(RestAPIService restAPIService, string forwarderShipmentNumber)
        {
            string url = "Shipment/GetSingleByForwarderShipmentNumber?fsn=" + forwarderShipmentNumber;
            ShipmentPM shipmentPM = restAPIService.GetEntityByUrl<ShipmentPM>(url, EnvironmentParams.LogboxTenantToken);
            return shipmentPM;
        }

        private static ShipmentPM GetShipmentPMByShipmentNumber(RestAPIService restAPIService, string shipmentNumber)
        {
            ShipmentPM shipmentPM = null;
            string url = "Shipment/getSingleByShipmentNumber?number=" + shipmentNumber;
            string shipmentId = restAPIService.GetEntityByUrl<string>(url, EnvironmentParams.CloudTenantToken);
            if (!string.IsNullOrEmpty(shipmentId))
            {
                url = "Shipment/GetSingle?id=" + shipmentId;
                shipmentPM = restAPIService.GetEntityByUrl<ShipmentPM>(url, EnvironmentParams.CloudTenantToken);

            }
            return shipmentPM;
        }

    }
}
