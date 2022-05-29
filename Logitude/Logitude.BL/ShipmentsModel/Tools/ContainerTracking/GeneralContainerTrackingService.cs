using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.APIDataContract;
using Logitude.BL.ShipmentsModel.CloseTables;
using Logitude.BL.ShipmentsModel.EntityOtherServices;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using RestSharp;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.ShipmentsModel.Tools.ContainerTracking
{
    public class GeneralContainerTrackingService
    {
        private const string GeneralUpdateContainerStatusQueueName = "GeneralUpdateContainerStatus";
        private const string GeneralRequestUpdateContainerStatusQueueName = "GeneralRequestUpdateContainerStatus";
        ICommonDataContext CommonContext;

        private GeneralContainerTrackingArgs simulatorArgs;
        private int tenant;
        public GeneralContainerTrackingService(GeneralContainerTrackingArgs simulatorArgs)
        {
            this.simulatorArgs = simulatorArgs;
            this.tenant = simulatorArgs.Tenant;
        }

        public GeneralContainerTrackingService()
        {

        }

        public void GeneralSimulateContainerStatus()
        {
            InitializeContext();
            SetSimulatorArgsFields();
            if (!CheckValidation())
                return;
            using (TransactionScope scope = Simplog.Server.Infrastructure.Helpers.TransactionFactory.GetTransaction())
            {
                var document = AddRequstDocument();
                var commLog = AddRequstCommunicationLog(document);
                SendCommunicationLogMessage(commLog, simulatorArgs.Tenant);
                scope.Complete();
            }
        }

        private void SetSimulatorArgsFields()
        {
            var context = ShipmentsContext.GetContext(simulatorArgs.Tenant);
            var shipment = context.Shipments.Where(e => e.Id == simulatorArgs.ShipmentId).FirstOrDefault();
            var masterID = simulatorArgs.ShipmentId;
            if (shipment.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(shipment.MasterShipmentDataId) )
            {
                masterID = shipment.MasterShipmentDataId;
            }
            var shipmentMasterData = context.ShipmentMasterDatas.Include(e=>e.MainCarriageCarrierCard).Where(e => e.Id == masterID && e.Tenant == simulatorArgs.Tenant).FirstOrDefault();

            simulatorArgs.CarrierId = shipmentMasterData?.MainCarriageCarrierId;
            simulatorArgs.CarrierCode = shipmentMasterData?.MainCarriageCarrierCard?.Code;
            simulatorArgs.Master = shipmentMasterData?.Master;
        }

        private bool CheckValidation()
        {
            if(string.IsNullOrEmpty( simulatorArgs.CarrierId ))
                simulatorArgs.Errors.Add("Carrier is missing");
            if(string.IsNullOrEmpty( simulatorArgs.Data ))
                simulatorArgs.Errors.Add("Response is missing");
            if(simulatorArgs.IsFromContainer && string.IsNullOrEmpty(simulatorArgs.ContainerNumber))
                simulatorArgs.Errors.Add("Container Number is missing");
            //CheckCarrierIsSupported(simulatorArgs);
            if(!simulatorArgs.IsFromContainer && string.IsNullOrEmpty(simulatorArgs.Master))
                simulatorArgs.Errors.Add("Master Number is missing");
            if (simulatorArgs.Errors.Count > 0)
            {
                simulatorArgs.Success = false;
            }
            return simulatorArgs.Success;
        }

        private void CheckCarrierIsSupported()
        {
            switch (simulatorArgs.ContainerStatusSourceCode)
            {
                case ContainerStatusSourceValues.Vizion:
                    CheckCarrierIsSupportedInVizion();
                    break;
                default:
                    break;
            }
        }

        private void CheckCarrierIsSupportedInVizion()
        {
            var vizionCarriers = new VizionService().GetAllCarriers();
            if(!vizionCarriers.Where(e=>e.carrier_code == simulatorArgs.CarrierCode).Any())
            {
                 simulatorArgs.Errors.Add("Carrier not supported");
            }
        }

        

        private Document AddRequstDocument()
        {
            DocumentRepository documentrepository = new DocumentRepository(CommonContext);
            var byteArray = ConvertObjectToByteArray(simulatorArgs);
            Document document = CreateDocument(simulatorArgs.Tenant, byteArray);
            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = CreateBlobFile(document, byteArray);
            storageservice.Write(byteArray, fileInfo);
            return document;
        }

        private byte[] ConvertObjectToByteArray(object simulatorArgs)
        {
            var objectText =  JsonConvert.SerializeObject(simulatorArgs);
            var jsonByteArray = Encoding.UTF8.GetBytes(objectText);
            return jsonByteArray;
        }

        private CommunicationLog AddRequstCommunicationLog(Document document)
        {
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(CommonContext);

            ObjectTable objectTable = GetObjectTableForSimulate();
            var commLog = CreateRequstCommunicationLog(objectTable, document);
            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();
            return commLog;
        }

        private ObjectTable GetObjectTableForSimulate()
        {
            ObjectTableRepository objecttableRep = new ObjectTableRepository(simulatorArgs.Tenant);

            if (simulatorArgs.IsFromContainer)
                return objecttableRep.GetObjectTableByName("Container", 0, true);
            else
                return objecttableRep.GetObjectTableByName("Shipment", 0, true);
        }

        private CommunicationLog CreateRequstCommunicationLog(ObjectTable objectTable, Document document)
        {
            return new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", simulatorArgs.Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(simulatorArgs.Tenant),
                InOut = "O",
                ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                Subject = "Request Update Container Status",
                Tenant = simulatorArgs.Tenant,
                DocumentId = document.Id,
                CommunicationLogTypeCode = "Q",
                CommunicationStatusTypeCode = "W",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(simulatorArgs.Tenant),
                CreateDateUTC = DateTime.UtcNow,
                LastStatusDateUTC = DateTime.UtcNow,
                QueueName = GeneralRequestUpdateContainerStatusQueueName,
                Priority = 1,
                EntityId = simulatorArgs.IsFromContainer ? simulatorArgs.ContainerId : simulatorArgs.ShipmentId

            };
        }

        public void UpdateStatusFromVizion( VisionContainerStatus containerStatus)
        {
            var shipmentsContext = ShipmentsContext.GetContext(0);
            var allContainerTrackingRequests = shipmentsContext.ContainerTrackingRequests.Where(e => e.RequestId == containerStatus.reference_id && e.Status == ContainerTrackingRequestStatus.Active).ToList();
            foreach (var containerTrackingRequest in allContainerTrackingRequests)
            {
                BuildCommunicationLogUpdateStatus(containerStatus, containerTrackingRequest);
            }

        }

        private void BuildCommunicationLogUpdateStatus(VisionContainerStatus containerStatus, ContainerTrackingRequest containerTrackingRequest)
        {
            if (string.IsNullOrEmpty(containerTrackingRequest.ContainerId))
                FillContainerId(containerTrackingRequest, containerStatus);
            InitializeContext();
            using (TransactionScope scope = Simplog.Server.Infrastructure.Helpers.TransactionFactory.GetTransaction())
            {
                var byteArray = ConvertObjectToByteArray(containerStatus);
                var document = AddDocument(containerTrackingRequest.Tenant, byteArray);
                var commLog = AddResponseCommunicationLog(containerTrackingRequest, document);
                
                SendCommunicationLogMessage(commLog, containerTrackingRequest.Tenant);
                AddContainerTrackingResponse(commLog, containerTrackingRequest);
                scope.Complete();
            }
        }

        private void AddContainerTrackingResponse(CommunicationLog commLog, ContainerTrackingRequest containerTrackingRequest)
        {
            var containerTrackingResponse = CreateContainerTrackingResponse(commLog, containerTrackingRequest);
            var shipmentContext = ShipmentsContext.GetContext(containerTrackingRequest.Tenant);
            ContainerTrackingResponseService containerTrackingResponseService = new ContainerTrackingResponseService(shipmentContext, containerTrackingRequest.Tenant);
            containerTrackingResponseService.Create(containerTrackingResponse);
        }

        private ContainerTrackingResponsePM CreateContainerTrackingResponse(CommunicationLog commLog, ContainerTrackingRequest containerTrackingRequest)
        {
            return new ContainerTrackingResponsePM()
            {
                CommunicationLogId = commLog.Id,
                Tenant = containerTrackingRequest.Tenant,
                ContainerTrackingRequestId = containerTrackingRequest.Id,
            };
        }

        private void FillContainerId(ContainerTrackingRequest containerTrackingRequest, VisionContainerStatus containerStatus)
        {
            var shipmentContext = ShipmentsContext.GetContext(containerTrackingRequest.Tenant);
            var container = shipmentContext.Containers.Where(r => r.ShipmentId == containerTrackingRequest.ShipmentId && containerStatus.payload.container_id == r.ContainerNumber).FirstOrDefault();
            if (container == null)
                return;
            containerTrackingRequest.ContainerId = container.Id;
        }

        private Document AddDocument(int tenant, byte[] byteArray)
        {
            var commonContext = CommonDataContext.GetContext(tenant);
            DocumentRepository documentrepository = new DocumentRepository(commonContext);
            Document document = CreateDocument(tenant, byteArray);
            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = CreateBlobFile(document, byteArray);
            storageservice.Write(byteArray, fileInfo);
            return document;
        }
        private BlobFileInfo CreateBlobFile(Document document, byte[] byteData)
        {
            return new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,
                FileSize = byteData.Length,

            };
        }
        private Document CreateDocument(int tenant, byte[] byteData)
        {
            return new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "json",
                FileSize = byteData.Length,
                Tenant = tenant,
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = "ContainerTrackingStatus",
            };
        }
        private CommunicationLog AddResponseCommunicationLog(ContainerTrackingRequest containerTrackingRequest, Document document)
        {
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(CommonContext);
            ObjectTableRepository objecttableRep = new ObjectTableRepository(containerTrackingRequest.Tenant);
            var objectTableName = string.IsNullOrEmpty(containerTrackingRequest.ContainerId) ? "Shipment" : "Container";
            ObjectTable objectTable = objecttableRep.GetObjectTableByName(objectTableName, 0, true);
            var commLog = CreateResponseCommunicationLog(objectTable, containerTrackingRequest, document);
            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();
            return commLog;
        }
        private CommunicationLog CreateResponseCommunicationLog(ObjectTable objectTable, ContainerTrackingRequest containerTrackingRequest, Document document)
        {
            return new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", containerTrackingRequest.Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(containerTrackingRequest.Tenant),
                InOut = "I",
                //EntityId = OceanInsightsRequest.Id,
                ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                Subject = "General Update Container Status",
                Tenant = containerTrackingRequest.Tenant,
                CommunicationLogTypeCode = "Q",
                CommunicationStatusTypeCode = "W",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(containerTrackingRequest.Tenant),
                DocumentId = document.Id,
                CreateDateUTC = DateTime.UtcNow,
                LastStatusDateUTC = DateTime.UtcNow,
                QueueName = GeneralUpdateContainerStatusQueueName,
                Priority = 1,
                EntityId = string.IsNullOrEmpty(containerTrackingRequest.ContainerId) ? containerTrackingRequest.ShipmentId : containerTrackingRequest.ContainerId

            };
        }
        private void SendCommunicationLogMessage(CommunicationLog commLog, int tenant)
        {
            if (!string.IsNullOrEmpty(commLog.QueueName))
            {
                try
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, $"Before adding message to queue {commLog.QueueName} " + DateTime.Now.ToString(), null);
                    SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, tenant);
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, $"after adding message to queue  {commLog.QueueName} " + DateTime.Now.ToString(), null);
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;

                    if (!string.IsNullOrEmpty(ex.StackTrace))
                    {
                        errorMessage += Environment.NewLine + ex.StackTrace;
                    }

                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, $"Exception occured while adding message to queue {commLog.QueueName} " + DateTime.Now.ToString(), errorMessage);

                }
            }
        }
        private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(queueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } }, tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Vizion Update Container Status Fail", null, null);
            }
        }
        private void InitializeContext()
        {
            CommonContext = CommonDataContext.GetContext(tenant);
        }

        public UnsubscribeResult UnsubscribeFromVizion()
        {
            var containerTrackingRequest = GetContainerTrackingRequest();
            if (containerTrackingRequest == null)
                throw new Exception("There is no active request to unsubscribe");
            var result = new UnsubscribeResult() { message = "Reference unsubscribed successfully" };
            if (!simulatorArgs.IsSimulator)
                result = new VizionService().Unsubscribe(containerTrackingRequest);
            InActiveContainerTrackingRequest(containerTrackingRequest);
            return result;
        }

        private void InActiveContainerTrackingRequest(ContainerTrackingRequestPM containerTrackingRequest)
        {
            var shipmentContext = ShipmentsContext.GetContext(containerTrackingRequest.Tenant);
            ContainerTrackingRequestService containerTrackingRequestService = new ContainerTrackingRequestService(shipmentContext,containerTrackingRequest.Tenant);
            containerTrackingRequest.Status = ContainerTrackingRequestStatus.InActive;
            containerTrackingRequestService.Update(containerTrackingRequest);
        }

        private ContainerTrackingRequestPM GetContainerTrackingRequest()
        {
            ContainerTrackingRequestQuery containerTrackingRequestQuery = new ContainerTrackingRequestQuery(tenant);
            var containerTrackingRequest = containerTrackingRequestQuery.GetActiveRequest(this.simulatorArgs, tenant);
            return containerTrackingRequest;
        }
    }
}
