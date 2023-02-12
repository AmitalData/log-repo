using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Workflow.BL.EntityDataMappings;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data;
using Logitude.Workflow.Data.EntityPOCOs;
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
                ValidateRetriesNumber(entityPM);
                ValidateRetriesDelay(entityPM);
                CreateNewVersion(entityPM);
            }
        }

        protected override void OnUpdating(WorkFlowPM entityPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                ValidateWorkflowName(entityPM, false);
                ValidateRetriesNumber(entityPM);
                ValidateRetriesDelay(entityPM);
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

        private void ValidateRetriesNumber(WorkFlowPM entityPM)
        {
            if (entityPM.RetriesNumber > 10)
            {
                throw new ApplicationException("The maximum number of retries is 10");
            }
        }

        private void ValidateRetriesDelay(WorkFlowPM entityPM)
        {
            if (entityPM.RetriesDelay > 0 && entityPM.RetriesDelay < 120)
            {
                throw new ApplicationException("The minimum number of retries delay is 120");
            }
        }

        private void CreateNewVersion(WorkFlowPM entityPM)
        {
            var versionPOCO = new WorkFlowVersion()
            {
                Id = IdCounter.GetNumber("WorkFlowVersion", entityPM.Tenant),
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
                SearchFields = "First Create WorkFlow Version",
                Entity = entityPM.Entity,
                Trigger = entityPM.Trigger
            };

            IWorkflowContext context = MainContext as WorkflowContext;
            WorkFlowVersionRepository workFlowVersionRepository = new WorkFlowVersionRepository(context);
            workFlowVersionRepository.Add(versionPOCO);
        }
    }
}