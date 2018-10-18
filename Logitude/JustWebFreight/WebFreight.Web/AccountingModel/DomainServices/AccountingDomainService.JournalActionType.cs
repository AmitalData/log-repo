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

        public JournalActionTypePM GetSingleJournalActionTypePM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            journalActionTypeQuery = new JournalActionTypeQueryService(accountingContext);
            JournalActionTypePM JournalActionType = journalActionTypeQuery.GetSingle(code, true, false);
            return JournalActionType;
        }

        [Invoke]
        public bool CheckWhetherJournalActionTypeCodeExists(string code, string id, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            journalActionTypeQuery = new JournalActionTypeQueryService(accountingContext);
            return this.journalActionTypeQuery.CheckWhetherCodeExists(code, id, tenant);
        }


        public JournalActionTypeList GetSingleJournalActionTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("JournalActionType", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            JournalActionTypeListQueryService listService = new JournalActionTypeListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<JournalActionTypeList> GetJournalActionTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("JournalActionType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            JournalActionTypeListQueryService listService = new JournalActionTypeListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<JournalActionTypeList> GetJournalActionTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ///SecurityUtility.CheckContactFeature("JournalActionType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            JournalActionTypeListQueryService listService = new JournalActionTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetJournalActionTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("JournalActionType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            JournalActionTypeListQueryService queryService = new JournalActionTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }



        public void InsertJournalActionType(JournalActionTypePM entityPm)
        {
            SecurityUtility.CheckContactFeature("JournalActionType", "NEW", entityPm.Tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(entityPm.Tenant);
            }


            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            JournalActionTypeUpdateService service = new JournalActionTypeUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPm.Tenant);

            service.Update(entityPm, true);
        }

        public void UpdateJournalActionType(JournalActionTypePM currententityPm)
        {
            SecurityUtility.CheckContactFeature("JournalActionType", "UPDATE", currententityPm.Tenant);
            var sssss = this.ChangeSet.ChangeSetEntries;
            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(currententityPm.Tenant);
            }
            //currententityPm.MarkAsChanged = true;

            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            JournalActionTypeUpdateService service = new JournalActionTypeUpdateService(accountingContext, new Dictionary<string, IContext>(), currententityPm.Tenant);

            service.Update(currententityPm, true);

        }

  
        public void UpdateJournalActionTypeList(JournalActionTypeList entity)
        {

        }

  



    }
}