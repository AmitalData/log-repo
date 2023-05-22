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
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityListQueryServices;

namespace WebFreight.Web.AccountingModel.DomainServices
{
    [EnableClientAccess()]
    public partial class OpenFormatReportDomainService : LogitudeDomainService
    {
        public List<OpenFormatReportList> GetOpenFormatReportFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("OpenFormatReport", "READ", tenant);

            IAccountingContext context = AccountingContext.GetContext(tenant);

            OpenFormatReportListQueryService listService = new OpenFormatReportListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<OpenFormatReportList> listQuery = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("OpenFormatReport", tenant, listQuery.Cast<object>().ToList());

            return listQuery;
        }

        public int GetOpenFormatReportFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("OpenFormatReport", "READ", tenant);

            IAccountingContext context = AccountingContext.GetContext(tenant);

            OpenFormatReportListQueryService queryService = new OpenFormatReportListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }
    }
}