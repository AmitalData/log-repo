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

        public JournalTypePM GetSingleJournalTypePM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            journalTypeQuery = new JournalTypeQueryService(accountingContext);
            JournalTypePM JournalType = journalTypeQuery.GetSingle(code, true, false);
            return JournalType;
        }

        public JournalTypeList GetSingleJournalTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalType", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            JournalTypeListQueryService listService = new JournalTypeListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<JournalTypeList> GetJournalTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            JournalTypeListQueryService listService = new JournalTypeListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<JournalTypeList> GetJournalTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            JournalTypeListQueryService listService = new JournalTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetJournalTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            JournalTypeListQueryService queryService = new JournalTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);//, tenant);

        }

        public void UpdateJournalTypeList(JournalTypeList entity)
        {

        }


        //public void InsertJournalType(JournalTypePM entityPm)
        //{
        //    //SecurityUtility.CheckContactFeature("Accounting.JournalType", "NEW", entityPm.Tenant);

        //    if (accountingContext == null)
        //    {
        //        accountingContext = AccountingContext.GetContext(tenant);//entityPm.Tenant);
        //    }


        //    entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

        //    JournalTypeUpdateService service = new JournalTypeUpdateService(accountingContext, new Dictionary<string, IContext>(), 0);//, entityPm.Tenant);

        //    service.Update(entityPm, true);
        //}

        //public void UpdateJournalType(JournalTypePM currententityPm)
        //{
        //    //SecurityUtility.CheckContactFeature("Accounting.JournalType", "UPDATE", currententityPm.Tenant);
        //    var sssss = this.ChangeSet.ChangeSetEntries;
        //    if (accountingContext == null)
        //    {
        //        accountingContext = AccountingContext.GetContext(tenant);//currententityPm.Tenant);
        //    }
        //    //currententityPm.MarkAsChanged = true;

        //    currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
        //    JournalTypeUpdateService service = new JournalTypeUpdateService(accountingContext, new Dictionary<string, IContext>(),0);//, currententityPm.Tenant);

        //    service.Update(currententityPm, true);

        //}





    }
}