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

        public JournalStatusTypePM GetSingleJournalStatusTypePM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            journalStatusTypeQuery = new JournalStatusTypeQueryService(accountingContext);
            JournalStatusTypePM JournalStatusType = journalStatusTypeQuery.GetSingle(code, true, false);
            return JournalStatusType;
        }

        public JournalStatusTypeList GetSingleJournalStatusTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            JournalStatusTypeListQueryService listService = new JournalStatusTypeListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<JournalStatusTypeList> GetJournalStatusTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            JournalStatusTypeListQueryService listService = new JournalStatusTypeListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<JournalStatusTypeList> GetJournalStatusTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            JournalStatusTypeListQueryService listService = new JournalStatusTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetJournalStatusTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            JournalStatusTypeListQueryService queryService = new JournalStatusTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);//, tenant);

        }

        public void UpdateJournalStatusTypeList(JournalStatusTypeList entity)
        {

        }

  




        //public void InsertJournalStatusType(JournalStatusTypePM entityPm)
        //{
        //    //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "NEW", entityPm.Tenant);

        //    if (accountingContext == null)
        //    {
        //        accountingContext = AccountingContext.GetContext(0);//entityPm.Tenant);
        //    }


        //    entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

        //    JournalStatusTypeUpdateService service = new JournalStatusTypeUpdateService(accountingContext, new Dictionary<string, IContext>(), 0);//, entityPm.Tenant);

        //    service.Update(entityPm, true);
        //}

        //public void UpdateJournalStatusType(JournalStatusTypePM currententityPm)
        //{
        //    //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "UPDATE", currententityPm.Tenant);
        //    var sssss = this.ChangeSet.ChangeSetEntries;
        //    if (accountingContext == null)
        //    {
        //        accountingContext = AccountingContext.GetContext(0);//currententityPm.Tenant);
        //    }
        //    //currententityPm.MarkAsChanged = true;

        //    currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
        //    JournalStatusTypeUpdateService service = new JournalStatusTypeUpdateService(accountingContext, new Dictionary<string, IContext>(),0);//, currententityPm.Tenant);

        //    service.Update(currententityPm, true);

        //}





    }
}