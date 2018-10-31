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

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class EventTypeService
    {
        bool isNewEntity;
        private int tenant;
        public EventType Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private EventTypePM entityPM;
        private IWebFreightContext objectContext;
        private EventTypeRepository entityRepository;
        public EventTypeService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new EventTypeRepository(objectContext);
        }

        public void Create(EventTypePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("EventType", tenant).ToString();
            this.entityPM.AddedManually = true;

            this.Poco = new EventType();
            this.Poco.Id = this.entityPM.Id;
            
            EventTypeValidating.Validate(theEntityPm);

            if (!entityPM.IsHybrid)
            {
                EventTypeTracing.Trace(theEntityPm, Poco, isNewEntity);
            }

            EventTypeMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(EventTypePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleEventType(theEntityPm.Id , theEntityPm.Tenant);
      
            EventTypeValidating.Validate(theEntityPm);

            if (!entityPM.IsHybrid)
            {
                EventTypeTracing.Trace(theEntityPm, Poco, isNewEntity);
            }

            EventTypeMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}