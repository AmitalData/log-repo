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
    public class TipsVisibilityService
    {

        bool isNewEntity;
        private int tenant;
        public TipsVisibility Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TipsVisibilityPM entityPM;
        private IWebFreightContext objectContext;
        private TipsVisibilityRepository entityRepository;
        public TipsVisibilityService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TipsVisibilityRepository(objectContext);
        }

        public void Create(TipsVisibilityPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("TipsVisibility", tenant).ToString();
            this.Poco = new TipsVisibility();
            this.Poco.Id = this.entityPM.Id;

            TipsVisibilityValidating.Validate(theEntityPm);
            TipsVisibilityTracing.Trace(theEntityPm, Poco, isNewEntity);
            TipsVisibilityMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(TipsVisibilityPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleTipsVisibility(theEntityPm.Id , theEntityPm.Tenant);

            TipsVisibilityValidating.Validate(theEntityPm);
            TipsVisibilityTracing.Trace(theEntityPm, Poco, isNewEntity);
            TipsVisibilityMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}