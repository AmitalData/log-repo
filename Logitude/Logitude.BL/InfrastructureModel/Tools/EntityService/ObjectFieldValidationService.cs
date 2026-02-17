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
    public class ObjectFieldValidationService
    {
        bool isNewEntity;
        private int tenant;
        public ObjectFieldValidation Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ObjectFieldValidationPM entityPM;
        private IWebFreightContext objectContext;
        private ObjectFieldValidationRepository entityRepository;
        public ObjectFieldValidationService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ObjectFieldValidationRepository(objectContext);
        }

        public void Create(ObjectFieldValidationPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ObjectFieldValidation", tenant).ToString();
            this.Poco = new ObjectFieldValidation();
            this.Poco.Id = this.entityPM.Id;

            ObjectFieldValidationValidating.Validate(theEntityPm);
            ObjectFieldValidationTracing.Trace(theEntityPm, Poco, isNewEntity);
            ObjectFieldValidationMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(ObjectFieldValidationPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleObjectFieldValidation(theEntityPm.Id , theEntityPm.Tenant);

            ObjectFieldValidationValidating.Validate(theEntityPm);
            ObjectFieldValidationTracing.Trace(theEntityPm, Poco, isNewEntity);
            ObjectFieldValidationMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}