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
    public class ObjectTableHelperControlService
    {
        bool isNewEntity;
        private int tenant;
        public ObjectTableHelperControl Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ObjectTableHelperControlPM entityPM;
        private IWebFreightContext objectContext;
        private ObjectTableHelperControlRepository entityRepository;
        public ObjectTableHelperControlService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ObjectTableHelperControlRepository(objectContext);
        }

        public void Create(ObjectTableHelperControlPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ObjectTableHelperControl", tenant).ToString();
            this.Poco = new ObjectTableHelperControl();
            this.Poco.Id = this.entityPM.Id;

            ObjectTableHelperControlValidating.Validate(theEntityPm);
            ObjectTableHelperControlTracing.Trace(theEntityPm, Poco, isNewEntity);
            ObjectTableHelperControlMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(ObjectTableHelperControlPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleObjectTableHelperControl(theEntityPm.Id);

            ObjectTableHelperControlValidating.Validate(theEntityPm);
            ObjectTableHelperControlTracing.Trace(theEntityPm, Poco, isNewEntity);
            ObjectTableHelperControlMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}