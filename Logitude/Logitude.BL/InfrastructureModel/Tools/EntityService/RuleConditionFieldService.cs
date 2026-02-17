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
    public class RuleConditionFieldService
    {

        bool isNewEntity;
        private int tenant;
        public RuleConditionField Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private RuleConditionFieldPM entityPM;
        private IWebFreightContext objectContext;
        private RuleConditionFieldRepository entityRepository;
        public RuleConditionFieldService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new RuleConditionFieldRepository(objectContext);
        }

        public void Create(RuleConditionFieldPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("RuleConditionField", tenant).ToString();
            this.Poco = new RuleConditionField();
            this.Poco.Id = this.entityPM.Id;

            RuleConditionFieldValidating.Validate(theEntityPm);
            RuleConditionFieldTracing.Trace(theEntityPm, Poco, isNewEntity);
            RuleConditionFieldMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(RuleConditionFieldPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleRuleConditionField(theEntityPm.Id , theEntityPm.Tenant);

            RuleConditionFieldValidating.Validate(theEntityPm);
            RuleConditionFieldTracing.Trace(theEntityPm, Poco, isNewEntity);
            RuleConditionFieldMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}