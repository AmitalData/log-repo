using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.AccountingModel.DomainServices
{
    public partial class AccountingDomainService
    {
        public AccountingPeriodPM GetSingleAccountingPeriodPM(string id, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            accountingPeriodQuery = new AccountingPeriodQueryService(accountingContext);
            AccountingPeriodPM accountingPeriodPM = accountingPeriodQuery.GetSingle(id, true, false);
            return accountingPeriodPM;
        }

        //[Invoke]
        //public bool CheckIfDisplayNumberExist(string displayNo, string internalNumber, int tenant)
        //{
        //    accountingContext = AccountingContext.GetContext(tenant);
        //    accountingPeriodQuery = new AccountingPeriodQueryService(accountingContext);
        //    return this.accountingPeriodQuery.CheckIfDisplayNumberExist(displayNo, internalNumber, tenant);
        //}

        public AccountingPeriodList GetSingleAccountingPeriodList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            AccountingPeriodListQueryService listService = new AccountingPeriodListQueryService(accountingContext);
            return listService.GetSingle(id);
        }

        public List<AccountingPeriodList> GetAccountingPeriodLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            AccountingPeriodListQueryService listService = new AccountingPeriodListQueryService(accountingContext);
            return listService.GetList(tenant);
        }

        //public List<AccountingPeriodList> GetAccountingPeriodLists(int tenant,int Year)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
        //    accountingContext = AccountingContext.GetContext(tenant);
        //    AccountingPeriodListQueryService listService = new AccountingPeriodListQueryService(accountingContext);
        //    return listService.GetList(tenant);
        //}

        public List<AccountingPeriodList> GetAccountingPeriodFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            AccountingPeriodListQueryService listService = new AccountingPeriodListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetAccountingPeriodFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            AccountingPeriodListQueryService queryService = new AccountingPeriodListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }


        public void InsertAccountingPeriod(AccountingPeriodPM entityPm)
        {
            SecurityUtility.CheckContactFeature("AccountingPeriod", "NEW", entityPm.Tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(entityPm.Tenant);
            }


            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            AccountingPeriodUpdateService service = new AccountingPeriodUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPm.Tenant);

            service.Update(entityPm, true);
        }

        public void UpdateAccountingPeriod(AccountingPeriodPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("AccountingPeriod", "UPDATE", currententityPm.Tenant);
            var sssss = this.ChangeSet.ChangeSetEntries;
            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(currententityPm.Tenant);
            }
            //currententityPm.MarkAsChanged = true;

            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            AccountingPeriodUpdateService service = new AccountingPeriodUpdateService(accountingContext, new Dictionary<string, IContext>(), currententityPm.Tenant);

            service.Update(currententityPm, true);

        }
        [Delete]
        public void DeleteAccountingPeriod(AccountingPeriodPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("AccountingPeriod", "UPDATE", currententityPm.Tenant);
            var sssss = this.ChangeSet.ChangeSetEntries;
            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(currententityPm.Tenant);
            }
            //currententityPm.MarkAsChanged = true;

            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
            AccountingPeriodUpdateService service = new AccountingPeriodUpdateService(accountingContext, new Dictionary<string, IContext>(), currententityPm.Tenant);

            service.Update(currententityPm, true);

        }


        public List<AccountingPeriodList> GetAccountingPeriodListByYear(int year, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountingPeriodQuery = new AccountingPeriodQueryService(tenant);
            return accountingPeriodQuery.GetAccountingPeriodListByYear(year, tenant);
        }

        public List<AccountingPeriodList> GetAccountingPeriodListByYearAndType(int year, string periodTypeCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountingPeriodQuery = new AccountingPeriodQueryService(tenant);
            return accountingPeriodQuery.GetAccountingPeriodListByYearAndType(year, periodTypeCode, tenant);
        }

        public void UpdateAccountingPeriodList(AccountingPeriodList entity)
        {

        }


    }
}