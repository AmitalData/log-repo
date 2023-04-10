using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.CloseTables;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Xml.Serialization;

namespace WebFreight.Web.ContainerTracking
{
    public class ContainerTrackingGeneralAnalyzer
    {
        private AnalyzeQueue analyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        private CommunicationLogRepository communicationLogRepository;
        private int tenant_Zero;
        private ArrayOfQueueTask externalTasksQueues;
        private VisionContainerStatus visionContainerStatus;
        private int? logitudeTenant = null;
        private string trackingSource;
        private ContainerUpdatedFields containerUpdatedFields;
        private ContainerTrackingHelper containerTrackingHelper;
        public ContainerTrackingGeneralAnalyzer(string trackingSource, AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
        {
            if (analyzeQueue != null)
            {
                this.tenant_Zero = analyzeQueue.Tenant;
                this.analyzeQueue = analyzeQueue;
                this.analyzeQueueRepository = analyzeQueueRepository;
                this.trackingSource = trackingSource;
            }
        }

        public void Run()
        {
            if (analyzeQueue != null)
            {
                this.Deserialize();
            }
        }
        private void Deserialize()
        {
            try
            {
                this.AnalyzeMessageBody();
            }

            catch (Exception ex)
            {
                analyzeQueue.Status = "F";
                analyzeQueue.ErrorMessage = "ContainerStatusesConnecterAnalyzer failed: " + ex.Message;
                analyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(analyzeQueue.Tenant);
                analyzeQueueRepository.Update(analyzeQueue);
                analyzeQueueRepository.SubmitChanges();
                throw ex;
            }

            this.AnalyzeData();
        }
        private void AnalyzeMessageBody()
        {
            if (trackingSource == ContainerStatusSourceValues.OceanInsights)
            {
                MemoryStream memorystream = new MemoryStream(analyzeQueue.MessageBody);
                XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfQueueTask));
                externalTasksQueues = (ArrayOfQueueTask)serializer.Deserialize(memorystream);
            }

