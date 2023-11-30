using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.APIDataContract;
using Logitude.BL.ShipmentsModel.CloseTables;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.ContainerTracking;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using RestSharp;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services.ContainerTraking
{
    public class RequestContainerStatusService
    {
        private readonly DbQueueService queueService;
        private readonly QueueResponse queueResponse;
        private ICommonDataContext Commoncontext;
        private CommunicationLog CommunicationLog;
        private CommunicationLogRepository CommunicationLogRep;
        private int Tenant;
        private GeneralContainerTrackingArgs containerTrackingArgs;
        private Shipment Shipment;
        private ShipmentMasterData ShipmentMasterData;
        private IShipmentsContext ShipmentContext;

        public RequestContainerStatusService(DbQueueService queueService, QueueResponse queueResponse)
        {
            this.queueService = queueService;
            this.queueResponse = queueResponse;

            InitiallizeFields();
        }

        private void InitiallizeFields()
        {
            Tenant = int.Parse(queueResponse.MessageValues["Tenant"]);
            Commoncontext = CommonDataContext.GetContext(Tenant);
            CommunicationLogRep = new CommunicationLogRepository(Commoncontext);
            CommunicationLog = GetCommunicationLog();
            containerTrackingArgs = GetContainerTrackingArgsFromDecuments();
            ShipmentContext = ShipmentsContext.GetContext(containerTrackingArgs.Tenant);
            Shipment = GetShipment();
            ShipmentMasterData = GetShipmentMasterData();
        }
        

        private CommunicationLog GetCommunicationLog()
        {
            var communicationLogId = queueResponse.MessageValues["CommunicationLogId"];
            CommunicationLog communicationLog = CommunicationLogRep.GetSingleCommunicationLog(communicationLogId, Tenant);
            return communicationLog;
        }

        private GeneralContainerTrackingArgs GetContainerTrackingArgsFromDecuments()
        {
            Logitude.Server.Tools.BlobFileInfo fileInfo = CreateBlobFileInfo();

            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            var datainByte = storageservice.Read(fileInfo);
            if (datainByte == null)
            {
                throw new Exception("GeneralContainerTrackingArgs document file not found!");
            }
            var datatext = Encoding.UTF8.GetString(datainByte);
            var args = JsonConvert.DeserializeObject<GeneralContainerTrackingArgs>(datatext);
            return args;
        }

        private BlobFileInfo CreateBlobFileInfo()
        {
            return new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = CommunicationLog.Document.Id,
                FolderName = CommunicationLog.Document.Folder,
                Extension = CommunicationLog.Document.Extension,
                Tenant = CommunicationLog.Document.Tenant,
                FileSize = CommunicationLog.Document.FileSize,
            };
        }

        public void ExecuteQueue()
        {
            try
            {
                UpdateContainerStatus();
                CompleteCommunicationLog();
            }
            catch (Exception e)
            {
                FailCommunicationLog(e.Message);
                throw e;
            }
        }

        private void UpdateContainerStatus()
        {
            switch (containerTrackingArgs.ContainerStatusSourceCode)
            {
                case ContainerStatusSourceValues.Vizion:
                    UpdateVizionContainerStatus();
                    break;

                default:
                    throw new Exception($"{containerTrackingArgs.ContainerStatusSourceCode} request handler not implemented yet");
            }
        }

        private void FailCommunicationLog(string message)
        {
            CommunicationLog.CommunicationStatusTypeCode = "F";
            CommunicationLog.ExceptionMessage = message;
            CommunicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(CommunicationLog.Tenant);
            CommunicationLog.LastStatusDateUTC = DateTime.UtcNow;
            CommunicationLogRep.Update(CommunicationLog);
            CommunicationLogRep.SubmitChanges();
        }

        private void CompleteCommunicationLog()
        {
            CommunicationLog.CommunicationStatusTypeCode = "D";
            CommunicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(CommunicationLog.Tenant);
            CommunicationLog.DoneDateUTC = DateTime.UtcNow;
            CommunicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(CommunicationLog.Tenant);
            CommunicationLog.LastStatusDateUTC = DateTime.UtcNow;
            CommunicationLogRep.Update(CommunicationLog);
            CommunicationLogRep.SubmitChanges();
        }

        private void UpdateVizionContainerStatus()
        {
            string requestId = CheckIfExistRequest(containerTrackingArgs.Tenant);
            var isExist = CheckIfExistOldRequest();
            if (requestId == null)
            {
                requestId = CreateNewRequest();
                isExist = false;
            }

            if (!isExist)
                AddContainerTrackingRequest(requestId);

            if (containerTrackingArgs.IsSimulator)
                SimulateVizionUpdateContainerStatus();
        }

        private string CreateNewRequest()
        {
            string requestId;
            if (containerTrackingArgs.IsSimulator)
            {
                VisionContainerStatus vizionContainerStatus = JsonConvert.DeserializeObject<VisionContainerStatus>(containerTrackingArgs.Data);
                requestId = containerTrackingArgs.IsFromContainer ? vizionContainerStatus.reference_id : vizionContainerStatus.parent_reference_id;
            }
            else
            {
                var result = new VizionService().SendRequest(containerTrackingArgs, Shipment);
                requestId = result.reference.id;
            }
            return requestId;
        }

        private string CheckIfExistRequest(int? tenant = null)
        {
            var previousReqesutQuery = ShipmentContext.ContainerTrackingRequests
                .Where(e=>e.Status == ContainerTrackingRequestStatus.Active && e.CarrierCode == containerTrackingArgs.CarrierCode).AsQueryable();

            if (containerTrackingArgs.IsFromContainer)
                previousReqesutQuery = previousReqesutQuery.Where(e => e.ContainerNumber == containerTrackingArgs.ContainerNumber || (e.Master == ShipmentMasterData.Master && e.ContainerNumber == null));
            else
                previousReqesutQuery = previousReqesutQuery.Where(e => e.Master == ShipmentMasterData.Master);

            if(tenant != null)
                previousReqesutQuery = previousReqesutQuery.Where(e => e.Tenant == tenant);

            if (!string.IsNullOrEmpty(containerTrackingArgs.ShipmentId))
                previousReqesutQuery = previousReqesutQuery.Where(e => e.ShipmentId == containerTrackingArgs.ShipmentId);

            var previousReqesut = previousReqesutQuery.FirstOrDefault();

            if (previousReqesut != null)
                return previousReqesut.RequestId;

            return null;
        }
        private bool CheckIfExistOldRequest()
        {
            var previousReqesutQuery = ShipmentContext.ContainerTrackingRequests
                .Where(e => e.Status == ContainerTrackingRequestStatus.Active && e.CarrierCode == containerTrackingArgs.CarrierCode).AsQueryable();
            if (containerTrackingArgs.IsFromContainer)
                previousReqesutQuery = previousReqesutQuery.Where(e => e.ContainerId == containerTrackingArgs.ContainerId || (e.ShipmentId == containerTrackingArgs.ShipmentId && e.ContainerNumber == null));
            else
                previousReqesutQuery = previousReqesutQuery.Where(e => e.ShipmentId == containerTrackingArgs.ShipmentId);

            previousReqesutQuery = previousReqesutQuery.Where(e => e.Tenant == containerTrackingArgs.Tenant);

            var previousReqesut = previousReqesutQuery.FirstOrDefault();

            if (previousReqesut != null)
                return true;

            return false;
        }
        private void SimulateVizionUpdateContainerStatus()
        {
            var source = GetSource();
            VisionContainerStatus vizionContainerStatus = JsonConvert.DeserializeObject<VisionContainerStatus>(containerTrackingArgs.Data);
            var result = APICaller.CallApi<object>(source.CallbackURL, vizionContainerStatus, Method.POST);
        }

        private void AddContainerTrackingRequest(string requestId)
        {
            var containerTrackingRequests = CreateContainerTrackingRequests(requestId);
            ContainerTrackingRequestService containerTrackingRequestService = new ContainerTrackingRequestService(ShipmentContext, containerTrackingArgs.Tenant);
            containerTrackingRequestService.Create(containerTrackingRequests);
        }

        private ContainerTrackingRequestPM CreateContainerTrackingRequests(string requestId)
        {
            var provider = containerTrackingArgs.ContainerStatusSourceCode;
            if (!string.IsNullOrEmpty(containerTrackingArgs?.ContainerStatusSourceCode) && containerTrackingArgs.ContainerStatusSourceCode.Equals("2", StringComparison.InvariantCultureIgnoreCase))
            {
                provider = "VZN";
            }

            return new ContainerTrackingRequestPM()
            {
                ContainerNumber = containerTrackingArgs.IsFromContainer ? containerTrackingArgs.ContainerNumber : null,
                Master = ShipmentMasterData.Master,
                Tenant = containerTrackingArgs.Tenant,
                Provider = provider,
                RequestId = requestId,
                ShipmentId = Shipment.Id,
                Status = ContainerTrackingRequestStatus.Active,
                CarrierCode = containerTrackingArgs.CarrierCode,
                ScacCode = containerTrackingArgs.ScacCode,
                ContainerId = containerTrackingArgs.ContainerId,
                IsSimulate = containerTrackingArgs.IsSimulator
            };
        }

        private Shipment GetShipment()
        {
            var shipment = ShipmentContext.Shipments
                .Include(e => e.ShipmentMasterData.MainCarriageCarrierCard)
                .Where(e => e.Id == containerTrackingArgs.ShipmentId && e.Tenant == containerTrackingArgs.Tenant).FirstOrDefault();
            return shipment;
        }

        private ShipmentMasterData GetShipmentMasterData()
        {
            var masterID = Shipment.Id;
            if (Shipment.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(Shipment.MasterShipmentDataId))
            {
                masterID = Shipment.MasterShipmentDataId;
            }
            var shipmentMasterData = ShipmentContext.ShipmentMasterDatas.Include(e => e.MainCarriageCarrierCard).Where(e => e.Id == masterID && e.Tenant == containerTrackingArgs.Tenant).FirstOrDefault();
            return shipmentMasterData;
        }
        private ContainerTrackingProvider GetSource()
        {
            var containerTrackingProviderRepository = new ContainerTrackingProviderRepository(ShipmentContext);
            var providerSetting = containerTrackingProviderRepository.GetBySourceCode(containerTrackingArgs.ContainerStatusSourceCode);
            return providerSetting;
        }
    }
}
