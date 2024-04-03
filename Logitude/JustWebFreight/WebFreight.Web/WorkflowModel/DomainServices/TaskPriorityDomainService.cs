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
    public partial class TaskPriorityDomainService : LogitudeDomainService
    {
        public List<TaskPriorityList> GetTaskPriorityFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TaskPriority", "READ", tenant);

            IWorkflowContext context = WorkflowContext.GetContext(tenant);

            TaskPriorityListQueryService listService = new TaskPriorityListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<TaskPriorityList> listQuery = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("TaskPriority", tenant, listQuery.Cast<object>().ToList());

            return listQuery;
        }

        public int GetTaskPriorityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TaskPriority", "READ", tenant);

            IWorkflowContext context = WorkflowContext.GetContext(tenant);

            TaskPriorityListQueryService queryService = new TaskPriorityListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }
    }
}