            else if (trackingSource == ContainerStatusSourceValues.Vizion)
            {
                var datatext = Encoding.UTF8.GetString(analyzeQueue.MessageBody);
                visionContainerStatus = JsonConvert.DeserializeObject<VisionContainerStatus>(datatext);
            }
        }
        private void AnalyzeData()
        {
            try
            {
                this.ConnectAnalyzeQueueToTenantAndEntity();

                if (trackingSource == ContainerStatusSourceValues.OceanInsights && externalTasksQueues != null)
                {
                    OceanInsightAnalyzer oceanInsightAnalyzer = new OceanInsightAnalyzer(externalTasksQueues);
                    containerUpdatedFields = oceanInsightAnalyzer.Run();
                }

                else if (trackingSource == ContainerStatusSourceValues.Vizion && visionContainerStatus != null)
                {
                    VizionAnalyzer vizionAnalyzer = new VizionAnalyzer(visionContainerStatus);
                    containerUpdatedFields = vizionAnalyzer.Run();
                }

                if (containerUpdatedFields != null)
                {
                    containerUpdatedFields.TrackingSource = trackingSource;
                    this.StartUpdating();
                }

                var tenant = this.tenant_Zero;
                if (containerUpdatedFields != null) tenant = containerUpdatedFields.Tenant;

                this.DoneAnalyzeQueue(tenant);
            }

            catch (Exception ex)
            {
                this.OnCatchAnalyzingError(ex);
                throw ex;
            }
        }
        private void ConnectAnalyzeQueueToTenantAndEntity()
        {
            if (!analyzeQueue.ConnectedToTenant)            
                analyzeQueue.ConnectedToTenant = true;                
            
            if (!analyzeQueue.ConnectedToEntity)            
                analyzeQueue.ConnectedToEntity = true;

            analyzeQueueRepository.Update(analyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }
        private void StartUpdating()
        {
            ContainerTrackingUpdateManager manager = new ContainerTrackingUpdateManager(containerUpdatedFields);

            if (trackingSource == ContainerStatusSourceValues.OceanInsights)
            {
                manager.Initialize(containerUpdatedFields.Tenant);
                manager.Update(true);
            }

            else if (trackingSource == ContainerStatusSourceValues.Vizion)
            {
                List<ContainerTrackingRequest> allContainerTrackingRequests = containerUpdatedFields.ShipmentContext.ContainerTrackingRequests.Where(e => (e.RequestId == visionContainerStatus.reference_id || e.RequestId == visionContainerStatus.parent_reference_id) && e.Status == ContainerTrackingRequestStatus.Active).ToList();
                foreach (var containerTrackingRequest in allContainerTrackingRequests)
                {
                    HandelUpdateManager(manager, containerTrackingRequest);
                }
            }
        }

        private void HandelUpdateManager(ContainerTrackingUpdateManager manager, ContainerTrackingRequest containerTrackingRequest)
        {
            string containerNumber = this.GetContainerNumber(visionContainerStatus.payload.container_id);
            Container myRequestContainer = this.GetContainer(containerTrackingRequest.Tenant, containerTrackingRequest.ShipmentId, containerNumber);

            string entityId = myRequestContainer == null ? containerTrackingRequest.ShipmentId : myRequestContainer.Id;
            string objectTableName = myRequestContainer == null ? "Shipment" : "Container";

            CommunicationLog comunicationLog = BuildCommunicationLogUpdateStatus(containerTrackingRequest, entityId, objectTableName);
            

  
            if (myRequestContainer == null)
            {
                SetComunicationLogDone(comunicationLog, false);
                return;
            }

            try
            {
                string containerId = myRequestContainer.Id;
                this.Update(containerId, manager, containerTrackingRequest, comunicationLog);                
            }
            catch (Exception ex)
            {
                this.HandleExceptionOnUpdate(comunicationLog, ex);
            }
        }
        private void Update(string containerId, ContainerTrackingUpdateManager manager, ContainerTrackingRequest containerTrackingRequest, CommunicationLog comunicationLog)
        {
            containerTrackingHelper = new ContainerTrackingHelper(containerTrackingRequest.Tenant);
            var analyz = false;

            ContainerPM container = GetContainerPM(containerId, containerTrackingRequest.Tenant);             

            if (IsValidToAnalyze(container))
            {
                manager.Initialize(containerTrackingRequest.Tenant);
                manager.SetContainer(container);
                MapContainersExternalData(container);
                manager.Update(IsUpdateContainerAllowed(container));
                analyz = IsUpdateContainerAllowed(container);
            }

            SetComunicationLogDone(comunicationLog, analyz);
        }

        private bool IsUpdateContainerAllowed(ContainerPM container)
        {
            if (!containerTrackingHelper.IsSameLocation(container.POLLocationPortId, containerUpdatedFields.POLLocation))            
                return false;
            
            if (!containerTrackingHelper.IsSameLocation(container.PODLocationPortId, containerUpdatedFields.PODLocation))            
                return false;            

            return true;
        }
        private bool IsUpdateShipmentAllowed(ShipmentPM shipment)
        {
            if (shipment.IsOperationalClosed)
                return false;

            if (!containerTrackingHelper.IsSameLocation(shipment.MainCarriageFromPortId, containerUpdatedFields.POLLocation))
                return false;

            if (!containerTrackingHelper.IsSameLocation(shipment.MainCarriageFinalDestinationPortId, containerUpdatedFields.PODLocation))
                return false;

            return true;
        }   

        private bool IsValidToAnalyze(ContainerPM container)
        {
            return (
                    string.IsNullOrEmpty(visionContainerStatus?.payload?.bill_of_lading) ||
                    string.IsNullOrEmpty(container.Master) ||
                    visionContainerStatus?.payload?.bill_of_lading == container.Master
                    ) &&
                    !container.IsCancelled &&
                    !container.IsClosed;
        }

        private void HandleExceptionOnUpdate(CommunicationLog comunicationLog, Exception exception)
        {
            comunicationLog.WasAnalyzed = false;
            comunicationLog.CommunicationStatusTypeCode = "F";
            comunicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(comunicationLog.Tenant);
            comunicationLog.LastStatusDateUTC = DateTime.UtcNow;
            comunicationLog.ExceptionMessage = exception.Message;

            if (exception.StackTrace != null)
            {
                comunicationLog.ExceptionMessage = comunicationLog.ExceptionMessage + Environment.NewLine + "Stack Trace: " + exception.StackTrace;
            }

            var communicationLogRepository = new CommunicationLogRepository(comunicationLog.Tenant);
            communicationLogRepository.Update(comunicationLog);
            communicationLogRepository.SubmitChanges();
            throw exception;
        }

        private void SetComunicationLogDone(CommunicationLog comunicationLog, bool wasAnalyzed)
        {
            comunicationLog.WasAnalyzed = wasAnalyzed;
            comunicationLog.CommunicationStatusTypeCode = "D";
            var communicationLogRepository = new CommunicationLogRepository(comunicationLog.Tenant);
            communicationLogRepository.Update(comunicationLog);
            communicationLogRepository.SubmitChanges();

   
            string activity = "";
            if (wasAnalyzed)
            {
                activity = "(A) Analyzed Responses";
                AddTotangoActivity(comunicationLog.Tenant, activity);
            }

            activity = "(A) Received Responses";
            AddTotangoActivity(comunicationLog.Tenant, activity);
        }

        private string GetContainerNumber(string container_id)
        {
            string containerNumber = container_id;

            if (container_id.Contains("-"))
            {
                var dash_index = container_id.LastIndexOf('-');
                var value_after_dash = container_id.ElementAt(dash_index + 1);
                if (value_after_dash != null && value_after_dash.ToString().Length == 1)
                {
                    containerNumber = container_id.Remove(dash_index, 1);
                }
            }

            return containerNumber;
        }
        private void MapContainersExternalData(ContainerPM container)
        {
            if (container == null)
            {
                containerUpdatedFields.ContainersExternal = null;
                return;
            }

            var containersExternalDataRepository = new ContainersExternalDataRepository(containerUpdatedFields.ShipmentContext);
            var containersExternalData_DB = containersExternalDataRepository.GetSingleContainersExternalData(container.Id, container.Tenant);
            if (containersExternalData_DB == null)
            {
                containersExternalData_DB = new ContainersExternalData() { Id = container.Id, Tenant = container.Tenant };
                containerUpdatedFields.ContainersExternal.IsNew = true;
            }

            containerUpdatedFields.ContainersExternal.ContainersExternalData_DB = containersExternalData_DB;
            containerUpdatedFields.ContainersExternal.ContainersExternalData_New.Id = container.Id;
            containerUpdatedFields.ContainersExternal.ContainersExternalData_New.Tenant = container.Tenant;
        }

        private ContainerPM GetContainerPM(string containerId, int tenant)
        {
            var containerRepository = new ContainerRepository(containerUpdatedFields.ShipmentContext);
            var containerQuery = new ContainerQuery(containerRepository);
            var containerPM = containerQuery.GetSinglePM(containerId, tenant);
            return containerPM;
        }

        private void DoneAnalyzeQueue(int tenant)
        {
            analyzeQueue.Status = "D";
            analyzeQueue.ErrorMessage = null;
            analyzeQueueRepository.Update(analyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }
        public void AddTotangoActivity(int tenant, string activity)
        {
            string activityDescription = activity;
            
            string email = AuthenticationUtil.IsAuthenticatedUserExists() ? AuthenticationUtil.GetAuthenticatedUser() : "system@tenant" + tenant + ".com";
            string moduleName = "(A) Container";
            ActivityLogger.SendTotangoContactActivity(email, moduleName, activityDescription, tenant,false,null);           
        }

        private void OnCatchAnalyzingError(Exception ex)
        {
            analyzeQueue.ErrorMessage = ex.Message + (ex.InnerException != null ? Environment.NewLine + "InnerException: " + ex.InnerException.Message : "");
            analyzeQueue.StackTrace = (ex.StackTrace != null ? Environment.NewLine + "Stack Trace: " + ex.StackTrace : "");
            analyzeQueue.ErrorMessage = analyzeQueue.ErrorMessage.Length > 7950 ? analyzeQueue.ErrorMessage.Substring(0, 7950) : analyzeQueue.ErrorMessage;
            analyzeQueue.StackTrace = analyzeQueue.StackTrace.Length > 7950 ? analyzeQueue.StackTrace.Substring(0, 7950) : analyzeQueue.StackTrace;

            if (ex.Message.StartsWith("--") || trackingSource != ContainerStatusSourceValues.OceanInsights)
            {
                analyzeQueue.Status = "F";
            }

            else
            {
                analyzeQueue.Retries++;

                if (analyzeQueue.Retries >= 5)
                {
                    analyzeQueue.Status = "F";
                }
            }

            if (analyzeQueue.Status == "F")
            {
                if (analyzeQueue.ConnectedToTenant && analyzeQueue.CommunicationLogId != null)
                {
                    this.communicationLogRepository = new CommunicationLogRepository(this.tenant_Zero);
                    CommunicationLog commLog = communicationLogRepository.GetSingleCommunicationLog(analyzeQueue.CommunicationLogId, tenant_Zero);
                    if (commLog != null)
                    {
                        commLog.CommunicationStatusTypeCode = "F";
                        commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(logitudeTenant.Value);
                        commLog.LastStatusDateUTC = DateTime.UtcNow;
                        commLog.ExceptionMessage = analyzeQueue.ErrorMessage;

                        if (analyzeQueue.StackTrace != null)
                        {
                            commLog.ExceptionMessage = commLog.ExceptionMessage + Environment.NewLine + "Stack Trace: " + analyzeQueue.StackTrace;
                        }

                        communicationLogRepository.Update(commLog);
                        communicationLogRepository.SubmitChanges();
                    }
                }
            }

            analyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(analyzeQueue.Tenant);
            analyzeQueueRepository.Update(analyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }

        private CommunicationLog BuildCommunicationLogUpdateStatus(ContainerTrackingRequest containerTrackingRequest, string entityId, string objectTableName)
        {
            using (TransactionScope scope = Simplog.Server.Infrastructure.Helpers.TransactionFactory.GetTransaction())
            {
                var byteArray = ConvertObjectToByteArray(visionContainerStatus);
                var document = AddDocument(containerTrackingRequest.Tenant, byteArray);
                var commLog = AddResponseCommunicationLog(objectTableName, entityId, document);
                AddContainerTrackingResponse(commLog, containerTrackingRequest);
                scope.Complete();
                return commLog;
            }
        }
        private byte[] ConvertObjectToByteArray(object simulatorArgs)
        {
            var objectText = JsonConvert.SerializeObject(simulatorArgs);
            var jsonByteArray = Encoding.UTF8.GetBytes(objectText);
            return jsonByteArray;
        }
        private void AddContainerTrackingResponse(CommunicationLog commLog, ContainerTrackingRequest containerTrackingRequest)
        {
            var containerTrackingResponse = CreateContainerTrackingResponse(commLog, containerTrackingRequest);
            var shipmentContext = ShipmentsContext.GetContext(commLog.Tenant);
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

        private Container GetContainer(int tenant, string shipmentId, string containerNumber)
        {
            var shipmentContext = ShipmentsContext.GetContext(tenant);
            return shipmentContext.Containers.Where(r => r.ShipmentId == shipmentId && r.ContainerNumber == containerNumber).FirstOrDefault();
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
        private CommunicationLog AddResponseCommunicationLog(string objectTableName, string entityId, Document document)
        {
            var commonContext = CommonDataContext.GetContext(document.Tenant);

            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            ObjectTableRepository objecttableRep = new ObjectTableRepository(document.Tenant);
            ObjectTable objectTable = objecttableRep.GetObjectTableByName(objectTableName, 0, true);
            var commLog = CreateResponseCommunicationLog(objectTable, entityId, document);
            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();
            return commLog;
        }
        private CommunicationLog CreateResponseCommunicationLog(ObjectTable objectTable, string entityId, Document document)
        {
            return new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", document.Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(document.Tenant),
                InOut = "I",
                ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                Subject = "General Update Container Status",
                Tenant = document.Tenant,
                CommunicationLogTypeCode = "A",
                CommunicationStatusTypeCode = "W",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(document.Tenant),
                DocumentId = document.Id,
                CreateDateUTC = DateTime.UtcNow,
                LastStatusDateUTC = DateTime.UtcNow,
                Priority = 1,
                WasAnalyzed = false,
                EntityId = entityId,
            };
        }
    }

    [XmlRoot("ArrayOfQueueTask")]
    public class ArrayOfQueueTask
    {
        [XmlElement("QueueTask")]
        public List<QueueTask> QueueTask { get; set; }

        public ArrayOfQueueTask()
        {
            this.QueueTask = new List<QueueTask>();
        }
    }

    public class QueueTask
    {
        [XmlAttribute("action")]
        public string Action { get; set; }
        public List<Parameter> Parameters { get; set; }
    }

    public class ContainerUpdatedFields
    {
        public int Tenant;
        public IShipmentsContext ShipmentContext;
        public ContainerRepository ContainerRepository;
        public ShipmentPM ShipmentPM;
        public ContainerPM ContainerPM;
        public ContainersExternal ContainersExternal;
        public DateTime? MainCarriageETD;
        public DateTime? MainCarriageETA;
        public DateTime? MainCarriageATD;
        public DateTime? MainCarriageATA;
        public DateTime? EstimatedEmptyPickupDate;
        public DateTime? ActualEmptyPickupDate;
        public DateTime? EstimatedPOLArrival;
        public DateTime? ActualPOLArrival;
        public string EmptyPickupLocation;
        public string DepartureLocation;
        public string DestinationLocation;
        public string CurrentStatus;
        public string CurrentLocation;
        public DateTime? CurrentStatusDate;
        public bool HasContainerException;
        public DateTime? ActualEmptyPickup;
        public DateTime? EstimatedEmptyPickup;
        public string OriginLocation;
        public DateTime? EstimatedOriginPickup;
        public DateTime? ActualOriginPickup;
        public string POLLocation;
        public DateTime? EstimatedPOLLoaded;
        public DateTime? ActualPOLLoaded;
        public DateTime? EstimatedPOLVesselDeparture;
        public DateTime? ActualPOLVesselDeparture;
        public int? TransshipmentCount;
        public string Transshipment1Location;
        public DateTime? EstimatedTrans1VesselArrival;
        public DateTime? ActualTransshipment1VesselArrival;
        public DateTime? EstimatedTransshipment1Discharge;
        public DateTime? ActualTransshipment1Discharge;
        public DateTime? EstimatedTransshipment1Loaded;
        public DateTime? ActualTransshipment1Loaded;
        public DateTime? EstimatedTrans1VesselDeparture;
        public DateTime? ActualTrans1VesselDeparture;
        public string Transshipment2Location;
        public DateTime? EstimatedTrans2VesselArrival;
        public DateTime? ActualTransshipment2VesselArrival;
        public DateTime? EstimatedTransshipment2Discharge;
        public DateTime? ActualTransshipment2Discharge;
        public DateTime? EstimatedTransshipment2Loaded;
        public DateTime? ActualTransshipment2Loaded;
        public DateTime? EstimatedTrans2VesselDeparture;
        public DateTime? ActualTrans2VesselDeparture;
        public string Transshipment3Location;
        public DateTime? EstimatedTrans3VesselArrival;
        public DateTime? ActualTransshipment3VesselArrival;
        public DateTime? EstimatedTransshipment3Discharge;
        public DateTime? ActualTransshipment3Discharge;
        public DateTime? EstimatedTransshipment3Loaded;
        public DateTime? ActualTransshipment3Loaded;
        public DateTime? EstimatedTrans3VesselDeparture;
        public DateTime? ActualTrans3VesselDeparture;
        public string Transshipment4Location;
        public DateTime? EstimatedTrans4VesselArrival;
        public DateTime? ActualTransshipment4VesselArrival;
        public DateTime? EstimatedTransshipment4Discharge;
        public DateTime? ActualTransshipment4Discharge;
        public DateTime? EstimatedTransshipment4Loaded;
        public DateTime? ActualTransshipment4Loaded;
        public DateTime? EstimatedTrans4VesselDeparture;
        public DateTime? ActualTrans4VesselDeparture;
        public string Leg1Vessel;
        public string Leg1VesselId;
        public string Leg1Voyage;
        public string Leg2Vessel;
        public string Leg2VesselId;
        public string Leg2Voyage;
        public string Leg3Vessel;
        public string Leg3VesselId;
        public string Leg3Voyage;
        public string Leg4Vessel;
        public string Leg4VesselId;
        public string Leg4Voyage;
        public string Leg5Vessel;
        public string Leg5VesselId;
        public string Leg5Voyage;
        public string PODLocation;
        public DateTime? EstimatedPODVesselArrival;
        public DateTime? ActualPODVesselArrival;
        public DateTime? EstimatedPODDischarge;
        public DateTime? ActualPODDischarge;
        public DateTime? EstimatedPODDeparture;
        public DateTime? ActualPODDeparture;
        public string DeliveryLocation;
        public DateTime? EstimatedDelivery;
        public DateTime? ActualDelivery;
        public string LIFLocation;
        public DateTime? EstimatedLIFArrival;
        public DateTime? ActualLIFArrival;
        public DateTime? EstimatedOnCarriageDeparture;
        public DateTime? ActualOnCarriageDeparture;
        public DateTime? POLGateIn;
        public DateTime? PODGateOut;
        public string EmptyReturnLocation;
        public DateTime? EstimatedEmptyReturn;
        public DateTime? ActualEmptyReturn;
        public string CustomsReleaseState;
        public DateTime? CustomsReleaseDate;
        public string CarrierReleaseState;
        public DateTime? CarrierReleaseDate;
        public DateTime? AvailablityDate;
        public string AvailabilityLocation;
        public string ContainerStatus;
        public string ShipmentPackageId;
        public DateTime? EventDate;
        public string TrackingSource;
        public DateTime? OnCarriageETA;
        public DateTime? OnCarriageATA;

        public MilestoneData LoadedTransshipment; 
        public MilestoneData VesselArrived;
        public MilestoneData VesselDeparted;
        public MilestoneData DischargedTransshipment;
        public string OnCarriageLocation;

        public string POLLegVessel;
        public string POLLegVoyage;

        public string PODLegVessel;
        public string PODLegVoyage;

        public string VisionPreCarriage { get; set; }
        public string VisionOnCarriage { get; set; }
    }

    public class MilestoneData
    {
        public string Key;
        public List<MilestoneDataUpdatedFields> MilestoneFields;
        public MilestoneData(string key)
        {
            this.Key = key;
            this.MilestoneFields = new List<MilestoneDataUpdatedFields>();
        }
    }
    public class MilestoneDataUpdatedFields
    {
        public string Location;
        public string Vessel;
        public string Voyage;
        public DateTime? EstimatedDate;
        public DateTime? ActualDate;
    }    
}