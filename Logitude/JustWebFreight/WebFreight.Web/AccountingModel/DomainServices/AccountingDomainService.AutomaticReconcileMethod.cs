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

        public AutomaticReconcileMethodPM GetSingleAutomaticReconcileMethodPM(string id, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            automaticReconcileMethodQuery = new AutomaticReconcileMethodQueryService(accountingContext);
            AutomaticReconcileMethodPM AutomaticReconcileMethod = automaticReconcileMethodQuery.GetSingle(id, true, false);
            return AutomaticReconcileMethod;
        }

        [Invoke]
        public AutomaticReconcileMethodPM GetAutomaticReconcileMethodPM(string id, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            AutomaticReconcileMethodQueryService qs = new AutomaticReconcileMethodQueryService(accountingContext);
            AutomaticReconcileMethodPM entityPM = qs.GetSingle(id, false, true);
            return entityPM;
        }

        public AutomaticReconcileMethodList GetAutomaticReconcileMethod(string id, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            AutomaticReconcileMethodListQueryService qs = new AutomaticReconcileMethodListQueryService(accountingContext);
            AutomaticReconcileMethodList list = qs.GetById(id, tenant);
            return list;
        }

        [Invoke]
        public bool CheckWhetherAutomaticReconcileMethodCodeExists(string code, string id, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            automaticReconcileMethodQuery = new AutomaticReconcileMethodQueryService(accountingContext);
            return this.automaticReconcileMethodQuery.CheckWhetherCodeExists(code, id, tenant);
        }


        public AutomaticReconcileMethodList GetSingleAutomaticReconcileMethodList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.AutomaticReconcileMethod", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            AutomaticReconcileMethodListQueryService listService = new AutomaticReconcileMethodListQueryService(accountingContext);
            return listService.GetSingle(id);
        }

        public List<AutomaticReconcileMethodList> GetAutomaticReconcileMethodLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.AutomaticReconcileMethod", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            AutomaticReconcileMethodListQueryService listService = new AutomaticReconcileMethodListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<AutomaticReconcileMethodList> GetAutomaticReconcileMethodFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.AutomaticReconcileMethod", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            AutomaticReconcileMethodListQueryService listService = new AutomaticReconcileMethodListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetAutomaticReconcileMethodFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.AutomaticReconcileMethod", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            AutomaticReconcileMethodListQueryService queryService = new AutomaticReconcileMethodListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }



        public void InsertAutomaticReconcileMethod(AutomaticReconcileMethodPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("AutomaticReconcileMethod", "NEW", entityPm.Tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(entityPm.Tenant);
            }


            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            AutomaticReconcileMethodUpdateService service = new AutomaticReconcileMethodUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPm.Tenant);

            service.Update(entityPm, true);
        }

        public void UpdateAutomaticReconcileMethod(AutomaticReconcileMethodPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("AutomaticReconcileMethod", "UPDATE", currententityPm.Tenant);
            var sssss = this.ChangeSet.ChangeSetEntries;
            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(currententityPm.Tenant);
            }
            //currententityPm.MarkAsChanged = true;

            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            AutomaticReconcileMethodUpdateService service = new AutomaticReconcileMethodUpdateService(accountingContext, new Dictionary<string, IContext>(), currententityPm.Tenant);

            service.Update(currententityPm, true);

        }


        public void UpdateAutomaticReconcileMethodList(AutomaticReconcileMethodList entity)
        {

        }




    }
}