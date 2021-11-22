using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.EntityChanges;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.Helpers;
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
            ContainerTracing.Trace(entityPM, containerPoco, isNewEntity);
            ShipmentMapping.MapContainer(entityPM, containerPoco, isNewEntity);
            entityRepository.Add(containerPoco);
            entityRepository.SubmitChanges();
            AddShipmentUpdateKafkaQueueMessage("CToolContainerCreate");
        }

        public void Update(ContainerPM entityPM)
        {
            this.isNewEntity = false;
            this.containerPm = entityPM;
            this.containerPoco = entityRepository.GetSingleContainer(entityPM.Id, tenant);
            this.MapContainerClosedDate(entityPM, containerPoco);
            ContainerTracing.Trace(entityPM, containerPoco, isNewEntity);
            if (!entityPM.IsUpdateByAutomation)
            {
                RunAutomation("OnUpdate", entityPM);
            }
            ShipmentMapping.MapContainer(entityPM, containerPoco, isNewEntity);
            entityRepository.Update(containerPoco);
            entityRepository.SubmitChanges();
            AddShipmentUpdateKafkaQueueMessage("CToolContainerUpdate");
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
            ContainerTracing.Trace(entityPM, containerPoco, isNewEntity);
            entityRepository.Remove(containerPoco);
            entityRepository.SubmitChanges();
        }

        private void RunAutomation(string processType, ContainerPM entityPM)
        {
            var mainEntityChangeService = new MainEntityChangeService(new EntityChangeArgs() { EntityPM = entityPM, ProcessType = processType, ObjectTableName = "Container", EntityId = entityPM.Id, Tenant = entityPM.Tenant, StartDate = DateTime.Now });
            mainEntityChangeService.AddEntityChange();
        }
    }
}
