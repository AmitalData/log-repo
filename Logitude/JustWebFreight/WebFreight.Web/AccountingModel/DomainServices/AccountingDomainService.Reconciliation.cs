using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using WebFreight.Web.Security;

namespace WebFreight.Web.AccountingModel.DomainServices
{
    public partial class AccountingDomainService
    {
        public ReconciliationPM GetSingleReconciliationPM(string Id, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            reconciliationQuery = new ReconciliationQueryService(accountingContext);
            ReconciliationPM reconciliation = reconciliationQuery.GetSingle(Id, true, false);
            return reconciliation;

        }

        public ReconciliationList GetSingleReconciliationList(string Id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);


            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            ReconciliationListQueryService listService = new ReconciliationListQueryService(accountingContext);
            return listService.GetSingle(Id);
        }

        public List<ReconciliationList> GetReconciliationLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ReconciliationListQueryService listService = new ReconciliationListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<ReconciliationList> GetReconciliationFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ReconciliationListQueryService listService = new ReconciliationListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetReconciliationFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ReconciliationListQueryService queryService = new ReconciliationListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertReconciliation(ReconciliationPM entityPm)
        {
            SecurityUtility.CheckContactFeature("Reconciliation", "NEW", entityPm.Tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(entityPm.Tenant);
            }

            ReconciliationUpdateService service = new ReconciliationUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            service.Update(entityPm, true);

        }

        public void UpdateReconciliation(ReconciliationPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("Reconciliation", "UPDATE", currententityPm.Tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(currententityPm.Tenant);
            }

            ReconciliationUpdateService service = new ReconciliationUpdateService(accountingContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetReconciliationLinesChangeSet(currententityPm);
            service.Update(currententityPm, true);

        }

        [Invoke]
        public List<ReconciliationList> OpenReconciliationsByAccount(string gLAccountId, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            ReconciliationListQueryService listService = new ReconciliationListQueryService(accountingContext);
            var list = listService.GetOpenByAccountId(gLAccountId, tenant);
            return list;
        }

        [Invoke]
        public List<ReconciliationList> ClosedReconciliationsByAccountIdAndDate(string gLAccountId, DateTime? fromDate, DateTime? toDate, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            ReconciliationListQueryService listService = new ReconciliationListQueryService(accountingContext);
            //ReconciliationQueryService listService = new ReconciliationQueryService(accountingContext);
            var list = listService.GetClosedByAccountIdAndDate(gLAccountId, fromDate, toDate, tenant);
            return list;
        }

        [Invoke]
        public ReconciliationPM GetReconciliationPM(string id, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            ReconciliationQueryService qs = new ReconciliationQueryService(accountingContext);
            ReconciliationPM entityPM = qs.GetSingle(id, true, false);
            return entityPM;
        }

        [Invoke]
        public List<ReconciliationLinePM> GetReconciliationLinePMs(string id, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            ReconciliationLineQueryService qs = new ReconciliationLineQueryService(accountingContext);
            ReconciliationKeys reconciliationKeys = new ReconciliationKeys { Id = id };
            List<ReconciliationLinePM> entityPMs = qs.GetMulti(reconciliationKeys, false);
            return entityPMs;
        }

        public void DeleteReconciliation(ReconciliationPM entityPM)
        {
            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(entityPM.Tenant);
            }
            ReconciliationUpdateService service = new ReconciliationUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            ReconciliationQueryService qs = new ReconciliationQueryService(accountingContext);
            ReconciliationPM  delEntityPM = qs.GetSingle(entityPM.Id, true, false);
            foreach (ReconciliationLinePM line in delEntityPM.ReconciliationLines)
            {
                ReconciliationLinePM deletedLine = new ReconciliationLinePM()
                {
                    ChangeSetOp = ChangeSetOperation.Delete,
                    ReconciliationId = line.ReconciliationId,
                    Tenant = line.Tenant,
                    CurrencyCode = line.CurrencyCode,
                    CurrencyId = line.CurrencyId,
                    CurrencyName = line.CurrencyName,
                    IsPartial = line.IsPartial,
                    Line = line.Line,
                    ReconciliationAmount = line.ReconciliationAmount,
                    TransactionId = line.TransactionId,
                };
                delEntityPM.DeletedReconciliationLines.Add(deletedLine);
            }
           delEntityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
           service.Update(delEntityPM, true);
        }


        private void SetReconciliationLinesChangeSet(ReconciliationPM currententityPm)
        {
            List<ReconciliationLinePM> reconciliationLineChangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.ReconciliationLines).Cast<ReconciliationLinePM>().ToList();
            foreach (ReconciliationLinePM itemPM in reconciliationLineChangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ReconciliationLinePM currentItemPM = currententityPm.ReconciliationLines.Where(d => d.ReconciliationId == itemPM.ReconciliationId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            ReconciliationLinePM currentItemPM = currententityPm.ReconciliationLines.Where(d => d.ReconciliationId == itemPM.ReconciliationId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            ReconciliationLinePM currentItemPM = new ReconciliationLinePM() { ChangeSetOp = ChangeSetOperation.Delete, ReconciliationId = itemPM.ReconciliationId, Line = itemPM.Line, Tenant = currententityPm.Tenant };
                            currententityPm.DeletedReconciliationLines.Add(currentItemPM);
                            break;
                        }
                    default:
                        {
                            ReconciliationLinePM currentItemPM = currententityPm.ReconciliationLines.Where(d => d.ReconciliationId == itemPM.ReconciliationId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        [Invoke]
        public List<ReconciliationList> GetRecoByLTSearch(string gLAccountId, string searchString, int maxHits, int tenant) 
        {
            accountingContext = AccountingContext.GetContext(tenant);
            ReconciliationListQueryService qs = new ReconciliationListQueryService(accountingContext);
            // ReconciliationKeys reconciliationKeys = new ReconciliationKeys { Id = id };
            List<ReconciliationList> entityLists = qs.GetRecoByLTSearch(gLAccountId, searchString, maxHits, tenant); 
            return entityLists;
        }

        public void UpdateReconciliationList(ReconciliationList entity)
        {

        }

    }
}