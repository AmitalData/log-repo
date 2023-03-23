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
    public partial class WorkFlowInstanceDomainService : LogitudeDomainService
    {
        public List<WorkFlowInstanceList> GetWorkFlowInstanceFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("WorkFlowInstance", "READ", tenant);

            IWorkflowContext context = WorkflowContext.GetContext(tenant);

            WorkFlowInstanceListQueryService listService = new WorkFlowInstanceListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<WorkFlowInstanceList> listQuery = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("WorkFlowInstance", tenant, listQuery.Cast<object>().ToList());

            return listQuery;
        }

        public int GetWorkFlowInstanceFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("WorkFlowInstance", "READ", tenant);

            IWorkflowContext context = WorkflowContext.GetContext(tenant);

            WorkFlowInstanceListQueryService queryService = new WorkFlowInstanceListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }
    }
}