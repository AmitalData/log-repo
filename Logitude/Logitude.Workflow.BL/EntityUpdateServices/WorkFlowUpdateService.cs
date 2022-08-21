using Logitude.Server.Tools;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data;
using Simplog.Server.Infrastructure;
using System;
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
    }
}