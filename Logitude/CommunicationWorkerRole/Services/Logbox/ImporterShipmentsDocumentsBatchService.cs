using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services.Logbox
{
    public class ImporterShipmentsDocumentsBatchService
    {
        private int tenant;
        private APILogsService apiLogsService;
        private IWebFreightContext webFreightContext;
        private IQueueService queueservice;
        private DocumentsFilingQuery documentsFilingQuery;
        private string shipmentId;
        private string shipmentOrderId;
        private string batchNumber;
        private string customerId;
        private string correlationId;
        private bool isNewLog;
        private int queueRetryNumber;
        private APILogsPM LogPM;
        public ImporterShipmentsDocumentsBatchService(QueueResponse queueResponse, IQueueService queueservice)
        {
            this.queueservice = queueservice;
            Initialize(queueResponse);
        }

        private void Initialize(QueueResponse queueResponse)
        {
            shipmentId = queueResponse.MessageValues.ContainsKey("ShipmentId") ? queueResponse.MessageValues["ShipmentId"].ToString() : "";
            shipmentOrderId = queueResponse.MessageValues.ContainsKey("ShipmentOrderId") ? queueResponse.MessageValues["ShipmentOrderId"].ToString() : "";
            batchNumber = queueResponse.MessageValues["BatchNumber"].ToString();
            customerId = queueResponse.MessageValues["CustomerId"].ToString();
            int.TryParse(queueResponse.MessageValues["Tenant"], out tenant);
            correlationId = queueResponse.MessageId;
            queueRetryNumber = queueResponse.RetryNumber;
            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Build()
        {
            string entityId = !string.IsNullOrEmpty(shipmentOrderId) ? shipmentOrderId : shipmentId;
            const string inDocumentFilingCode = "I";
            if (!documentsFilingQuery.HaveDocumentsFilingPMsByEntityIdAndDirectionCode(entityId, inDocumentFilingCode, tenant))
            {
                queueservice.Complete();
                return;
            }
            List<DocumentsFilingPM> DocumentFilingPMs = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdWithoutCustomsDetails(entityId, inDocumentFilingCode, tenant);
            if (DocumentFilingPMs == null || DocumentFilingPMs.Count == 0)
            {
                queueservice.Complete();
                return;
            }

            LogPM = GeLogPM();
            string documentsFilingObjectTableId = GetDocumentsFilingObjectTableId();
            LogPM.ObjectTableId = documentsFilingObjectTableId;
            LogPM.Tenant = tenant;
            LogPM.Subject = "Start Building Queues For Documents Batch ImporterShipmentDocuments Controller";
            if (isNewLog)
            {
                CreateAPILog(LogPM);
            }

            foreach (DocumentsFilingPM documentFilingPM in DocumentFilingPMs)
            {
                SendImporterShipmentDocumentQueue(LogPM, documentFilingPM);
            }

            queueservice.Complete();

        }

        private void SendImporterShipmentDocumentQueue(APILogsPM LogPM, DocumentsFilingPM documentFilingPM)
        {
            var msg = "Queue Is Building for Document with Id " + documentFilingPM.Id + " " + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", queueRetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");
            if (!string.IsNullOrEmpty(shipmentOrderId))
            {
                new ShipmentOrderDocumentsQueueService().Build(documentFilingPM);
            }
            else if (documentFilingPM.IsSharedWithCustomer)
            {
                queueservice.InitializeQueue("ImportersShipmentDocumentsBatchQueue", 0);
                queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", shipmentId }, { "DocumentFilingId", documentFilingPM.Id }, { "Tenant", tenant.ToString() }, { "CustomerId", customerId }, { "BatchNumber", batchNumber } }, tenant, null, customerId, batchNumber);
            }

            msg = "Queue Built for Document with Id " + documentFilingPM.Id + " " + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", queueRetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");
        }

        private void CreateAPILog(APILogsPM LogPM)
        {
            LogPM.BatchNumber = batchNumber;
            LogPM.CustomerId = customerId;
            apiLogsService = new APILogsService(webFreightContext, tenant);
            apiLogsService.Create(LogPM);
        }

        private string GetDocumentsFilingObjectTableId()
        {
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable documentsFilingObjecttable = objectTabelRepository.GetObjectTableByName("DocumentsFiling", tenant, true);
            if (documentsFilingObjecttable == null) return "";

            return documentsFilingObjecttable.Id;
        }

        private APILogsPM GeLogPM()
        {
            APILogsRepository aPILogsRepository = new APILogsRepository(webFreightContext);
            APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(correlationId, tenant);
            isNewLog = true;

            if (Log == null)
            {
                return new APILogsPM()
                {
                    Id = IdCounter.GetNumber("APILogs", tenant),
                    CorrelationId = correlationId,
                    CreateDate = DateTime.Now,
                    CreateDateUTC = DateTime.UtcNow,
                    Direction = "O",
                    LastUpdateDate = DateTime.Now,
                    LastUpdateDateUTC = DateTime.UtcNow,
                    NumberOfRetries = 1,
                    ExpirationDate = DateTime.Now.AddDays(90),
                    Status = "I",
                    QueueMessageMoreDetailsId = correlationId
                };
            }

            isNewLog = false;
            return new APILogsPM()
            {
                Id = Log.Id,
                CorrelationId = Log.CorrelationId,
                CreateDate = Log.CreateDate,
                CreateDateUTC = Log.CreateDateUTC,
                Direction = Log.Direction,
                EntityId = Log.EntityId,
                LastUpdateDate = Log.LastUpdateDate,
                LastUpdateDateUTC = Log.LastUpdateDateUTC,
                NumberOfRetries = Log.NumberOfRetries++,
                ObjectTableId = Log.ObjectTableId,
                ExpirationDate = Log.ExpirationDate,
                Refrence = Log.Refrence,
                Status = "I",
                Tenant = Log.Tenant,
                QueueMessageMoreDetailsId = Log.QueueMessageMoreDetailsId
            };
        }

        public void UpsertAPILogAsFailed(string Failmsg, string errorMessage)
        {
            if (!isNewLog)
            {
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", queueRetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
            }

            LogPM.Subject = "Build Documents Queues";
            LogPM.BatchNumber = batchNumber;
            LogPM.CustomerId = customerId;
            apiLogsService = new APILogsService(webFreightContext, tenant);
            apiLogsService.Create(LogPM);

            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", queueRetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
        }
    }
}
