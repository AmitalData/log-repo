using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Utilities;
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
        public List<TaxReportLineList> GetTaxReportLineFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.SupplierInvoiceItemsTax", "READ", tenant);

            accountingContext = AccountingContext.GetContext(tenant);
            TaxReportLineListQueryService listService = new TaxReportLineListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetTaxReportLineFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.SupplierInvoiceItemsTax", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            TaxReportLineListQueryService queryService = new TaxReportLineListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
    }
}