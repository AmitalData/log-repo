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
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.QueueService;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class CustomPickListService
    {

        bool isNewEntity;
        private int tenant;
        public CustomPickList Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CustomPickListPM entityPM;
        private IWebFreightContext objectContext;
        private CustomPickListRepository entityRepository;
        public CustomPickListService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomPickListRepository(objectContext);
        }

        public void Create(CustomPickListPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("CustomPickList", tenant).ToString();
            this.Poco = new CustomPickList();
            this.Poco.Id = this.entityPM.Id;

            CustomPickListValidating.Validate(theEntityPm);
            CustomPickListTracing.Trace(theEntityPm, Poco, isNewEntity);
            CustomPickListMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            ProcessPickListCToolMessage(theEntityPm, "UpsertCustomPickListValue");
        }

        public void Update(CustomPickListPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleCustomPickList(theEntityPm.Id, theEntityPm.Tenant);

            string entityName = "CustomPickList" + theEntityPm.Id + theEntityPm.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }

            CustomPickListValidating.Validate(theEntityPm);
            CustomPickListTracing.Trace(theEntityPm, Poco, isNewEntity);
            CustomPickListMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            ProcessPickListCToolMessage(theEntityPm, "UpsertCustomPickListValue");
        }

        public void ProcessPickListCToolMessage(CustomPickListPM theEntityPm, string messageType)
        {
            if (IsMetConditionsToSendCToolMessage(theEntityPm))
            {
                AddKafkaQueueMessage(theEntityPm, "CToolShipmentsUpdate");
            }
        }

        private bool IsMetConditionsToSendCToolMessage(CustomPickListPM theEntityPm)
        {
            return FeatureToggleHelper.HasFeatureToggle("CTL", theEntityPm.Tenant);
        }

        private void AddKafkaQueueMessage(CustomPickListPM theEntityPm, string messageType)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("CToolLookups", 0);
            var queueMessage = new Dictionary<string, string>() {
                { "Entity", messageType },
                { "EntityId", entityPM.Id },
                { "Tenant", tenant.ToString()}};
            queueservice.Send(queueMessage, tenant);
        }
    }
}