using Logitude.Server.Tools;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data.EntityKeys;
using Logitude.Workflow.Data.EntityPOCOs;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Workflow.BL.EntityQueryServices
{
    public partial class WorkFlowVersionQueryService : EntityQueryService<WorkFlowVersion, WorkFlowVersionKeys, WorkFlowVersionPM, object, WorkFlowVersionKeys>
    {
        public List<string> GetVersionIds(string workflowId, int tenant)
        {
            var versionIds = repository.GetAllByWorkflowId(tenant, workflowId).ToList().Select(ca => ca.Id).ToList();
            return versionIds;
        }

        public List<WorkFlowVersionPM> GetAllWorkflowVersions(string workflowId, int tenant)
        {
            List<WorkFlowVersionPM> query = (from a in context.WorkFlowVersions
                                             where a.WorkflowId == workflowId
                                             && a.Tenant == tenant
                                             orderby a.VersionNumber
                                             select new WorkFlowVersionPM()
                                             {
                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 CreateDate = a.CreateDate,
                                                 CreatedByUserId = a.CreatedByUserId,
                                                 UpdateDate = a.UpdateDate,
                                                 UpdatedByUserId = a.UpdatedByUserId,
                                                 SearchFields = a.SearchFields,
                                                 VersionNumber = a.VersionNumber,
                                                 Description = a.Description,
                                                 StatusName = a.Status.Name,
                                                 StatusCode = a.StatusCode,
                                                 WorkflowId = a.WorkflowId,
                                                 FlowJson = a.FlowJson,
                                                 Entity = a.Entity,
                                                 Trigger = a.Trigger,
                                                 ActivatedDate = a.ActivatedDate
                                             }).ToList();
            return query;
        }
    }
}