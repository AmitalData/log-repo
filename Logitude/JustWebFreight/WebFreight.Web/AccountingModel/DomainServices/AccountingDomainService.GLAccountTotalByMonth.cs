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
using Logitude.Accounting.BL.CloseTables;

namespace WebFreight.Web.AccountingModel.DomainServices
{
    public partial class AccountingDomainService
    {
        public GLAccountTotalByMonthPM GetSingleGLAccountTotalByMonthPM(string code, int year, int month, string currencyId , int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            gLAccountTotalByMonthQuery = new GLAccountTotalByMonthQueryService(accountingContext);
            GLAccountTotalByMonthPM gLAccountTotalByMonthPM = gLAccountTotalByMonthQuery.GetSingle(code,GLAccountTotalDateTypeValues.Accountingdate, year, month, currencyId, true, false);
            return gLAccountTotalByMonthPM;
        }

        //public GLAccountTotalByMonthList GetSingleGLAccountTotalByMonthList(string code, int year, int tenant)
        //{

        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    //SecurityUtility.CheckContactFeature("GLAccountTotalByMonth", "READ", tenant);
        //    accountingContext = AccountingContext.GetContext(tenant);
        //    GLAccountTotalByMonthListQueryService listService = new GLAccountTotalByMonthListQueryService(accountingContext);
        //    return listService.GetList(tenant);
        //}


        public GLAccountTotalByMonthList GetSingleGLAccountTotalByMonthList(string code, int year, int month, string currencyId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("GLAccountTotalByMonth", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTotalByMonthListQueryService listService = new GLAccountTotalByMonthListQueryService(accountingContext);
            return listService.GetSingle(code,GLAccountTotalDateTypeValues.Accountingdate,year, month, currencyId );
        }

        public List<GLAccountTotalByMonthList> GetGLAccountTotalByMonthLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("GLAccountTotalByMonth", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTotalByMonthListQueryService listService = new GLAccountTotalByMonthListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        [Invoke]
        public decimal? GetBalanceLocalAmountByMonthAndCurrncy(string gLAccointId, DateTime date, int tenant, string CurrencyId)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTotalByMonthQueryService ServiceQuery = new GLAccountTotalByMonthQueryService(accountingContext);
            var myBalance = ServiceQuery.GetBalanceLocalAmountByMonthAndCurrency(gLAccointId, date.Year, date.Month, tenant, CurrencyId);
            return myBalance;
        }


        [Invoke]
        public List<CurrencySum> GetGLAccountTotalByMonthsForMonthSum(string gLAccointId, DateTime date, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTotalByMonthQueryService ServiceQuery = new GLAccountTotalByMonthQueryService(accountingContext);
            return ServiceQuery.GetSumByMonth(gLAccointId, date.Year, date.Month, tenant);
        }



        public List<GLAccountTotalByMonthList> GetGLAccountTotalByMonthFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("GLAccountTotalByMonth", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTotalByMonthListQueryService listService = new GLAccountTotalByMonthListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetGLAccountTotalByMonthFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("GLAccountTotalByMonth", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTotalByMonthListQueryService queryService = new GLAccountTotalByMonthListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertGLAccountTotalByMonth(GLAccountTotalByMonthPM entityPm)
        {
            SecurityUtility.CheckContactFeature("GLAccountTotalByMonth", "NEW", entityPm.Tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(entityPm.Tenant);
            }


            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            GLAccountTotalByMonthUpdateService service = new GLAccountTotalByMonthUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPm.Tenant);

            service.Update(entityPm, true);
        }

        public void UpdateGLAccountTotalByMonth(GLAccountTotalByMonthPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("GLAccountTotalByMonth", "UPDATE", currententityPm.Tenant);
            var sssss = this.ChangeSet.ChangeSetEntries;
            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(currententityPm.Tenant);
            }
            //currententityPm.MarkAsChanged = true;

            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            GLAccountTotalByMonthUpdateService service = new GLAccountTotalByMonthUpdateService(accountingContext, new Dictionary<string, IContext>(), currententityPm.Tenant);

            service.Update(currententityPm, true);

        }
    }
}