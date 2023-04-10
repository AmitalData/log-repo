using Logitude.BL.AnalyticTableServices;
using Logitude.BL.ExternalService;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.APIDataContract;
using Logitude.BL.ShipmentsModel.EntityOtherServices;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.Behaviours;
using Logitude.BL.ShipmentsModel.Tools.ContainerTracking;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.BL.ShipmentsModel.Tools.Validating;
using Logitude.BL.Workfkow;
using Logitude.BL.Workfkow.Constants;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Models.AuditLog;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.CToolWorkflows;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
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
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ContainerService
    {
        bool isNewEntity;
        private int tenant;
        private ContainerPM containerPm;
        private IShipmentsContext shipmentsContext;
        private ContainerRepository entityRepository;
        private Container containerPoco;
        private AuditLogRepository AuditLogRepository;        

        public ContainerService(IShipmentsContext shipmentsContext, int tenant)
        {
            AuditLogRepository = new AuditLogRepository(tenant);

            this.tenant = tenant;
            this.shipmentsContext = shipmentsContext;
            this.entityRepository = new ContainerRepository(shipmentsContext);
        }

        public void Create(ContainerPM entityPM)
        {
            List<FieldChange> FieldChanges = new List<FieldChange>();

            this.isNewEntity = true;
            this.containerPm = entityPM;
            this.containerPm.Id = IdCounter.GetNumber("Container", tenant).ToString();
            this.containerPoco = new Container { Id = this.containerPm.Id, Tenant = this.containerPm.Tenant };

            EntityAutomationService entityAutomationService = new EntityAutomationService(new EntityAutomationArgs() { Poco = containerPoco, EntityPM = entityPM, OldEntityPM = new ContainerPM(), AutomationType = "OnCreate", ObjectTableName = "Container", Tenant = entityPM.Tenant, EntityId = entityPM.Id, EntityReference = entityPM.ContainerNumber });
            entityAutomationService.RunAutomation();

            this.ComputeTransshipmentCount();
            this.ComputeHasTransShipments();
            ContainerValidating.Validate(this.containerPm, this.containerPoco, isNewEntity);
            
            ContainerTracing containerTracing = new ContainerTracing(entityPM, containerPoco, isNewEntity);
            containerTracing.Trace();
            
            SaveChildEntities();

            ShipmentMapping.MapContainer(entityPM, containerPoco, isNewEntity, FieldChanges);
            this.GetForeignFields_Status(entityPM, containerPoco);
            entityRepository.Add(containerPoco);
            entityRepository.SubmitChanges();

            if (this.containerPm != null && !this.containerPm.FromCTool)
            {
                EntityChangesMessageProducer.ProduceContainerCreateMessage(containerPoco, containerPm);
            }

            AuditLog auditLog = null;
            if (entityPM != null && FeatureToggleHelper.HasFeatureToggle("ADL", entityPM.Tenant))
            {
                auditLog = AddContainerAuditLogChanges(containerPoco, FieldChanges);
                AuditLogRepository.Add(auditLog);
                AuditLogRepository.SubmitChanges();
            }
          
            new WorkflowEntityQueueMessage()
            {
                Entity = WorkflowEntities.Container,
                EntityId = entityPM.Id,
                AuditLogId = auditLog?.Id,
                Tenant = entityPM.Tenant,
                Type = QueueMessagesTypes.Create
            }.Produce();

            entityAutomationService.RunAutomationThatDependencyOnLastEntityUpdate();
            string activity = "(A) Container Create";
            AddTotangoActivity(entityPM, activity);
            new GeneralContainerTrackingService(GetGeneralContainerTrackingArgs(entityPM)).AutomaticTrackContainer();         
            new ContainerAnalyticTableService(shipmentsContext.GetActiveDbContext()).AddUpdate(containerPoco, tenant);
        }
        public void AddTotangoActivity(ContainerPM containerPM, string activityDescription)
        {
            string email = AuthenticationUtil.IsAuthenticatedUserExists() ? AuthenticationUtil.GetAuthenticatedUser() : "system@tenant" + tenant + ".com";
            string moduleName = "(A) Container";
            ActivityLogger.SendTotangoContactActivity(email, moduleName, activityDescription, containerPM.Tenant,false,null);
        }

        private GeneralContainerTrackingArgs GetGeneralContainerTrackingArgs(ContainerPM entityPM)
        {
            return new GeneralContainerTrackingArgs
            {
                ContainerId = entityPM.Id,
                ContainerNumber = entityPM.ContainerNumber,
                IsFromContainer = true,
                ShipmentId = entityPM.ShipmentId,
                Tenant = entityPM.Tenant,
                IsSimulator = false,
                Data = null,
                ContainerStatusSourceCode = "2",
                DirectionId = this.GetShipment()?.DirectionId,
                IsUpdatedFromRequest = entityPM.IsUpdatedFromRequest,
            };
        }

        public void Update(ContainerPM entityPM, ContainersExternal containersExternal = null)
        {
            List<FieldChange> FieldChanges = new List<FieldChange>();

            this.isNewEntity = false;
            this.containerPm = entityPM;

            containerPm.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.SetUpdatedByUser();
            this.containerPoco = entityRepository.GetSingleContainer(entityPM.Id, tenant);
            this.MapContainerClosedDate(entityPM, containerPoco);
            this.ComputeTransshipmentCount();
            this.ComputeHasTransShipments();
            ContainerValidating.Validate(this.containerPm, this.containerPoco, isNewEntity);
            ContainerTracing containerTracing = new ContainerTracing(entityPM, containerPoco, isNewEntity);
            containerTracing.Trace();
            SaveChildEntities();
            if (!entityPM.IsUpdateByAutomation)
            {
                EntityAutomationService entityAutomationService = new EntityAutomationService(new EntityAutomationArgs() { Poco = containerPoco, EntityPM = entityPM, OldEntityPM = new ContainerPM(), AutomationType = "OnUpdate", ObjectTableName = "Container", Tenant = entityPM.Tenant, EntityId = entityPM.Id, EntityReference = entityPM.ContainerNumber });
                entityAutomationService.RunAutomation();
            }
     
            this.HandleContainersExternalData(entityPM, containersExternal);

            if (containerPm.EstimatedEmptyReturn != containerPoco.EstimatedEmptyReturn || containerPm.ActualEmptyReturn != containerPoco.ActualEmptyReturn)
                entityPM.IsEmptyReturnDatesChanged = true;

            Container containerPocoCopy = CloneObjectService.Clone(containerPoco);
            ContainerPM containerPMCopy = CloneObjectService.Clone(containerPm);            
            ShipmentMapping.MapContainer(entityPM, containerPoco, isNewEntity, FieldChanges);
            this.GetForeignFields_Status(entityPM, containerPoco);
            entityRepository.Update(containerPoco);
            entityRepository.SubmitChanges();

            this.UpdateShipment();

            if (this.containerPm != null && !this.containerPm.FromCTool)
            {
                EntityChangesMessageProducer.ProduceContainerUpdateMessage(containerPocoCopy, containerPMCopy);
            }

            AuditLog auditLog = null;
            if (entityPM != null && FeatureToggleHelper.HasFeatureToggle("ADL", entityPM.Tenant))
            {
                auditLog = AddContainerAuditLogChanges(containerPoco, FieldChanges);
                AuditLogRepository.Add(auditLog);
                AuditLogRepository.SubmitChanges();
            }

            new WorkflowEntityQueueMessage()
            {
                Entity = WorkflowEntities.Container,
                EntityId = entityPM.Id,
                AuditLogId = auditLog?.Id,
                Tenant = entityPM.Tenant,
                Type = QueueMessagesTypes.Update
            }.Produce();

            string activity = "(A) Container Update";
            AddTotangoActivity(entityPM, activity);
            new GeneralContainerTrackingService(GetGeneralContainerTrackingArgs(entityPM)).AutomaticTrackContainer();          
            new ContainerAnalyticTableService(shipmentsContext.GetActiveDbContext()).AddUpdate(containerPoco, tenant);
        }
        private AuditLog AddContainerAuditLogChanges(Container entityPoco, List<FieldChange> FieldChanges)
        {
            ObjectTableRepository objecttableRepository = new ObjectTableRepository(entityPoco.Tenant);
            ObjectTable objecttable = objecttableRepository.GetObjectTableByName("Container", 0, true);
            AuditLog auditLog = new AuditLog()
            {
                Id = IdCounter.GetNumber("AuditLog", entityPoco.Tenant).ToString(),
                Tenant = entityPoco.Tenant,
                UpdateDate = entityPoco.UpdateDate,
                UpdatedByUserId = entityPoco.UpdatedByUserId,
                EntityId = entityPoco.Id,
                ObjectTableId = objecttable.Id,
                ChangesJson = JsonConvert.SerializeObject(FieldChanges)
            };

            return auditLog;
        }
        private void SaveChildEntities()
        {
            new CustomChildEntityService(new CustomChildEntityArgs() { ParentEntity = containerPm, ParentEntityId = containerPm.Id, ParentObjectTableName = "Container", Tenant = tenant }).Update();
        }

        private void SetUpdatedByUser()
        {
            bool setUser = true;
            if(containerPm.IsUpdatedFromAPI && !string.IsNullOrEmpty(containerPm.UpdatedByUserId))
            {
                setUser = false;
            }

            if (!setUser)
            {
                return;
            }

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "system@tenant" + tenant + ".com";

            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }

            Contact contact = contactRep.GetSingleContactByEmail(email, tenant);
            if (contact != null)
            {
                containerPm.UpdatedByUserId = contact.Id;
            }
        }
        private void HandleContainersExternalData(ContainerPM entityPM, ContainersExternal containersExternal)
        {
            if(containersExternal == null)
            {
                return;
            }
            if (!containersExternal.IsFromOceanInsights)
                return;
            ContainersExternalDataBehaviour containersExternalDataBehaviour = new ContainersExternalDataBehaviour(entityPM, shipmentsContext, containersExternal);
            containersExternalDataBehaviour.Handle();
        }
        private void GetForeignFields_Status(ContainerPM entityPM, Container entityPoco)
        {
            entityPM.StatusName = null;
            if (entityPoco.StatusId == null)
            {
                return;
            }
            EntityStatus iEntityStatus = EntityStatusRepository.GetSingleEntityStatus(entityPoco.StatusId, entityPoco.Tenant, true);
            if (iEntityStatus != null)
            {
                entityPM.StatusName = iEntityStatus.Name;
            }
        }
        private void MapContainerClosedDate(ContainerPM containerPM, Container container)
        {
            if (containerPM.IsClosed != container.IsClosed)
            {
                if (containerPM.IsClosed)
                {
                    containerPM.ClosedDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                }
                else
                {
                    containerPM.ClosedDate = null;
                }
            }
        }
        public void Delete(ContainerPM entityPM)
        {
            this.containerPm = entityPM;
            this.containerPoco = entityRepository.GetSingleContainer(entityPM.Id, tenant);
            ContainerTracing containerTracing = new ContainerTracing(entityPM, containerPoco, isNewEntity);
            containerTracing.Trace();
            entityRepository.Remove(containerPoco);
            entityRepository.SubmitChanges();
        }
        
        private void ComputeTransshipmentCount()
        {
            if (!string.IsNullOrEmpty(containerPm.Transshipment3LocationPortId))
            {
                containerPm.TransshipmentCount = 3;
            }

            else if (!string.IsNullOrEmpty(containerPm.Transshipment2LocationPortId))
            {
                containerPm.TransshipmentCount = 2;
            }

            else if (!string.IsNullOrEmpty(containerPm.Transshipment1LocationPortId))
            {
                containerPm.TransshipmentCount = 1;
            }

            else
            {
                containerPm.TransshipmentCount = null;
            }
        }
        private void ComputeHasTransShipments()
        {
            if (containerPm.TransshipmentCount > 0)
            {
                containerPm.HasTransshipments = true;
            }

            else if (!string.IsNullOrEmpty(containerPm.ShipmentTransshipment1FromId))
            {
                containerPm.HasTransshipments = true;
            }

            else
            {
                containerPm.HasTransshipments = false;
            }
        }

        private ShipmentPM GetShipment()
        {
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
            return shipmentQuery.GetSinglePM(containerPm.ShipmentId, tenant);
        }
        private void UpdateShipment()
        {
            if (!IsUpdatingShipment()) return;             

            ContainerShipmentUpdateService containerShipmentUpdateService = new ContainerShipmentUpdateService(containerPm, shipmentsContext);
            containerShipmentUpdateService.HandleUpdate();       
        } 
        private bool IsUpdatingShipment()
        {
            if (!FeatureToggleHelper.HasFeatureToggle("OIU", tenant)) return false;
            if (containerPm.IsUpdatedFromRequest) return false;
            if (containerPm.IsCancelled) return false;
            if (containerPm.IsClosed) return false;
            if (containerPm.IsShipmentBatchUpdate) return false;
            return true;
        }
    }
}
