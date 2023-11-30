
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Workflow.Data.Repositories
{
    public partial class WorkFlowVersionRepository : IRepository<WorkFlowVersion>
    {

        public List<WorkFlowVersion> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public IQueryable<WorkFlowVersion> GetAllByWorkflowId(int tenant, string workflowId)
        {
            return from a in context.WorkFlowVersions
                   where a.Tenant == tenant && a.WorkflowId == workflowId
                   orderby a.VersionNumber
                   select a;
        }

        public IQueryable<WorkFlowVersion> GetAllVersionByStatus(int tenant, string workflowId, string statusCode)
        {
            return from a in context.WorkFlowVersions
                   where a.Tenant == tenant && a.WorkflowId == workflowId && a.StatusCode == statusCode
                   orderby a.VersionNumber
                   select a;
        }



    }

}
