using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.Server.Tools.QueueService;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class EntityStatusService
    {
        bool isNewEntity;
        private int tenant;
        public EntityStatus Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private EntityStatusPM entityPM;
        private IWebFreightContext objectContext;
        private EntityStatusRepository entityRepository;
        public EntityStatusService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new EntityStatusRepository(objectContext);
        }

        public void Create(EntityStatusPM theEntityPm)
        {
            EntityStatus entity = entityRepository.GetSingleEntityStatusByCodeTableId(entityPM.Code, entityPM.ObjectTableId, entityPM.Tenant);
            if (entity != null)
            {
                theEntityPm.Id = entity.Id;
                Update(theEntityPm);
            }
                
            else
            {
                this.isNewEntity = true;
                this.entityPM = theEntityPm;
                this.entityPM.Id = IdCounter.GetNumber("EntityStatus", tenant).ToString();
                this.Poco = new EntityStatus();
                this.Poco.Id = this.entityPM.Id;

                EntityStatusValidating.Validate(theEntityPm);
                if (!entityPM.IsHybrid)
                {
                    EntityStatusTracing.Trace(theEntityPm, Poco, isNewEntity);
                }
                EntityStatusMapping.MapEntity(theEntityPm, Poco, isNewEntity);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
                AddQueueMessages();
            }
        }

        public void Update(EntityStatusPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleEntityStatus(theEntityPm.Id , theEntityPm.Tenant);


            string entityName = "EntityStatus" + theEntityPm.Id + theEntityPm.Tenant;
            string entityPMName = "EntityStatusPM" + theEntityPm.Id + theEntityPm.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPMName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPMName);
            }

            EntityStatusValidating.Validate(theEntityPm);
            if (!entityPM.IsHybrid)
            {
                EntityStatusTracing.Trace(theEntityPm, Poco, isNewEntity);
            }
            EntityStatusMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            AddQueueMessages();
        }
        private void AddPortKafkaQueueMessage()
        {
            if (!FeatureToggleHelper.HasFeatureToggle("CTL", entityPM.Tenant))
            {
                return;
            }
            AddKafkaQueueMessage();
        }

        private void AddKafkaQueueMessage()
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("CToolLookups", 0);
            var queueMessage = new Dictionary<string, string>() {
                { "Entity", "EntityStatus" },
                { "EntityId", entityPM.Id },
                { "Tenant", tenant.ToString()}};
            queueservice.Send(queueMessage, tenant);
        }

        private void AddQueueMessages()
        {
            AddPortKafkaQueueMessage();
            AddImporterQueueMessage();
        }
        private void AddImporterQueueMessage()
        {
            if (entityPM.IsFromWorkerRole) return;
            if (tenant != 0) return;
            if (!IsCloudEnvironment() && !SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Development)) return;
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("ImporterEntityStatusesQueue", 0);
            Dictionary<string, string> importerQueueMessage = new Dictionary<string, string>() {
                { "EntityStatusId", entityPM.Id },
                { "Tenant", tenant.ToString()}
            };
            queueservice.Send(importerQueueMessage, tenant);
        }
        private bool IsCloudEnvironment()
        {
            return LogitudeSettings.WorkEnvironment?.ToLower() == "cloud";
        }

       
    }
}