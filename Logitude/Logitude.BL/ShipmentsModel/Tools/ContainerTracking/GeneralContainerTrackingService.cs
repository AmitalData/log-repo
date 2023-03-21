using Logitude.BL.DataContracts;
using Logitude.BL.ShipmentsModel.APIDataContract;
using Logitude.BL.ShipmentsModel.CloseTables;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Logitude.BL.ShipmentsModel.Tools.ContainerTracking
{
    public class GeneralContainerTrackingService
    {
        private readonly string GeneralRequestUpdateContainerStatusQueueName = "GeneralRequestUpdateContainerStatus";
        private readonly ICommonDataContext CommonContext;

        private readonly GeneralContainerTrackingArgs generalContainerTrackingArgs;
        private List<VizionCarrier> supportedCarriers;
        private readonly int tenant;
        private readonly ContainerSettingRepository containerSettingRepository;

        public GeneralContainerTrackingService(GeneralContainerTrackingArgs containerTrackingArgs)
        {
            if(!string.IsNullOrEmpty(containerTrackingArgs?.ContainerStatusSourceCode) && containerTrackingArgs.ContainerStatusSourceCode.Equals("VZN", StringComparison.InvariantCultureIgnoreCase))
            {
                containerTrackingArgs.ContainerStatusSourceCode = "2";
            }
            this.generalContainerTrackingArgs = containerTrackingArgs;
            this.tenant = containerTrackingArgs.Tenant;
            CommonContext = CommonDataContext.GetContext(tenant);
            containerSettingRepository = new ContainerSettingRepository(tenant);
        }

        public GeneralContainerTrackingService()
        {

        }

        public void TrackContainer()
        {
            SetArgsFields();
            ValidateAndTrack();
        }

        public void AutomaticTrackContainer()
        {
            TenantManagement tenantManagement = GetTenantManagement();
            if (tenantManagement?.IsContainerTrackingPrepaid != true) return;
            if (generalContainerTrackingArgs.IsUpdatedFromRequest) return;

            try
            {
                SetArgsFields();
                if (!AllowAutomaticTrackContainer()) return;
                ValidateAndTrack();
            }
            catch (Exception) { }

        }

        private TenantManagement GetTenantManagement()
        {
            TenantManagement tenantManagement;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
                scope.Complete();
            }
            return tenantManagement;
        }

        private void ValidateAndTrack()
        {
            if (!generalContainerTrackingArgs.IsSimulator) 
                supportedCarriers = GetSupportedCarriers();
            if (!CheckValidation()) return;
            SetCarriarCode();

            using (TransactionScope scope = Simplog.Server.Infrastructure.Helpers.TransactionFactory.GetTransaction())
            {
                var document = AddRequstDocument();
                var commLog = AddRequstCommunicationLog(document);
                SendCommunicationLogMessage(commLog, generalContainerTrackingArgs.Tenant);
                this.UpdateContainer();
                scope.Complete();
            }
        }

        private bool AllowAutomaticTrackContainer()
        {
            var containerSettings = containerSettingRepository.GetAll(tenant).FirstOrDefault();
            if (containerSettings == null || !containerSettings.AddedManually) return BeforeActivationDate(containerSettings.ActivationDate) && CheckAutomaticTrackContainerFromTenantZero();
            if (!AutomaticTrackContainerDirectionAllowd(containerSettings, generalContainerTrackingArgs.DirectionId)) return false;
            return BeforeActivationDate(containerSettings.ActivationDate) && CommonContext.ShippingLines.Any(x => x.Tenant == tenant && x.SCACCode == generalContainerTrackingArgs.ScacCode && x.IsAutomaticRequestsSent);
        }

        private bool BeforeActivationDate(DateTime? activationDate)
        {
            if (activationDate == null || generalContainerTrackingArgs.ShipmentCreateDateTime == null) return false;
            return generalContainerTrackingArgs.ShipmentCreateDateTime.Value.Date >= activationDate.Value.Date;
        }

        private bool CheckAutomaticTrackContainerFromTenantZero()
        {
            return CommonContext.ShippingLines.Any(x => x.Tenant == 0 && x.SCACCode == generalContainerTrackingArgs.ScacCode && x.IsAutomaticRequestsSent);
        }

        private bool AutomaticTrackContainerDirectionAllowd(ContainerSetting containerSettings, string directionId)
        {
            switch (directionId)
            {
                case "E": return containerSettings.IsExport;
                case "I": return containerSettings.IsImport;
                case "D": return containerSettings.IsDomestic;
                case "R": return containerSettings.IsDrop;
                default: return false;
            }
        }

        private void SetCarriarCode()
        {
            if (generalContainerTrackingArgs.IsSimulator) return;
            var carrier = supportedCarriers.Where(e => e.scac == generalContainerTrackingArgs.ScacCode).FirstOrDefault();
            generalContainerTrackingArgs.CarrierCode = carrier.carrier_code;
        }

        private List<VizionCarrier> GetSupportedCarriers()
        {
            switch (generalContainerTrackingArgs.ContainerStatusSourceCode)
            {
                case ContainerStatusSourceValues.Vizion:
                    return new VizionService().GetAllCarriers();
                default:
                    throw new Exception("Get Supported Carriers not implement in this Source");
            }
        }

        private void SetArgsFields()
        {
            var context = ShipmentsContext.GetContext(generalContainerTrackingArgs.Tenant);
            var shipment = context.Shipments.Where(e => e.Id == generalContainerTrackingArgs.ShipmentId).FirstOrDefault();
            var masterID = generalContainerTrackingArgs.ShipmentId;
            if (shipment.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(shipment.MasterShipmentDataId))
            {
                masterID = shipment.MasterShipmentDataId;
            }
            var shipmentMasterData = context.ShipmentMasterDatas.Include(e => e.MainCarriageCarrierCard)
                .Include(e => e.MainCarriageCarrierCard.ShippingLine).Where(e => e.Id == masterID && e.Tenant == generalContainerTrackingArgs.Tenant).FirstOrDefault();

            generalContainerTrackingArgs.CarrierId = shipmentMasterData?.MainCarriageCarrierId;
            generalContainerTrackingArgs.ScacCode = shipmentMasterData?.MainCarriageCarrierCard?.ShippingLine?.SCACCode;
            generalContainerTrackingArgs.Master = shipmentMasterData?.Master;
            generalContainerTrackingArgs.ShipmentCreateDateTime = shipment?.CreateDateTime;
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrEmpty(generalContainerTrackingArgs.CarrierId)) generalContainerTrackingArgs.Errors.Add("Carrier is missing");
            if (string.IsNullOrEmpty(generalContainerTrackingArgs.ScacCode)) generalContainerTrackingArgs.Errors.Add("Carrier Scac Code is missing");
            if (string.IsNullOrEmpty(generalContainerTrackingArgs.Data) && generalContainerTrackingArgs.IsSimulator) generalContainerTrackingArgs.Errors.Add("Response is missing");
            if (generalContainerTrackingArgs.IsFromContainer && string.IsNullOrEmpty(generalContainerTrackingArgs.ContainerNumber)) generalContainerTrackingArgs.Errors.Add("Container Number is missing");
            if (!generalContainerTrackingArgs.IsSimulator) CheckCarrierIsSupported();
            if (!generalContainerTrackingArgs.IsFromContainer && string.IsNullOrEmpty(generalContainerTrackingArgs.Master)) generalContainerTrackingArgs.Errors.Add("Master Number is missing");
            if (generalContainerTrackingArgs.Errors.Count > 0) generalContainerTrackingArgs.Success = false;
            return generalContainerTrackingArgs.Success;
        }

        private void CheckCarrierIsSupported()
        {
            if (!CommonContext.ShippingLines.Any(x => x.Tenant == 0 && x.SCACCode == generalContainerTrackingArgs.ScacCode && x.IsSupportsContainerTracking))
            {
                generalContainerTrackingArgs.Errors.Add("Carrier not supported");
                return;
            }
            var carrier = supportedCarriers.Where(e => e.scac == generalContainerTrackingArgs.ScacCode).FirstOrDefault();
            if (carrier == null) generalContainerTrackingArgs.Errors.Add("Carrier not supported");
        }

        private Document AddRequstDocument()
        {
            DocumentRepository documentrepository = new DocumentRepository(CommonContext);
            var byteArray = ConvertObjectToByteArray(generalContainerTrackingArgs);
            Document document = CreateDocument(generalContainerTrackingArgs.Tenant, byteArray);
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

        private byte[] ConvertObjectToByteArray(object containerTrackingArgs)
        {
            var objectText = JsonConvert.SerializeObject(containerTrackingArgs);
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
            ObjectTableRepository objecttableRep = new ObjectTableRepository(generalContainerTrackingArgs.Tenant);

            if (generalContainerTrackingArgs.IsFromContainer)
                return objecttableRep.GetObjectTableByName("Container", 0, true);
            else
                return objecttableRep.GetObjectTableByName("Shipment", 0, true);
        }

        private CommunicationLog CreateRequstCommunicationLog(ObjectTable objectTable, Document document)
        {
            return new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", generalContainerTrackingArgs.Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(generalContainerTrackingArgs.Tenant),
                InOut = "O",
                ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                Subject = "Request Update Container Status",
                Tenant = generalContainerTrackingArgs.Tenant,
                DocumentId = document.Id,
                CommunicationLogTypeCode = "Q",
                CommunicationStatusTypeCode = "W",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(generalContainerTrackingArgs.Tenant),
                CreateDateUTC = DateTime.UtcNow,
                LastStatusDateUTC = DateTime.UtcNow,
                QueueName = GeneralRequestUpdateContainerStatusQueueName,
                Priority = 1,
                EntityId = generalContainerTrackingArgs.IsFromContainer ? generalContainerTrackingArgs.ContainerId : generalContainerTrackingArgs.ShipmentId

            };
        }

        public void UpdateStatusFromVizion(VisionContainerStatus containerStatus)
        {
            this.InsertNewAnalyzeQueue(containerStatus);
        }
        private void InsertNewAnalyzeQueue(VisionContainerStatus containerStatus)
        {
            IGlobalContext globalContext = GlobalContext.GetContext();
            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository(globalContext);
            byte[] analyzeQueueMessageBody = this.ConvertObjectToByteArray(containerStatus);

            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "GeneralContainerTrackingReceiver",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = analyzeQueueMessageBody,
                Status = "W",

                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = false,
                FileSize = analyzeQueueMessageBody.Length,
                Tenant = 0,
            };
            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
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
                    string activity = "(A) Container Automatic Request Sent";
                    AddTotangoActivity(tenant, activity);
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
        private void AddTotangoActivity(int tenant, string activityDescription)
        {
            string email = AuthenticationUtil.IsAuthenticatedUserExists() ? AuthenticationUtil.GetAuthenticatedUser() : "system@tenant" + tenant + ".com";
            string moduleName = "(A) Container";
            ActivityLogger.SendTotangoContactActivity(email, moduleName, activityDescription, tenant, false, null);
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
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Container Tracking Status Fail", null, null);
            }
        }
        private void UpdateContainer()
        {
            if (!generalContainerTrackingArgs.IsFromContainer) return;
            if (string.IsNullOrEmpty(generalContainerTrackingArgs.ContainerId)) return;

            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(generalContainerTrackingArgs.Tenant);
            ContainerRepository containerRepository = new ContainerRepository(shipmentsContext);
            ContainerQuery containerQuery = new ContainerQuery(containerRepository);
            ContainerPM container = containerQuery.GetSinglePM(generalContainerTrackingArgs.ContainerId, generalContainerTrackingArgs.Tenant);

            if (container == null) return;
            if (container.RequestDate == null)
                container.RequestDate = TenantServerConfigration.GetCurrentDateTime(generalContainerTrackingArgs.Tenant);

            container.IsUpdatedFromRequest = true;
            ContainerService containerService = new ContainerService(shipmentsContext, generalContainerTrackingArgs.Tenant);
            containerService.Update(container);
        }

        public UnsubscribeResult UnsubscribeFromVizion()
        {
            var containerTrackingRequest = GetContainerTrackingRequest();
            if (containerTrackingRequest == null)
                throw new Exception("There is no active request to unsubscribe");
            var result = new UnsubscribeResult() { message = "Reference unsubscribed successfully" };
            if (!generalContainerTrackingArgs.IsSimulator)
                result = new VizionService().Unsubscribe(containerTrackingRequest);
            InActiveContainerTrackingRequest(containerTrackingRequest);
            return result;
        }

        private void InActiveContainerTrackingRequest(ContainerTrackingRequestPM containerTrackingRequest)
        {
            var shipmentContext = ShipmentsContext.GetContext(containerTrackingRequest.Tenant);
            ContainerTrackingRequestService containerTrackingRequestService = new ContainerTrackingRequestService(shipmentContext, containerTrackingRequest.Tenant);
            containerTrackingRequest.Status = ContainerTrackingRequestStatus.InActive;
            containerTrackingRequestService.Update(containerTrackingRequest);
        }

        private ContainerTrackingRequestPM GetContainerTrackingRequest()
        {
            ContainerTrackingRequestQuery containerTrackingRequestQuery = new ContainerTrackingRequestQuery(tenant);
            var containerTrackingRequest = containerTrackingRequestQuery.GetActiveRequest(this.generalContainerTrackingArgs, tenant);
            return containerTrackingRequest;
        }
    }
}
