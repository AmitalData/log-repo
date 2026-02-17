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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

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

        private ContactRepository contactRepository;
        private Contact loggedContact;
        public TasksSchedulerService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TasksSchedulerRepository(objectContext);


            this.contactRepository = new ContactRepository(tenant);
            this.GetLoggedContact();

        }


        private void GetLoggedContact()
        {

            if (HttpContext.Current != null)
            {
                string email = HttpContext.Current.User.Identity.Name;
                this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
            }
            else
            {
                string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
                this.loggedContact = contactRepository.GetSingleContactByEmail(systemContactEmail, tenant);

            }
        }



        public void Create(TasksSchedulerPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("TasksScheduler", tenant).ToString();
            this.entityPM.CreateDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdateDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            FillNextRunDateFields();

            if (this.loggedContact != null)
            {
                this.entityPM.UpdatedBy = this.loggedContact.EnglishName;
                this.entityPM.CreatedBy = this.loggedContact.EnglishName;
            }

            this.Poco = new TasksScheduler();
            this.Poco.Id = this.entityPM.Id;
           
            TasksSchedulerMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("SchedularQueue", 0);
            queueservice.Send(new Dictionary<string, string>() { { "TaskId", Poco.Id }, { "Tenant", Poco.Tenant.ToString() }, { "Version", Poco.Version.ToString() } }, null, null, null, Poco.NextRunTimeUTC);

        }

        private void FillNextRunDateFields()
        {
           
            this.entityPM.NextRunTime = this.entityPM.NextRunTime == null ? this.entityPM.StartDateTime : this.entityPM.NextRunTime;
            this.entityPM.NextRunTimeUTC = this.entityPM.NextRunTimeUTC == null ? this.entityPM.StartDateTimeUTC : this.entityPM.NextRunTimeUTC;
        }

        public void Update(TasksSchedulerPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.entityPM.UpdateDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            if (this.loggedContact != null) this.entityPM.UpdatedBy = this.loggedContact.EnglishName;

            FillNextRunDateFields();


            this.Poco = entityRepository.GetSingleTasksScheduler(theEntityPm.Id, theEntityPm.Tenant);
            if (theEntityPm.StartDateTime != Poco.StartDateTime)
            {
                theEntityPm.Version = theEntityPm.Version + 1;
                theEntityPm.NextRunTime = theEntityPm.StartDateTime;
                theEntityPm.NextRunTimeUTC = theEntityPm.StartDateTimeUTC;
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("SchedularQueue", 0);
                queueservice.Send(new Dictionary<string, string>() { { "TaskId", Poco.Id }, { "Tenant", Poco.Tenant.ToString() }, { "Version", theEntityPm.Version.ToString() } }, null, null, null, theEntityPm.NextRunTimeUTC);

            }
            TasksSchedulerMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}