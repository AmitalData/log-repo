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
    public class SharedLogisticsUpdateService
    {

        bool isNewEntity;
        private int tenant;
        public SharedLogisticsUpdate Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private SharedLogisticsUpdatePM entityPM;
        private IWebFreightContext objectContext;
        private SharedLogisticsUpdateRepository entityRepository;
        public SharedLogisticsUpdateService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new SharedLogisticsUpdateRepository(objectContext);
        }

        public void Create(SharedLogisticsUpdatePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("SharedLogisticsUpdate", tenant).ToString();
            this.Poco = new SharedLogisticsUpdate();
            this.Poco.Id = this.entityPM.Id;

            SharedLogisticsUpdateValidating.Validate(theEntityPm);
            SharedLogisticsUpdateTracing.Trace(theEntityPm, Poco, isNewEntity);
            SharedLogisticsUpdateMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(SharedLogisticsUpdatePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleSharedLogisticUpdate(theEntityPm.Id);

            SharedLogisticsUpdateValidating.Validate(theEntityPm);
            SharedLogisticsUpdateTracing.Trace(theEntityPm, Poco, isNewEntity);
            SharedLogisticsUpdateMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}