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
        public GLAccountTypePM GetSingleGLAccountTypePM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            gLAccountTypeQuery = new GLAccountTypeQueryService(accountingContext);
            GLAccountTypePM gLAccountTypePM = gLAccountTypeQuery.GetSingle(code, true, false);
            return gLAccountTypePM;
        }

        public GLAccountTypeList GetSingleGLAccountTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTypeListQueryService listService = new GLAccountTypeListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<GLAccountTypeList> GetGLAccountTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTypeListQueryService listService = new GLAccountTypeListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<GLAccountTypeList> GetGLAccountTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTypeListQueryService listService = new GLAccountTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetGLAccountTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountTypeListQueryService queryService = new GLAccountTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);//, tenant);

        }

        public void UpdateGLAccountTypeList(GLAccountTypeList entity)
        {

        }


    }
}