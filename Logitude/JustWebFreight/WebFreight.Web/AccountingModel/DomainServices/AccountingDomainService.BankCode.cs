using Logitude.Accounting.BL.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
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
        public BankCodePM GetSingleBankCodePM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            bankCodeQuery = new BankCodeQueryService(accountingContext);
            BankCodePM BankCode = bankCodeQuery.GetSingle(code, true, false);
            return BankCode;
        }

        public BankCodeList GetSingleBankCodeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Accounting.BankCode", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            BankCodeListQueryService listService = new BankCodeListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<BankCodeList> GetBankCodeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Accounting.JournalActionType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            BankCodeListQueryService listService = new BankCodeListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<BankCodeList> GetBankCodeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Accounting.BankCode", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            BankCodeListQueryService listService = new BankCodeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetBankCodeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Accounting.BankCode", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            BankCodeListQueryService queryService = new BankCodeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

       


        public void InsertBankCode(BankCodePM entityPm)
        {
            SecurityUtility.CheckContactFeature("Accounting.BankCode", "NEW", entityPm.Tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(entityPm.Tenant);
            }


            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            BankCodeUpdateService service = new BankCodeUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPm.Tenant);

            service.Update(entityPm, true);
        }

        public void UpdateBankCode(BankCodePM currententityPm)
        {
            SecurityUtility.CheckContactFeature("Accounting.BankCode", "UPDATE", currententityPm.Tenant);
            var sssss = this.ChangeSet.ChangeSetEntries;
            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(currententityPm.Tenant);
            }
            //currententityPm.MarkAsChanged = true;

            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            BankCodeUpdateService service = new BankCodeUpdateService(accountingContext, new Dictionary<string, IContext>(), currententityPm.Tenant);

            service.Update(currententityPm, true);
        }
    }
}