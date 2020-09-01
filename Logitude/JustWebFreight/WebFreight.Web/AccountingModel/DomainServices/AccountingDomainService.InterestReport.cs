using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.AccountingModel.DomainServices
{
	public partial class AccountingDomainService
	{
        public List<InterestReportList> GetInterestReportFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.SupplierInvoiceItemsTax", "READ", tenant);

            accountingContext = AccountingContext.GetContext(tenant);
            InterestReportListQueryService listService = new InterestReportListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetInterestReportFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.SupplierInvoiceItemsTax", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            InterestReportListQueryService queryService = new InterestReportListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
    }
}