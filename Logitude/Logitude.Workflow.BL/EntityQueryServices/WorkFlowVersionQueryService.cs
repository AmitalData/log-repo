using Logitude.Server.Tools;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data.EntityKeys;
using Logitude.Workflow.Data.EntityPOCOs;
using System.Collections.Generic;
using System.Linq;
namespace Logitude.Workflow.BL.EntityQueryServices
{
    public partial class WorkFlowVersionQueryService: EntityQueryService<WorkFlowVersion,WorkFlowVersionKeys,WorkFlowVersionPM,object,WorkFlowVersionKeys>
   {
        public List<string> GetVersionIds(string workflowId, int tenant)
        {
            var versionIds = repository.GetAllByWorkflowId(tenant,workflowId).ToList().Select(ca => ca.Id).ToList();
            return versionIds;
        }
    }
   
}
	 