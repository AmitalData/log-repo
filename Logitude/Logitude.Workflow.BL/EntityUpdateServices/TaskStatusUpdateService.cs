using Logitude.Server.Tools.Helpers;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.Workflow.BL.EntityUpdateServices
{
    public partial class TaskStatusUpdateService
    {
        protected override void Trace(TaskStatusPM entityPM, TaskStatus entityPOCO, string changesXml)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                CreateTraceEvent(entityPM, "CREV", changesXml);
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                CreateTraceEvent(entityPM, "UPEV", changesXml);
            }
        }

        private void CreateTraceEvent(TaskStatusPM entityPM, string eventTypeCode, string changesXml)
        {
            string contactId = GetLoggedContact(entityPM.Tenant)?.Id;

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = entityPM.Tenant,
                EventTypeCode = eventTypeCode,
                UserId = contactId,
                EntityId = entityPM.Id,
                ObjectTableName = "TaskStatus",
                Notes = changesXml
            });
        }

        private Contact GetLoggedContact(int tenant)
        {
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRepository = new ContactRepository(commonDataContext);
            Contact contact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), tenant);
            return contact;
        }
    }
}
