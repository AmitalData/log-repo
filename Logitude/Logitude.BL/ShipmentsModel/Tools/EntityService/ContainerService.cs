using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviours;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.BL.ShipmentsModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.EntityChanges;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
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

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ContainerService
    {
        bool isNewEntity;
        private int tenant;
        private ContainerPM containerPm;
        private IShipmentsContext shipmentsContext;
        private ContainerRepository entityRepository;
        private Container containerPoco { get; set; }
        public ContainerService(IShipmentsContext shipmentsContext, int tenant)
        {
            this.tenant = tenant;
            this.shipmentsContext = shipmentsContext;
            this.entityRepository = new ContainerRepository(shipmentsContext);
        }
        public void Create(ContainerPM entityPM)
        {
            this.isNewEntity = true;
            this.containerPm = entityPM;
            this.containerPm.Id = IdCounter.GetNumber("Container", tenant).ToString();
            this.containerPoco = new Container { Id = this.containerPm.Id, Tenant = this.containerPm.Tenant };
            RunAutomation("OnCreate", entityPM);
            ContainerValidating.Validate(this.containerPm, this.containerPoco, isNewEntity);
            ContainerTracing containerTracing = new ContainerTracing(entityPM, containerPoco, isNewEntity);
            containerTracing.Trace();
            ShipmentMapping.MapContainer(entityPM, containerPoco, isNewEntity);
            this.GetForeignFields_Status(entityPM, containerPoco);
            entityRepository.Add(containerPoco);
            entityRepository.SubmitChanges();
            AddShipmentUpdateKafkaQueueMessage("CToolContainerCreate");
            MapShipmentConcurrencyFields();
        }
        public void Update(ContainerPM entityPM, ContainersExternal containersExternal = null)
        {
            this.isNewEntity = false;
            this.containerPm = entityPM;
            containerPm.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.SetUpdatedByUser();
            this.containerPoco = entityRepository.GetSingleContainer(entityPM.Id, tenant);
            this.MapContainerClosedDate(entityPM, containerPoco);
            ContainerValidating.Validate(this.containerPm, this.containerPoco, isNewEntity);
            ContainerTracing containerTracing = new ContainerTracing(entityPM, containerPoco, isNewEntity);
            containerTracing.Trace();
            if (!entityPM.IsUpdateByAutomation)
            {
                RunAutomation("OnUpdate", entityPM);
            }
            this.HandleContainersExternalData(entityPM, containersExternal);
            ShipmentMapping.MapContainer(entityPM, containerPoco, isNewEntity);
            this.GetForeignFields_Status(entityPM, containerPoco);
            entityRepository.Update(containerPoco);
            entityRepository.SubmitChanges();
            AddShipmentUpdateKafkaQueueMessage("CToolContainerUpdate");
            MapShipmentConcurrencyFields();
        }
        private void SetUpdatedByUser()
        {
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
        private void AddShipmentUpdateKafkaQueueMessage(string queueName)
        {
            if (!FeatureToggleHelper.HasFeatureToggle("CTL", containerPm.Tenant))
            {
                return;
            }
            AddKafkaQueueMessage(queueName);
        }
        private void AddKafkaQueueMessage(string queueName)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue(queueName, 0);
            var queueMessage = new Dictionary<string, string>() {
                { "ContainerId", containerPm.Id },
                { "Tenant", tenant.ToString()}};

            queueservice.Send(queueMessage, tenant);
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
        private void RunAutomation(string processType, ContainerPM entityPM)
        {
            var mainEntityChangeService = new MainEntityChangeService(new EntityChangeArgs() { EntityPM = entityPM, ProcessType = processType, ObjectTableName = "Container", EntityId = entityPM.Id, Tenant = entityPM.Tenant, StartDate = DateTime.Now });
            mainEntityChangeService.AddEntityChange();
        }
        private void MapShipmentConcurrencyFields()
        {
            if (string.IsNullOrEmpty(this.containerPm.ShipmentId))
            {
                return;
            }
            this.containerPm.ShipmentConcurrencyGUID = entityRepository.GetConcurrencyGUIDByShipmentId(this.containerPm.ShipmentId, this.containerPm.Tenant);
            this.containerPm.ShipmentNewConcurrencyGUID = Guid.NewGuid().ToString();
        }
        //private void MapUpdatedByPartnerField(ContainerPM entityPM)
        //{
        //    entityPM.UpdatedByPartner = entityPM.UpdatedByUserName;
        //    if (entityPM.IsUpdatedOceanInsightsAnalyzer)
        //    {
        //        entityPM.UpdatedByPartner = "Ocean Insights Transmission";
        //    }
        //}
    }
}
