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
    public class CustomTableService
    {

        bool isNewEntity;
        private int tenant;
        public CustomTable Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CustomTablePM entityPM;
        private IWebFreightContext objectContext;
        private CustomTableRepository entityRepository;
        public CustomTableService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomTableRepository(objectContext);
        }

        public void Create(CustomTablePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("CustomTable", tenant).ToString();
            this.Poco = new CustomTable();
            this.Poco.Id = this.entityPM.Id;

            CustomTableValidating.Validate(theEntityPm);
            CustomTableTracing.Trace(theEntityPm, Poco, isNewEntity);
            CustomTableMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(CustomTablePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
        //    this.Poco = entityRepository.get(theEntityPm.Id);

            CustomTableValidating.Validate(theEntityPm);
            CustomTableTracing.Trace(theEntityPm, Poco, isNewEntity);
            CustomTableMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}