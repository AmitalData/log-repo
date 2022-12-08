using Logitude.Server.Tools;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data;
using Logitude.Workflow.Data.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Workflow.BL.EntityUpdateServices
{
    public partial class WorkFlowUpdateService
    {
        protected override void OnCreating(WorkFlowPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                ValidateWorkflowName(entityPM, true);
            }
        }

        protected override void OnUpdating(WorkFlowPM entityPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                ValidateWorkflowName(entityPM, false);
                CreateNewVersion(entityPM);
            }
        }

        private void ValidateWorkflowName(WorkFlowPM entityPM, bool isNewEntity)
        {
            bool exists = false;
            IWorkflowContext workflowContext = WorkflowContext.GetContext(entityPM.Tenant);

            if (isNewEntity)
            {
                exists = (from w in workflowContext.WorkFlows
                          where w.Name.ToLower() == entityPM.Name.ToLower() && w.Tenant == entityPM.Tenant
                          select w).Any();
            }
            else
            {
                exists = (from w in workflowContext.WorkFlows
                          where w.Name.ToLower() == entityPM.Name.ToLower()
                          && w.Id != entityPM.Id
                          && w.Tenant == entityPM.Tenant
                          select w).Any();
            }

            if (exists)
            {
                throw new ApplicationException("Workflow name already exists");
            }
        }

        private void CreateNewVersion(WorkFlowPM entityPM)
        {
            if (!IsFirstUpdate(entityPM))
            {
                return;
            }
            var versionPM = new WorkFlowVersionPM()
            {
                Tenant = entityPM.Tenant,
                CreateDate = entityPM.CreateDate,
                UpdateDate = entityPM.UpdateDate,
                UpdatedByUserId = entityPM.CreatedByUserId,
                CreatedByUserId = entityPM.UpdatedByUserId,
                WorkflowId = entityPM.Id,
                FlowJson = entityPM.FlowJson,
                VersionNumber = 1,
                StatusCode = "DRFT",
                Description = "First Create WorkFlow Version",
                SearchFields = "First Create WorkFlow Version"
            };

            IWorkflowContext MyContext = WorkflowContext.GetContext(entityPM.Tenant);
            WorkFlowVersionUpdateService service = new WorkFlowVersionUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            versionPM.ChangeSetOp = ChangeSetOperation.Insert;
            service.Update(versionPM, true);

            if(versionPM.Id != null)
            {
                entityPM.WorkFlowActiveVersionId = versionPM.Id;
                entityPM.WorkFlowVersionStatusCode = versionPM.StatusCode;
                entityPM.WorkFlowVersionNumber = versionPM.VersionNumber;
            }
        }

        private bool IsFirstUpdate(WorkFlowPM entityPM)
        {
            WorkFlowVersionRepository workFlowVersionRepository = new WorkFlowVersionRepository(entityPM.Tenant);
            var workFlowVersions = workFlowVersionRepository.GetAllByWorkflowId(entityPM.Tenant, entityPM.Id).ToList();
            return workFlowVersions.Count() == 0;
        }
    }
}