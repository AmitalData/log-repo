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
    public partial class TaxDeductionReportDomainService : LogitudeDomainService
    {
        public List<TaxDeductionReportList> GetTaxDeductionReportFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TaxDeductionReport", "READ", tenant);

            IAccountingContext context = AccountingContext.GetContext(tenant);

            TaxDeductionReportListQueryService listService = new TaxDeductionReportListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<TaxDeductionReportList> listQuery = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("TaxDeductionReport", tenant, listQuery.Cast<object>().ToList());

            return listQuery;
        }

        public int GetTaxDeductionReportFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TaxDeductionReport", "READ", tenant);

            IAccountingContext context = AccountingContext.GetContext(tenant);

            TaxDeductionReportListQueryService queryService = new TaxDeductionReportListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }
    }
}