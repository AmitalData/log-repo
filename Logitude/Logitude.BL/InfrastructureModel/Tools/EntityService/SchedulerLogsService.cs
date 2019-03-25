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
    public class SchedulerLogsService
    {

        bool isNewEntity;
        private int tenant;
        public SchedulerLogs Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private SchedulerLogsPM entityPM;
        private IWebFreightContext objectContext;
        private SchedulerLogsRepository entityRepository;
        public SchedulerLogsService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new SchedulerLogsRepository(objectContext);
        }

        public void Create(SchedulerLogsPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("SchedulerLogs", tenant).ToString();
            this.Poco = new SchedulerLogs();
            this.Poco.Id = this.entityPM.Id;
             
            SchedulerLogsMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(SchedulerLogsPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleSchedulerLogs(theEntityPm.Id, theEntityPm.Tenant);

           
            SchedulerLogsMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}