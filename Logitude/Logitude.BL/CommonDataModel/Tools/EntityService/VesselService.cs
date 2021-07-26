using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class VesselService
    {
        bool isNewEntity;
        private int tenant;
        public Vessel Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private VesselPM entityPM;
        private ICommonDataContext objectContext;
        private VesselRepository entityRepository;
        public VesselService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new VesselRepository(objectContext);
        }

        public void Create(VesselPM entityPM)
        {
            bool exist = false;
            if (!string.IsNullOrEmpty(entityPM.Code))
            {
                exist = (from a in entityRepository.GetVesselsByTenant(tenant)
                         where a.Code == entityPM.Code && a.Tenant == tenant
                         select a).Any();
            }

            if (!exist)
            {
                this.isNewEntity = true;
                this.entityPM = entityPM;
                this.entityPM.Id = IdCounter.GetNumber("Vessel", tenant).ToString();
                this.Poco = new Vessel();
                this.Poco.Id = this.entityPM.Id;

                VesselValidating.Validate(entityPM);
                if (!entityPM.IsHybrid)
                {
                    VesselTracing.Trace(entityPM, Poco, isNewEntity);
                }

                VesselMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", tenant);
                msg = msg.Replace("%Entity", "Vessel");
                throw new Exception(msg);
            }

            AddVesselKafkaQueueMessage();
        }

        public void Update(VesselPM entityPM)
        {
            bool exist = false;
            if (!string.IsNullOrEmpty(entityPM.Code))
            {
                exist = (from a in entityRepository.GetVesselsByTenant(entityPM.Tenant)
                         where a.Code == entityPM.Code
                         && a.Id != entityPM.Id
                         && a.Tenant == entityPM.Tenant
                         select a).Any();
            }

            if (!exist)
            {
                this.isNewEntity = false;
                this.entityPM = entityPM;
                this.Poco = entityRepository.GetSingleVessel(entityPM.Id, entityPM.Tenant);

                string entityName = "Vessel" + entityPM.Id + entityPM.Tenant;
                string entityPmName = "VesselPM" + entityPM.Id + entityPM.Tenant;
                if (CacheManager.CacheWrapper.Get(entityName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityName);
                }
                if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityPmName);
                }

                VesselValidating.Validate(entityPM);
                if (!entityPM.IsHybrid)
                {
                    VesselTracing.Trace(entityPM, Poco, isNewEntity);
                }

                VesselMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", entityPM.Tenant);
                msg = msg.Replace("%Entity", "Vessel");
                throw new Exception(msg);
            }

            AddVesselKafkaQueueMessage();
        }

        private void AddVesselKafkaQueueMessage()
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
                { "Entity", "Vessel" },
                { "EntityId", entityPM.Id },
                { "Tenant", tenant.ToString()}};
            queueservice.Send(queueMessage, tenant);
        }
    }
}
