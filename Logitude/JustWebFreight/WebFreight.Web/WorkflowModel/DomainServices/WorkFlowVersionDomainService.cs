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
    public partial class WorkFlowVersionDomainService : LogitudeDomainService
    {
        public List<WorkFlowVersionList> GetWorkFlowVersionFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("WorkFlowVersion", "READ", tenant);

            IWorkflowContext context = WorkflowContext.GetContext(tenant);

            WorkFlowVersionListQueryService listService = new WorkFlowVersionListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<WorkFlowVersionList> listQuery = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("WorkFlowVersion", tenant, listQuery.Cast<object>().ToList());

            return listQuery;
        }

        public int GetWorkFlowVersionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("WorkFlowVersion", "READ", tenant);

            IWorkflowContext context = WorkflowContext.GetContext(tenant);

            WorkFlowVersionListQueryService queryService = new WorkFlowVersionListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }
    }
}