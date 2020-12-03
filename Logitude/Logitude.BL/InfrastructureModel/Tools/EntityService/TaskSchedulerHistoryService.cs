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
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Data.Entity;

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

            DocumentRepository documentrepository = new DocumentRepository(entityPM.Tenant);
            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "txt",
                Tenant = Convert.ToInt32(entityPM.Tenant),
                Id = IdCounter.GetNumber("Document", entityPM.Tenant),
                Folder = "SchedularLogs",
            };
            documentrepository.Add(document);
            documentrepository.SubmitChanges();

            this.Poco.LogDocumentId= this.entityPM.LogDocumentId = document.Id;

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
            ComputeTaskAverageRunTime(Poco.TaskId);
            //SchedulerLogsService SchedulerLogsService = new SchedulerLogsService(objectContext, theEntityPm.Tenant);
            //SchedulerLogsQuery SchedulerLogsQuery = new SchedulerLogsQuery(theEntityPm.Tenant);

            //SchedulerLogsPM SchedulerLog = SchedulerLogsQuery.GetSchedulerLogsByHistory(theEntityPm.Id);
            //SchedulerLog.CreateDate = TenantServerConfigration.GetCurrentDateTime(SchedulerLog.Tenant);
            //SchedulerLog.Log = "Start Runing the Scheduler";
            //SchedulerLogsService.Create(SchedulerLog);
        }

        private void ComputeTaskAverageRunTime(string TaskId)
        {
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                // in devart oracle DbFunctions.DiffSeconds - undeclare !!
                return;// meanwhile  no need to  ComputeTaskAverageRunTime 
            }
                
            TasksScheduler SelectedTasksScheduler = (from a in ObjectContext.TasksSchedulers
                                                     where a.Id == TaskId
                                                     select a).FirstOrDefault();

            IQueryable<TaskSchedulerHistoryPM> Latest15HistoriesQuery = (from a in ObjectContext.TaskSchedulerHistories
                                          where a.TaskId == TaskId
                                          select new TaskSchedulerHistoryPM()
                                          {
                                              StartDateTime = a.StartDateTime,
                                              EndDateTime = a.EndDateTime,
                                              Duration = DbFunctions.DiffSeconds(a.StartDateTime, a.EndDateTime),
                                              //DurationTS = a.EndDateTime - a.StartDateTime,
                                          }).Where(x => x.StartDateTime != null && x.EndDateTime != null && x.EndDateTime > x.StartDateTime).OrderByDescending(x => x.StartDateTime).Take(15);

            //double? Latest15HistoriesAverageRunTime = Latest15HistoriesQuery.Average(a => a.Duration);//.ToList();.OrderByDescending(x => x.StartDateTime).Take(10)
            double? Latest15HistoriesAverageRunTime = Latest15HistoriesQuery.Average(a => a.Duration);// DurationTS.Value.TotalSeconds);//.ToList();.OrderByDescending(x => x.StartDateTime).Take(10)

            double? Duration = 0.0;
            if (Latest15HistoriesAverageRunTime != null)
            {
                Duration = Latest15HistoriesAverageRunTime;
            }

            TasksSchedulerRepository TaskSchedulerRepository = new TasksSchedulerRepository(objectContext);
            SelectedTasksScheduler.AverageRunTime = (double)Duration;
            TaskSchedulerRepository.Update(SelectedTasksScheduler);
            TaskSchedulerRepository.SubmitChanges();
        }

    }
}