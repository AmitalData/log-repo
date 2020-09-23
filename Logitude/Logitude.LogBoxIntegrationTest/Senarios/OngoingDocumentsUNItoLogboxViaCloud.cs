using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.HybridTest;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.LogboxIntegrationTest.Senarios
{
    [TestClass]
    public class OngoingDocumentsUNItoLogboxViaCloud
    {
        [TestMethod]
        public void Senario10_OpenDocumentInCloud_QueueOpened()
        {
            RestAPIService restAPIService = new RestAPIService();
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            ServiceOutcome serviceOutcome = UpsertShipmentInCloud(shipmentPM);
            string queueMessagesId = GetQueueMessageId(serviceOutcome);
            CheckStatusOfQueueMessage(restAPIService, queueMessagesId);
            DocumentsFilingPM documentsFilingPM = GetDocumentFilingPM(shipmentPM.ShipmentNumber, serviceOutcome.Response.Result);
            serviceOutcome = UpsertDocumentInCloud(documentsFilingPM);
            queueMessagesId = GetQueueMessageId(serviceOutcome);
            CheckStatusOfQueueMessage(restAPIService, queueMessagesId);
        }

        private static DocumentsFilingPM GetDocumentFilingPM(string shipmentNumber, string shipmentId)
        {
            return new DocumentsFilingPM
            {
                Id = "123456789123456789123456789123",
                EntityNumber = shipmentNumber,
                EntityId = shipmentId,
                Code = "DFL",
                CreatedByUserId = "WA",
                UpdatedByUserId = "WA",
                OwnerId = "WA",
                DirectionCode = "I",
                DocumentTypeId = "WRR",
                FileData = Encoding.ASCII.GetBytes("This is a test"),
                FileExtension = "txt",
                ObjectTableName = "Shipment",
                IsSharedWithCustomer = true,
                Tenant = EnvironmentParams.CloudTenant
            };
        }

        private ServiceOutcome UpsertDocumentInCloud(DocumentsFilingPM documentsFilingPM)
        {
            AdditionalIncludedData includedData = new AdditionalIncludedData
            {
                URL = EnvironmentParams.CloudServerURL,
                Token = EnvironmentParams.CloudTenantToken
            };
            ServiceOutcome serviceOutcome = EntityWcfCaller.CallEntityUpsert(documentsFilingPM, null, includedData);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            return serviceOutcome;
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

    }
}
