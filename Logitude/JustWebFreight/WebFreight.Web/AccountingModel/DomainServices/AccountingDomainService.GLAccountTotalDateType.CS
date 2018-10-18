using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
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
        public BankPageEntryTypePM GetSingleBankPageEntryTypePM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            BankPageEntryTypeQuery = new BankPageEntryTypeQueryService(accountingContext);
            BankPageEntryTypePM BankPageEntryTypePM = BankPageEntryTypeQuery.GetSingle(code, true, false);
            return BankPageEntryTypePM;
        }

        public BankPageEntryTypeList GetSingleBankPageEntryTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            BankPageEntryTypeListQueryService listService = new BankPageEntryTypeListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<BankPageEntryTypeList> GetBankPageEntryTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            BankPageEntryTypeListQueryService listService = new BankPageEntryTypeListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<BankPageEntryTypeList> GetBankPageEntryTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            BankPageEntryTypeListQueryService listService = new BankPageEntryTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetBankPageEntryTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            BankPageEntryTypeListQueryService queryService = new BankPageEntryTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);//, tenant);

        }

        public void UpdateBankPageEntryTypeList(BankPageEntryTypeList entity)
        {

        }


    }
}