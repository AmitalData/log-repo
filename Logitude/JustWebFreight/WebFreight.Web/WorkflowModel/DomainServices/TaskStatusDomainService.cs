using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.Workflow.Data;
using Logitude.Workflow.Data.EntityListQueryServices;
using Logitude.Workflow.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using System.Collections.Generic;
using System.Linq;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.ServiceModel.DomainServices.Hosting;

namespace WebFreight.Web.WorkflowModel.DomainServices
{
    [EnableClientAccess()]
    public partial class TaskStatusDomainService : LogitudeDomainService
    {
        public List<TaskStatusList> GetTaskStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TaskStatus", "READ", tenant);

            IWorkflowContext context = WorkflowContext.GetContext(tenant);

            TaskStatusListQueryService listService = new TaskStatusListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<TaskStatusList> listQuery = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("TaskStatus", tenant, listQuery.Cast<object>().ToList());

            return listQuery;
        }

        public int GetTaskStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TaskStatus", "READ", tenant);

            IWorkflowContext context = WorkflowContext.GetContext(tenant);

            TaskStatusListQueryService queryService = new TaskStatusListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }
    }
}