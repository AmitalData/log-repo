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
using Logitude.Server.Tools.QueueService;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class TasksSchedulerService
    {

        bool isNewEntity;
        private int tenant;
        public TasksScheduler Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TasksSchedulerPM entityPM;
        private IWebFreightContext objectContext;
        private TasksSchedulerRepository entityRepository;
        public TasksSchedulerService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TasksSchedulerRepository(objectContext);
        }

        public void Create(TasksSchedulerPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("TasksScheduler", tenant).ToString();
            this.Poco = new TasksScheduler();
            this.Poco.Id = this.entityPM.Id;
             
            TasksSchedulerMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("SchedularQueue", 0);
            queueservice.Send(new Dictionary<string, string>() { { "TaskId", Poco.Id }, { "Tenant", Poco.Tenant.ToString() } }, null, null, null, Poco.NextRunTime);

        }

        public void Update(TasksSchedulerPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleTasksScheduler(theEntityPm.Id, theEntityPm.Tenant);

           
            TasksSchedulerMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}