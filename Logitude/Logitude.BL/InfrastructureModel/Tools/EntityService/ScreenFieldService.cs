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
    public class ScreenFieldService
    {

        bool isNewEntity;
        private int tenant;
        public ScreenField Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ScreenFieldPM entityPM;
        private IWebFreightContext objectContext;
        private ScreenFieldsRepository entityRepository;
        public ScreenFieldService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ScreenFieldsRepository(objectContext);
        }

        public void Create(ScreenFieldPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ScreenField", tenant).ToString();
            this.Poco = new ScreenField();
            this.Poco.Id = this.entityPM.Id;

            ScreenFieldValidating.Validate(theEntityPm);
            ScreenFieldTracing.Trace(theEntityPm, Poco, isNewEntity);
            ScreenFieldMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(ScreenFieldPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleScreenField(theEntityPm.Id);

            ScreenFieldValidating.Validate(theEntityPm);
            ScreenFieldTracing.Trace(theEntityPm, Poco, isNewEntity);
            ScreenFieldMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}