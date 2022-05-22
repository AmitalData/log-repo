using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.APIDataContract;
using Logitude.BL.ShipmentsModel.CloseTables;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using RestSharp;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
        private int Tenant;
        private GeneralContainerStatusSimulatorArgs ContainerStatusSimulatorArgs;
        private Shipment Shipment;
        private IShipmentsContext ShipmentContext;



        public RequestContainerStatusService(DbQueueService queueService, QueueResponse queueResponse)
        {
            this.queueService = queueService;
            this.queueResponse = queueResponse;

            InitiallizeFields();
            InitiallizeContext();

        }

        private void InitiallizeContext()
        {
            ShipmentContext = ShipmentsContext.GetContext(ContainerStatusSimulatorArgs.Tenant);
        }

        private void InitiallizeFields()
        {
            Tenant = int.Parse(queueResponse.MessageValues["Tenant"]);
            Commoncontext = CommonDataContext.GetContext(Tenant); ;
            CommunicationLog = GetCommunicationLog();
            ContainerStatusSimulatorArgs = GetContainerStatusSimulatorArgsFromDecuments();
            Shipment = GetShipment();

        }

        private CommunicationLog GetCommunicationLog()
        {
            var communicationLogId = queueResponse.MessageValues["CommunicationLogId"];
            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(Commoncontext);
            CommunicationLog communicationLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, Tenant);
            return communicationLog;
        }

        private GeneralContainerStatusSimulatorArgs GetContainerStatusSimulatorArgsFromDecuments()
        {
            Logitude.Server.Tools.BlobFileInfo fileInfo = CreateBlobFileInfo();

            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            var datainByte = storageservice.Read(fileInfo);
            if (datainByte == null)
            {
                throw new Exception("GeneralContainerStatusSimulatorArgs document file not found!");
            }
            MemoryStream ms = new MemoryStream(datainByte);
            using (BsonDataReader reader = new BsonDataReader(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                var args = serializer.Deserialize<GeneralContainerStatusSimulatorArgs>(reader);
                return args;
            }


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
            switch (ContainerStatusSimulatorArgs.ContainerStatusSourceCode)
            {
                case ContainerStatusSourceValues.Vizion:
                    UpdateVizionContainerStatus();
                    break;
                default:
                    throw new Exception($"{ContainerStatusSimulatorArgs.ContainerStatusSourceCode} request handler not implemented yet");
            }
        }

        private void UpdateVizionContainerStatus()
        {
            string requestId;
            var source = GetSourceByCode(ContainerStatusSimulatorArgs);

            if (ContainerStatusSimulatorArgs.IsSimulator)
            {
                requestId = "simulate-" + Guid.NewGuid().ToString();
            }
            else
            {
                var result = new VizionService().SendRequest(ContainerStatusSimulatorArgs, source, Shipment);
                requestId = result.reference.id;
            }
            AddContainerTrackingRequest(requestId);
            if (ContainerStatusSimulatorArgs.IsSimulator)
                SimulateVizionUpdateContainerStatus(source);
            
        }

        private void SimulateVizionUpdateContainerStatus(ContainerTrackingProvider source)
        {
            VisionContainerStatus vizionContainerStatus = JsonConvert.DeserializeObject<VisionContainerStatus>(ContainerStatusSimulatorArgs.Data);

            var result = APICaller.CallApi<object>(source.CallbackURL, vizionContainerStatus, Method.POST);
        }

        private void AddContainerTrackingRequest(string requestId)
        {
            var containerTrackingRequests = CreateContainerTrackingRequests(requestId);
            ContainerTrackingRequestService containerTrackingRequestService = new ContainerTrackingRequestService(ShipmentContext, ContainerStatusSimulatorArgs.Tenant);
            containerTrackingRequestService.Create(containerTrackingRequests);
        }

        private ContainerTrackingRequestPM CreateContainerTrackingRequests(string requestId)
        {
            return new ContainerTrackingRequestPM()
            {
                ContainerNumber = ContainerStatusSimulatorArgs.IsFromContainer ? ContainerStatusSimulatorArgs.ContainerNumber : null,
                Master = Shipment.ShipmentMasterData.Master,
                Tenant = ContainerStatusSimulatorArgs.Tenant,
                Provider = ContainerStatusSimulatorArgs.ContainerStatusSourceCode,
                RequestId = requestId
            };
        }

        private Shipment GetShipment()
        {
            var shipmentsContext = ShipmentsContext.GetContext(ContainerStatusSimulatorArgs.Tenant);
            var shipment = shipmentsContext.Shipments
                .Include(e=>e.ShipmentMasterData.MainCarriageCarrierCard)
                .Where(e => e.Id == ContainerStatusSimulatorArgs.ShipmentId && e.Tenant == ContainerStatusSimulatorArgs.Tenant).FirstOrDefault();
            return shipment;
        }

        private ContainerTrackingProvider GetSourceByCode(GeneralContainerStatusSimulatorArgs containerStatusSimulatorArgs)
        {
            var containerTrackingProviderRepository = new ContainerTrackingProviderRepository(ShipmentContext);
            var providerSetting = containerTrackingProviderRepository.GetBySourceCode(containerStatusSimulatorArgs.ContainerStatusSourceCode);
            return providerSetting;
        }
    }
}
