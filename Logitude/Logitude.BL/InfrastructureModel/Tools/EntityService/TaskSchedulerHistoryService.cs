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
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class TaskSchedulerHistoryService
    {

        bool isNewEntity;
        private int tenant;
        public TaskSchedulerHistory Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TaskSchedulerHistoryPM entityPM;
        private IWebFreightContext objectContext;
        private TaskSchedulerHistoryRepository entityRepository;
        public TaskSchedulerHistoryService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TaskSchedulerHistoryRepository(objectContext);
        }

        public void Create(TaskSchedulerHistoryPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("TaskSchedulerHistory", tenant).ToString();
            this.Poco = new TaskSchedulerHistory();
            this.Poco.Id = this.entityPM.Id;
             
            TaskSchedulerHistoryMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
            //SchedulerLogsService SchedulerLogsService = new SchedulerLogsService(objectContext, theEntityPm.Tenant);
            //SchedulerLogsPM SchedulerLog = new SchedulerLogsPM() { Tenant = theEntityPm.Tenant, HistoryId = theEntityPm.Id };
            //SchedulerLog.CreateDate = TenantServerConfigration.GetCurrentDateTime(SchedulerLog.Tenant);
            //SchedulerLog.Log = "Start Runing the Scheduler";
            //SchedulerLogsService.Create(SchedulerLog);
        }

        public void Update(TaskSchedulerHistoryPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleTaskSchedulerHistory(theEntityPm.Id, theEntityPm.Tenant);

           
            TaskSchedulerHistoryMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            //SchedulerLogsService SchedulerLogsService = new SchedulerLogsService(objectContext, theEntityPm.Tenant);
            //SchedulerLogsQuery SchedulerLogsQuery = new SchedulerLogsQuery(theEntityPm.Tenant);

            //SchedulerLogsPM SchedulerLog = SchedulerLogsQuery.GetSchedulerLogsByHistory(theEntityPm.Id);
            //SchedulerLog.CreateDate = TenantServerConfigration.GetCurrentDateTime(SchedulerLog.Tenant);
            //SchedulerLog.Log = "Start Runing the Scheduler";
            //SchedulerLogsService.Create(SchedulerLog);
        }

    }
}