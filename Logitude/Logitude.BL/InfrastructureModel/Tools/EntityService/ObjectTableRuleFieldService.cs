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
    public class ObjectTableRuleFieldService
    {

        bool isNewEntity;
        private int tenant;
        public ObjectTableRuleField Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ObjectTableRuleFieldPM entityPM;
        private IWebFreightContext objectContext;
        private ObjectTableRuleFieldRepository entityRepository;
        public ObjectTableRuleFieldService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ObjectTableRuleFieldRepository(objectContext);
        }

        public void Create(ObjectTableRuleFieldPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ObjectTableRuleField", tenant).ToString();
            this.Poco = new ObjectTableRuleField();
            this.Poco.Id = this.entityPM.Id;

            ObjectTableRuleFieldValidating.Validate(theEntityPm);
            ObjectTableRuleFieldTracing.Trace(theEntityPm, Poco, isNewEntity);
            ObjectTableRuleFieldMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(ObjectTableRuleFieldPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleObjectTableRuleField(theEntityPm.Id, theEntityPm.Tenant);
       
            string rulesFieldsListName = "RuleFields" + this.Poco.Tenant;
            if (CacheManager.CacheWrapper.Get(rulesFieldsListName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(rulesFieldsListName);
            }

           
            ObjectTableRuleFieldValidating.Validate(theEntityPm);
            ObjectTableRuleFieldTracing.Trace(theEntityPm, Poco, isNewEntity);
            ObjectTableRuleFieldMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}