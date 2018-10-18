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

        public AutomaticReconcilePM GetSingleAutomaticReconcilePM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            automaticReconcileQuery = new AutomaticReconcileQueryService(accountingContext);
            AutomaticReconcilePM AutomaticReconcile = automaticReconcileQuery.GetSingle(code, true, false);
            return AutomaticReconcile;
        }

        public AutomaticReconcileList GetSingleAutomaticReconcileList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.AutomaticReconcile", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            AutomaticReconcileListQueryService listService = new AutomaticReconcileListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<AutomaticReconcileList> GetAutomaticReconcileLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.AutomaticReconcile", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            AutomaticReconcileListQueryService listService = new AutomaticReconcileListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<AutomaticReconcileList> GetAutomaticReconcileFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.AutomaticReconcile", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            AutomaticReconcileListQueryService listService = new AutomaticReconcileListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetAutomaticReconcileFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.AutomaticReconcile", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            AutomaticReconcileListQueryService queryService = new AutomaticReconcileListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);//, tenant);

        }

        public void UpdateAutomaticReconcileList(AutomaticReconcileList entity)
        {

        }


        //public void InsertAutomaticReconcile(AutomaticReconcilePM entityPm)
        //{
        //    //SecurityUtility.CheckContactFeature("Accounting.AutomaticReconcile", "NEW", entityPm.Tenant);

        //    if (accountingContext == null)
        //    {
        //        accountingContext = AccountingContext.GetContext(0);//entityPm.Tenant);
        //    }


        //    entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

        //    AutomaticReconcileUpdateService service = new AutomaticReconcileUpdateService(accountingContext, new Dictionary<string, IContext>(), 0);//, entityPm.Tenant);

        //    service.Update(entityPm, true);
        //}

        //public void UpdateAutomaticReconcile(AutomaticReconcilePM currententityPm)
        //{
        //    //SecurityUtility.CheckContactFeature("Accounting.AutomaticReconcile", "UPDATE", currententityPm.Tenant);
        //    var sssss = this.ChangeSet.ChangeSetEntries;
        //    if (accountingContext == null)
        //    {
        //        accountingContext = AccountingContext.GetContext(0);//currententityPm.Tenant);
        //    }
        //    //currententityPm.MarkAsChanged = true;

        //    currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
        //    AutomaticReconcileUpdateService service = new AutomaticReconcileUpdateService(accountingContext, new Dictionary<string, IContext>(),0);//, currententityPm.Tenant);

        //    service.Update(currententityPm, true);

        //}





    }
}