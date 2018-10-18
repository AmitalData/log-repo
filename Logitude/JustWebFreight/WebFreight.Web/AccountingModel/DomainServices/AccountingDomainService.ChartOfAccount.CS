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
        public ChartOfAccountPM GetSingleChartOfAccountPM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            chartOfAccountQuery = new ChartOfAccountQueryService(accountingContext);
            ChartOfAccountPM chartOfAccountPM = chartOfAccountQuery.GetSingle(code, true, false);
            return chartOfAccountPM;
        }

        [Invoke]
        public bool CheckWhetherChartOfAccountCodeExists(string code, string id, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            chartOfAccountQuery = new ChartOfAccountQueryService(accountingContext);
            return this.chartOfAccountQuery.CheckWhetherCodeExists(code, id, tenant);
        }


        public ChartOfAccountList GetSingleChartOfAccountList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("ChartOfAccount", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            ChartOfAccountListQueryService listService = new ChartOfAccountListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<ChartOfAccountList> GetChartOfAccountLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("ChartOfAccount", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ChartOfAccountListQueryService listService = new ChartOfAccountListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<ChartOfAccountList> GetChartOfAccountFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("ChartOfAccount", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ChartOfAccountListQueryService listService = new ChartOfAccountListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public void UpdateChartOfAccountList(ChartOfAccountList entity)
        {

        }

        public int GetChartOfAccountFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("ChartOfAccount", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ChartOfAccountListQueryService queryService = new ChartOfAccountListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertChartOfAccount(ChartOfAccountPM entityPm)
        {
            SecurityUtility.CheckContactFeature("ChartOfAccount", "NEW", entityPm.Tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(entityPm.Tenant);
            }


            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            ChartOfAccountUpdateService service = new ChartOfAccountUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPm.Tenant);

            service.Update(entityPm, true);
        }

        public void UpdateChartOfAccount(ChartOfAccountPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("ChartOfAccount", "UPDATE", currententityPm.Tenant);
            var sssss = this.ChangeSet.ChangeSetEntries;
            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(currententityPm.Tenant);
            }
            //currententityPm.MarkAsChanged = true;

            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            ChartOfAccountUpdateService service = new ChartOfAccountUpdateService(accountingContext, new Dictionary<string, IContext>(), currententityPm.Tenant);

            service.Update(currententityPm, true);

        }



    }
}