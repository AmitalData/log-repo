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
    public partial class TaskTypeDomainService : LogitudeDomainService
    {
        public List<TaskTypeList> GetTaskTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TaskType", "READ", tenant);

            IWorkflowContext context = WorkflowContext.GetContext(tenant);

            TaskTypeListQueryService listService = new TaskTypeListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<TaskTypeList> listQuery = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("TaskType", tenant, listQuery.Cast<object>().ToList());

            return listQuery;
        }

        public int GetTaskTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TaskType", "READ", tenant);

            IWorkflowContext context = WorkflowContext.GetContext(tenant);

            TaskTypeListQueryService queryService = new TaskTypeListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }
    }
}