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
    public partial class TaxReportDomainService : LogitudeDomainService
    {
        public List<TaxReportList> GetTaxReportFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TaxReport", "READ", tenant);

            IAccountingContext context = AccountingContext.GetContext(tenant);

            TaxReportListQueryService listService = new TaxReportListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<TaxReportList> listQuery = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("TaxReport", tenant, listQuery.Cast<object>().ToList());

            return listQuery;
        }

        public int GetTaxReportFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TaxReport", "READ", tenant);

            IAccountingContext context = AccountingContext.GetContext(tenant);

            TaxReportListQueryService queryService = new TaxReportListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }
    }
}