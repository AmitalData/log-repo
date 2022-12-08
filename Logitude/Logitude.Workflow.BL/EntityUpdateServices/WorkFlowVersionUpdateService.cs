using Logitude.Server.Tools;
using Logitude.Workflow.BL.EntityDataMappings;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data;
using Logitude.Workflow.Data.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Workflow.BL.EntityUpdateServices
{
    public partial class WorkFlowVersionUpdateService
    {
        protected override void OnCreating(WorkFlowVersionPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                WorkFlowVersionRepository workFlowVersionRepository = new WorkFlowVersionRepository(entityPM.Tenant);

                SetDefaultValues(entityPM, workFlowVersionRepository);
                DeactiveVersionStatus(entityPM, workFlowVersionRepository);
                UpdateWorkflowWithVersionId(entityPM);
            }
        }

        protected override void OnUpdating(WorkFlowVersionPM entityPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                WorkFlowVersionRepository workFlowVersionRepository = new WorkFlowVersionRepository(entityPM.Tenant);

                DeactiveVersionStatus(entityPM, workFlowVersionRepository);
                UpdateWorkflowWithVersionId(entityPM);

            }
        }

        private static void SetDefaultValues(WorkFlowVersionPM entityPM, WorkFlowVersionRepository workFlowVersionRepository)
        {
            var workFlowVersions = workFlowVersionRepository.GetAllByWorkflowId(entityPM.Tenant, entityPM.WorkflowId).ToList();
            var versionNumber = workFlowVersions?.Count() == 0 ? 1 : workFlowVersions.LastOrDefault().VersionNumber + 1;
            entityPM.VersionNumber = versionNumber;
            entityPM.StatusCode = versionNumber == 1 ? "DRFT" : "ACVE";
        }

        private static void DeactiveVersionStatus(WorkFlowVersionPM entityPM, WorkFlowVersionRepository workFlowVersionRepository)
        {
            var workFlowVersions = workFlowVersionRepository.GetAllVersionByStatus(entityPM.Tenant, entityPM.WorkflowId, "ACVE").ToList();
            foreach (var version in workFlowVersions)
            {
                if (version.Id != entityPM.Id)
                {
                    version.StatusCode = "INVE";
                    workFlowVersionRepository.Update(version);
                }
            }

            workFlowVersionRepository.SubmitChanges();
        }

        private static void UpdateWorkflowWithVersionId(WorkFlowVersionPM entityPM)
        {
            WorkFlowRepository workFlowRepository = new WorkFlowRepository(entityPM.Tenant);
            var workflow = workFlowRepository.GetSingle(entityPM.WorkflowId, entityPM.Tenant);
            workflow.WorkFlowActiveVersionId = entityPM.Id;
            workflow.WorkFlowVersionStatusCode = entityPM.StatusCode;
            workflow.FlowJson = entityPM.FlowJson;
            workflow.WorkFlowVersionNumber = entityPM.VersionNumber;
            workFlowRepository.Update(workflow);
            workFlowRepository.SubmitChanges();
        }
    }
}
