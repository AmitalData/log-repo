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
    public class AdvancedQueryFilterService
    {
        bool isNewEntity;
        private int tenant;
        public AdvancedQueryFilter Poco { get; set; }
        private AdvancedQueryFilterPM entityPM;
        private IWebFreightContext objectContext;
        private AdvancedQueryFilterRepository entityRepository;
        public AdvancedQueryFilterService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new AdvancedQueryFilterRepository(objectContext);
        }

        public void Create(AdvancedQueryFilterPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("AdvancedQueryFilter", tenant).ToString();
            this.Poco = new AdvancedQueryFilter();
            this.Poco.Id = this.entityPM.Id;

            AdvancedQueryFilterValidating.Validate(theEntityPm);
            AdvancedQueryFilterTracing.Trace(theEntityPm, Poco, isNewEntity);
            AdvancedQueryFilterMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(AdvancedQueryFilterPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleAdvancedQueryfilter(theEntityPm.Id);

            AdvancedQueryFilterValidating.Validate(theEntityPm);
            AdvancedQueryFilterTracing.Trace(theEntityPm, Poco, isNewEntity);
            AdvancedQueryFilterMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}