using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.ShipmentOrderModule.BL.APIDataContract.ApiV1;
using Logitude.ShipmentOrderModule.BL.EntityUpdateServices;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CommunicationWorkerRole.Services.Logbox
{
    public class ImporterShipmentsBatchService
    {
        public int tenant = 0;
        public CustomerTenantAccessCardsBatchPM customerTenantAccessCardsBatch = null;
        public CustomerTenantAccessCardPM customerTenantAccessCard;
        public CustomerTenantAccessPM customerTenantAccessInfo;
        public IQueueService queueservice;
        public string BatchNumber;
        public string CustomerId;
        private string CustomerTenantAccessId;
        private ICommonDataContext commonContext;
        private string queueResponseMessageId;
        private int queueResponseRetryNumber;
        private IWebFreightContext webFreightContext;
        private ImporterBuilderShipmentsBatchService importerBuilderShipmentsBatchService;
        private ImporterBuilderShipmentsOrderBatchService importerBuilderShipmentsOrderBatchService;
        public ImporterShipmentsBatchService()
        {

        }

        public ImporterShipmentsBatchService(QueueResponse queueResponse, IQueueService queueservice)
        {
            this.queueservice = queueservice;
            Initialize(queueResponse);
        }

        private void Initialize(QueueResponse queueResponse)
        {
            int.TryParse(queueResponse.MessageValues["tenant"], out tenant);
            CustomerId = queueResponse.MessageValues["CustomerId"];
            CustomerTenantAccessId = queueResponse.MessageValues["CustomerTenantAccessId"];
            BatchNumber = queueResponse.MessageValues["BatchNumber"];
            queueResponseMessageId = queueResponse.MessageId;
            queueResponseRetryNumber = queueResponse.RetryNumber;
            webFreightContext = WebFreightContext.GetContext(tenant);
            commonContext = CommonDataContext.GetContext(tenant);
            customerTenantAccessCard = null;
            customerTenantAccessInfo = null;
            importerBuilderShipmentsBatchService = new ImporterBuilderShipmentsBatchService(this);
            importerBuilderShipmentsOrderBatchService = new ImporterBuilderShipmentsOrderBatchService(this);

            if (string.IsNullOrEmpty(CustomerTenantAccessId) || string.IsNullOrEmpty(CustomerId))
            {
                return;
            }

            CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
            CustomerTenantAccessCardQuery customerTenantAccessCardQuery = new CustomerTenantAccessCardQuery(tenant);
            customerTenantAccessInfo = customerTenantAccessQuery.GetSinglePM(CustomerTenantAccessId, tenant);
            customerTenantAccessCard = customerTenantAccessInfo.CustomerTenantAccessCards.Where(a => a.CustomerId == CustomerId && a.CustomerTenantAccessId == CustomerTenantAccessId).FirstOrDefault();
            CustomerTenantAccessCardBatchQuery customerTenantAccessCardBatchQuery = new CustomerTenantAccessCardBatchQuery(tenant);
            customerTenantAccessCardsBatch = customerTenantAccessCardBatchQuery.GetSinglePM(CustomerId, CustomerTenantAccessId, BatchNumber, tenant);
        }

        public void Build()
        {
            if (customerTenantAccessCard == null)
            {
                queueservice.Complete();
                return;
            }
            List<Shipment> Shipments = importerBuilderShipmentsBatchService.GetAllShipmentsFromToDate();
            List<Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder> ShipmentsOrders = importerBuilderShipmentsOrderBatchService.GetAllShipmentsOrderFromToDate();
            if ((Shipments == null || Shipments.Count() == 0) && (ShipmentsOrders == null || ShipmentsOrders.Count() == 0))
            {
                queueservice.Complete();
                return;
            }


            const string inProgressStatusCode = "IP";
            UpdateCustomerTenantAccessCard(inProgressStatusCode);

            List<Shipment> allowedShipments = new List<Shipment>();
            List<Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder> allowedShipmentsOrder = new List<Logitude.ShipmentOrderModule.Data.EntityPOCOs.ShipmentOrder>();
            if (Shipments != null && Shipments.Count() > 0)
            {
                allowedShipments = importerBuilderShipmentsBatchService.Run();
            }

            if (ShipmentsOrders != null && ShipmentsOrders.Count() > 0)
            {
                allowedShipmentsOrder = importerBuilderShipmentsOrderBatchService.Run();
            }

            customerTenantAccessCardsBatch.Status = "In Progress";
            customerTenantAccessCardsBatch.TotalShipment = allowedShipments.Count() + allowedShipmentsOrder.Count();
            UpdateCustomerTenantAccessCardsBatchService();

            queueservice.Complete();
            const string acceptedStatusCode = "A";
            customerTenantAccessInfo.Status = acceptedStatusCode;
            UpdateCustomerTenantAccessCard(acceptedStatusCode);
        }

        public void UpdateCustomerTenantAccessCardsBatchService()
        {
            if (customerTenantAccessCardsBatch == null) return;

            CustomerTenantAccessCardsBatchService customerTenantAccessCardsBatchService = new CustomerTenantAccessCardsBatchService(commonContext, tenant, customerTenantAccessCardsBatch);
            customerTenantAccessCardsBatchService.Update();
        }

        private void UpdateCustomerTenantAccessCard(string status)
        {
            customerTenantAccessCard.StatusTypeCode = status;
            customerTenantAccessCard.ChangeSetOp = ChangeSetOperation.Update;
            string systemEmail = "system@tenant" + tenant + ".com";
            CustomerTenantAccessService Service = new CustomerTenantAccessService(commonContext, tenant, customerTenantAccessInfo, systemEmail);
            Service.Update();
        }

        public APILogsPM GetNewLogPM()
        {
            return new APILogsPM()
            {
                Id = IdCounter.GetNumber("APILogs", tenant),
                CorrelationId = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now,
                CreateDateUTC = DateTime.UtcNow,
                Direction = "O",
                LastUpdateDate = DateTime.Now,
                LastUpdateDateUTC = DateTime.UtcNow,
                NumberOfRetries = 1,
                ExpirationDate = DateTime.Now.AddDays(90),
                Status = "I",
                QueueMessageMoreDetailsId = queueResponseMessageId,
                Tenant = tenant
            };
        }

        public void CreateAPILog(List<string> allowedEntityIds, APILogsPM LogPM, string objectTableName)
        {
            APILogsService apiLogsService = new APILogsService(webFreightContext, tenant);
            string shipmentObjectTableId = GetObjectTableIdByName(objectTableName);

            string message = "Start Getting " + objectTableName + " From Date " + customerTenantAccessCardsBatch.FromDatetime + " To Date" + customerTenantAccessCardsBatch.ToDatetime + System.Environment.NewLine + DateTime.Now + System.Environment.NewLine;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))
            {
                LogPM.ObjectTableId = shipmentObjectTableId;
                LogPM.Subject = "Get " + objectTableName + " Range To Send To Importer Tenant";
                apiLogsService.Create(LogPM);
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", queueResponseRetryNumber + 1, DateTime.Now, DateTime.UtcNow, message, LogitudeXmlSerializer.SerializeObjectToXmlString(allowedEntityIds), null, null, "");
                scope.Complete();
            };
        }

        private string GetObjectTableIdByName(string objectTableName)
        {
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objecttable = objectTabelRepository.GetObjectTableByName(objectTableName, tenant, true);
            if (objecttable == null) return "";

            return objecttable.Id;
        }
    }
}
