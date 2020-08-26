using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Senario8_OpenShipmentInCloud_ShipmentOpenedInLogbox()
        {
            RestAPIService restAPIService = new RestAPIService();
            DocumentsFilingPM documentsFilingPM = new DocumentsFilingPM
            {
                Id="12323222",
                EntityNumber = "1112233",
                Code = "DFL",
                CreatedByUserId = "WA",
                UpdatedByUserId = "WA",
                OwnerId = "WA",
                DirectionCode = "O",
                DocumentTypeId = "WRR",
                Tenant = EnvironmentParams.CloudTenant
            };
            //ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPMWithNewNumber();
            ServiceOutcome serviceOutcome = UpsertDocumentInCloud(documentsFilingPM);
            string queueMessagesId = GetQueueMessageId(serviceOutcome);
            CheckStatusOfQueueMessage(restAPIService, queueMessagesId);
            //CheckIfShipmentOpenedInLogbox(restAPIService, shipmentPM);
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
