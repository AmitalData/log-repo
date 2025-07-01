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
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        private TenantManagementRepository tenantManagementRepository;
        public TasksSchedulerService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TasksSchedulerRepository(objectContext);


            this.contactRepository = new ContactRepository(tenant);
            this.tenantManagementRepository = new TenantManagementRepository();
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

        public bool isExceedsScheduledTasksLimitPerReport(int tenant, string entityId)
        {
            int scheduledTasksLimitPerReport = tenantManagementRepository.GetScheduledTasksLimitPerReport(tenant);
            int userDefinedTaskPerCurrentReport = entityRepository.GetUserTasksSchedulerPerReport(tenant, entityId);

            if(userDefinedTaskPerCurrentReport >= scheduledTasksLimitPerReport)
            {
                return true;
            }
            return false;
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
            TasksSchedulerValidator.Validate(this.entityPM, this.Poco);

            TasksSchedulerMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            ConnectDocumentTypeTemplateToScheduler();
            ConnectMessageReportTemplateToScheduler();

            if (!FeatureToggleHelper.HasFeatureToggle("STQ", entityPM.Tenant)) 
            { 
			    IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("SchedularQueue", 0);
                queueservice.Send(new Dictionary<string, string>() { { "TaskId", Poco.Id }, { "Tenant", Poco.Tenant.ToString() }, { "Version", Poco.Version.ToString() } }, tenant, null, null, null, Poco.NextRunTimeUTC);
			}
		}

        private void ConnectDocumentTypeTemplateToScheduler()
        {
            if (this.entityPM.DocumentTypeTemplateIds == null || this.entityPM.DocumentTypeTemplateIds.Count <= 0 || entityPM.Type != "Report" || entityPM.ProcedureCode != "BIReportSchedulerTask") return;
            DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(this.entityPM.Tenant);
            List<DocumentTypeTemplate> documentTypeTemplates = documentTypeTemplateRepository.GetDocumentTypeTemplatesBydocumentTypeTemplateIds(this.entityPM.DocumentTypeTemplateIds, this.entityPM.Tenant).ToList();
            string objectTableId = ObjectTableRepository.GetObjectTableByName("TasksScheduler");
            foreach (DocumentTypeTemplate documentTypeTemplate in documentTypeTemplates)
            {
                UpdateDocumentTypeTemplate(documentTypeTemplateRepository, objectTableId, documentTypeTemplate);
            }

            documentTypeTemplateRepository.SubmitChanges();
        }

        private void UpdateDocumentTypeTemplate(DocumentTypeTemplateRepository documentTypeTemplateRepository, string objectTableId, DocumentTypeTemplate documentTypeTemplate)
        {
            documentTypeTemplate.EntityId = this.entityPM.Id;
            documentTypeTemplate.ObjectTableId = objectTableId;
            documentTypeTemplateRepository.Update(documentTypeTemplate);
        }
        private void ConnectMessageReportTemplateToScheduler()
        {
            if (this.entityPM.DocumentTypeTemplateIds == null || this.entityPM.DocumentTypeTemplateIds.Count <= 0 || entityPM.Type != "Report" || entityPM.ProcedureCode != "ReportSchedulerTask") return;
            ReportsTemplateRepository reportsTemplateRepository = new ReportsTemplateRepository(this.entityPM.Tenant);
            List<ReportsTemplate> messageTemplates = reportsTemplateRepository.GetMessageTemplatesByMessageTemplateIds(this.entityPM.DocumentTypeTemplateIds, this.entityPM.Tenant).ToList();
            string objectTableId = ObjectTableRepository.GetObjectTableByName("TasksScheduler");
            foreach (ReportsTemplate messageTemplate in messageTemplates)
            {
                UpdateMessageReportTemplate(reportsTemplateRepository, objectTableId, messageTemplate);
            }

            reportsTemplateRepository.SubmitChanges();
        }

        private void UpdateMessageReportTemplate(ReportsTemplateRepository messageTemplateRepository, string objectTableId, ReportsTemplate messageTemplate)
        {
            messageTemplate.EntityId = this.entityPM.Id;
            messageTemplate.ObjectTableId = objectTableId;
            messageTemplateRepository.Update(messageTemplate);
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
            TasksSchedulerValidator.Validate(this.entityPM, this.Poco);

            if (theEntityPm.StartDateTime != Poco.StartDateTime)
            {
                theEntityPm.Version = theEntityPm.Version + 1;
                theEntityPm.NextRunTime = theEntityPm.StartDateTime;
                theEntityPm.NextRunTimeUTC = theEntityPm.StartDateTimeUTC;
                if (!FeatureToggleHelper.HasFeatureToggle("STQ", entityPM.Tenant)) 
                {
                    IQueueService queueservice = new DbQueueService();
                    queueservice.InitializeQueue("SchedularQueue", 0);
                    queueservice.Send(new Dictionary<string, string>() { { "TaskId", Poco.Id }, { "Tenant", Poco.Tenant.ToString() }, { "Version", theEntityPm.Version.ToString() } }, tenant, null, null, null, theEntityPm.NextRunTimeUTC);

                }
            }
            TasksSchedulerMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
		public void Delete(int tenant,string taskSchedularId)
		{
			this.Poco = entityRepository.GetSingleTasksScheduler(taskSchedularId, tenant);
            if (Poco == null) return;
			entityRepository.Remove(Poco);
			entityRepository.SubmitChanges();
		}

        public void RunTaskNow(string taskSchedulerId, int tenant, DateTime FromDate, DateTime ToDate)
        {
            var poco = entityRepository.GetSingleTasksScheduler(taskSchedulerId, tenant);
            if (FromDate != null && ToDate != null)
            {
                var details = new SchedulerDateRange { FromDate = FromDate, ToDate = ToDate };
                poco.SchedulerDetailsXML = LogitudeXmlSerializer.SerializeObjectToElementString(details);
                entityRepository.Update(poco);
                entityRepository.SubmitChanges();
	
            }

            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("SchedularQueue", 0);
            queueservice.Send(
                new Dictionary<string, string> {
                    { "TaskId", poco.Id },
                    { "Tenant", poco.Tenant.ToString() },
                    { "Version", poco.Version.ToString() },
                    { "IsStartedFromUI", true.ToString() }
                },
                tenant, null, null, null,null
            );
        }

    }
}
[System.Runtime.Serialization.DataContract]
public class SchedulerDateRange
{
    [System.Runtime.Serialization.DataMember]
    public DateTime FromDate { get; set; }

    [System.Runtime.Serialization.DataMember]
    public DateTime ToDate { get; set; }
}