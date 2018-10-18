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
    public class RatesTableService
    {

        bool isNewEntity;
        private int tenant;
        public RatesTable Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private RatesTablePM entityPM;
        private IWebFreightContext objectContext;
        private RatesTableRepository entityRepository;
        public RatesTableService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new RatesTableRepository(objectContext);
        }

        public void Create(RatesTablePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("RatesTable", tenant).ToString();
            this.Poco = new RatesTable();
            this.Poco.Id = this.entityPM.Id;

            RatesTableValidating.Validate(theEntityPm);
            RatesTableTracing.Trace(theEntityPm, Poco, isNewEntity);
            RatesTableMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(RatesTablePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleRatesTable(theEntityPm.Id , theEntityPm.Tenant);

            RatesTableValidating.Validate(theEntityPm);
            RatesTableTracing.Trace(theEntityPm, Poco, isNewEntity);
            RatesTableMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}