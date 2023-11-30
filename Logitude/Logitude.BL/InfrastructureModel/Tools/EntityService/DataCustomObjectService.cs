using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.BL.Workfkow;
using Logitude.BL.Workfkow.Constants;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Models.AuditLog;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Collections.Generic;
using System.Web;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class DataCustomObjectService
    {
        bool isNewEntity;
        private int tenant;
        public DataCustomObject Poco { get; set; }
        private bool isChange = false;
        private IWebFreightContext context;
        public IWebFreightContext Context
        {
            get { return context; }
            set { context = value; }
        }
        private DataCustomObjectPM entityPM;
        private DataCustomObjectRepository entityRepository;
        private Contact loggedContact;
        private List<FieldChange> FieldChanges = new List<FieldChange>();
        private AuditLogRepository AuditLogRepository;

        public DataCustomObjectService(IWebFreightContext context, int tenant)
        {
            FieldChanges = new List<FieldChange>();
            AuditLogRepository = new AuditLogRepository(tenant);

            this.tenant = tenant;
            this.isChange = false;
            this.Context = context;
            this.entityRepository = new DataCustomObjectRepository(context);
            this.GetLoggedContact();
        }

        public void Create(DataCustomObjectPM dataCustomObjectPM)
        {
            this.isNewEntity = true;
            this.entityPM = dataCustomObjectPM;
            this.entityPM.Id = IdCounter.GetNumber("DataCustomObject", tenant).ToString();
            this.entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdatedBy = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.UpdatedBy;
            this.entityPM.CreatedBy = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.CreatedBy;
            this.Poco = new DataCustomObject();
            new CustomChildEntityService(new CustomChildEntityArgs() { ParentEntity = entityPM, ParentEntityId = entityPM.Id, ParentObjectTableId = entityPM.ObjectTableId, Tenant = tenant }).Update();
            DataCustomObjectMapping.MapEntity(dataCustomObjectPM, Poco, isNewEntity, FieldChanges);

            AuditLog auditLog = AddDataCustomObjectAuditLog();
            AddWorkflowEntityQueueMessage(QueueMessagesTypes.Create, auditLog?.Id);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DataCustomObjectPM dataCustomObjectPM)
        {
            this.isNewEntity = false;
            this.entityPM = dataCustomObjectPM;
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdatedBy = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.UpdatedBy;
            this.Poco = entityRepository.GetSingleDataCustomObject(dataCustomObjectPM.Id, dataCustomObjectPM.Tenant);
            if (this.Poco == null) return;
            new CustomChildEntityService(new CustomChildEntityArgs() { ParentEntity = entityPM, ParentEntityId = entityPM.Id, ParentObjectTableId = entityPM.ObjectTableId, Tenant = tenant }).Update();
            DataCustomObjectMapping.MapEntity(dataCustomObjectPM, Poco, isNewEntity, FieldChanges);

            AuditLog auditLog = AddDataCustomObjectAuditLog();
            AddWorkflowEntityQueueMessage(QueueMessagesTypes.Update, auditLog?.Id);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private void AddWorkflowEntityQueueMessage(string type, string auditLogId)
        {
            ObjectTableRepository objecttableRepository = new ObjectTableRepository(Poco.Tenant);
            ObjectTable objecttable = objecttableRepository.GetObjectTableById(Poco.ObjectTableId, Poco.Tenant);

            new WorkflowEntityQueueMessage()
            {
                Entity = objecttable.Name,
                EntityId = Poco.Id,
                AuditLogId = auditLogId,
                Tenant = Poco.Tenant,
                Type = type,
                IsCustom = true
            }.Produce();
        }

        public void Delete(DataCustomObjectPM dataCustomObjectPM)
        {
            this.Poco = entityRepository.GetSingleDataCustomObject(dataCustomObjectPM.Id, dataCustomObjectPM.Tenant);
            if (this.Poco == null) return;
            entityRepository.Remove(Poco);
            entityRepository.SubmitChanges();
        }

        private void GetLoggedContact()
        {
            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                this.loggedContact = new ContactRepository(tenant).GetSingleContactByEmail((HttpContext.Current.User.Identity.Name), tenant , true);
                return;
            }
            this.loggedContact = new ContactRepository(tenant).GetSingleContactByEmail(("system@tenant" + tenant.ToString() + ".com"), tenant,true);
        }

        private AuditLog AddDataCustomObjectAuditLog()
        {
            AuditLog auditLog = null;
            if (entityPM != null && FeatureToggleHelper.HasFeatureToggle("ADL", entityPM.Tenant))
            {
                auditLog = AddDataCustomObjectAuditLogChanges(Poco);
                AuditLogRepository.Add(auditLog);
                AuditLogRepository.SubmitChanges();
            }

            return auditLog;
        }

        private AuditLog AddDataCustomObjectAuditLogChanges(DataCustomObject entityPoco)
        {
            return new AuditLog()
            {
                Id = IdCounter.GetNumber("AuditLog", entityPoco.Tenant).ToString(),
                Tenant = entityPoco.Tenant,
                UpdateDate = entityPoco.UpdateDate,
                UpdatedByUserId = entityPoco.UpdatedBy,
                EntityId = entityPoco.Id,
                ObjectTableId = entityPoco.ObjectTableId,
                ChangesJson = JsonConvert.SerializeObject(FieldChanges)
            };
        }
    }
}