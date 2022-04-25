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
        public List<InterestTransactionList> GetGLAccountInterestTransactionsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.SupplierInvoiceItemsTax", "READ", tenant);

            accountingContext = AccountingContext.GetContext(tenant);
            InterestTransactionListQueryService listService = new InterestTransactionListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            queryOperations.QueryFilterItems.ForEach(x => x.IsCustom = false);
            var entityLists = listService.GetList(queryOperations, tenant);

            return listService.MapListQuery(entityLists, tenant, true);

        }

        public int GetGLAccountInterestTransactionsFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.SupplierInvoiceItemsTax", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            InterestTransactionListQueryService queryService = new InterestTransactionListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
    }
}