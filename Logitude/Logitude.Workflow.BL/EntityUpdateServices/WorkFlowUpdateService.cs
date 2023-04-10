using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Linq;

namespace Logitude.Workflow.BL.EntityUpdateServices
{
    public partial class WorkFlowUpdateService
    {
        public readonly int MaxDifferenceTime = 1;
        public readonly int MaxRetryNumber = 5;
        protected override void OnCreating(WorkFlowPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPM.WorkFlowNumber = CodeCounter.GetNumber("WorkFlow", entityPM.Tenant).ToString();

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
            if (entityPM.RetriesNumber > MaxRetryNumber)
            {
                throw new ApplicationException("The maximum number of retries is " + MaxRetryNumber.ToString());
            }
        }

        private void ValidateRetriesDelay(WorkFlowPM entityPM)
        {
            var delayString = entityPM.RetriesDelay.Split(',');
            if (delayString.Count() != entityPM.RetriesNumber)
            {
                throw new ApplicationException("Retries Delay is not valid");
            }
            for (int i = 0; i < delayString.Count(); i++)
            {
                if (delayString[i] == null || string.IsNullOrEmpty(delayString[i]) || int.Parse(delayString[i]) == 0)
                {
                    throw new ApplicationException("Retries Delay is not valid");
                }
                else if (i != 0 && (int.Parse(delayString[i]) - int.Parse(delayString[i - 1])) < MaxDifferenceTime)
                {
                    throw new ApplicationException("Retries Delay is not valid");
                }
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