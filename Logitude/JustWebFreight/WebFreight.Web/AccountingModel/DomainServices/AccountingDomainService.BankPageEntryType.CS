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
        public GLAccountTotalDateTypePM GetSingleGLAccountTotalDateTypePM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTotalDateTypeQuery = new GLAccountTotalDateTypeQueryService(accountingContext);
            GLAccountTotalDateTypePM GLAccountTotalDateTypePM = GLAccountTotalDateTypeQuery.GetSingle(code, true, false);
            return GLAccountTotalDateTypePM;
        }

        public GLAccountTotalDateTypeList GetSingleGLAccountTotalDateTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTotalDateTypeListQueryService listService = new GLAccountTotalDateTypeListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<GLAccountTotalDateTypeList> GetGLAccountTotalDateTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTotalDateTypeListQueryService listService = new GLAccountTotalDateTypeListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<GLAccountTotalDateTypeList> GetGLAccountTotalDateTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTotalDateTypeListQueryService listService = new GLAccountTotalDateTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetGLAccountTotalDateTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTotalDateTypeListQueryService queryService = new GLAccountTotalDateTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);//, tenant);

        }

        public void UpdateGLAccountTotalDateTypeList(GLAccountTotalDateTypeList entity)
        {

        }


    }
}