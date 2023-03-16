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
        public string contactId = "";
        public string eventTypeCode = "";
        protected override void Trace(TaskStatusPM entityPM, TaskStatus entityPOCO, string changesXml)
        {
            GetLoggedContact(entityPM.Tenant);
            GetEventTypeCode(entityPM);

            if (!string.IsNullOrEmpty(contactId) && !string.IsNullOrEmpty(eventTypeCode))
            {
                CreateTraceEvent(entityPM, changesXml);
            }
        }

        private void GetLoggedContact(int tenant)
        {
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRepository = new ContactRepository(commonDataContext);
            Contact contact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), tenant);
            contactId = contact?.Id;
        }

        private void GetEventTypeCode(TaskStatusPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                eventTypeCode = "CREV";
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                eventTypeCode = "UPEV";
            }
        }

        private void CreateTraceEvent(TaskStatusPM entityPM, string changesXml)
        {
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
    }
}
