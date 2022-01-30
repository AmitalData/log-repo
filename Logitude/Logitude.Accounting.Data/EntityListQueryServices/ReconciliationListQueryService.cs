using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.Generic;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools.Helpers;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class ReconciliationListQueryService
    {
	    private IQueryable<ReconciliationList> GetIqueryableList(IQueryable<Reconciliation> iQueryable)
        {
            IQueryable<ReconciliationList> query = (from a in iQueryable
                                                    select new ReconciliationList()
                                             {
                                                 Id = a.Id,
                                                 AccountId = a.AccountId,
                                                 CreatedByUserId = a.CreatedByUserId,
                                                 Number = a.Number,
                                                 CreateDate = a.CreateDate,
                                                 Tenant = a.Tenant,
                                                 SearchFields = a.SearchFields,
                                                 CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                 AccountNumber = a.Account != null ? a.Account.DisplayNumber : null,
                                                 AccountName = a.Account != null ? a.Account.EnglishName : null,
                                                 IsCancelled = a.IsCancelled,
                                             });

            return query;
        }

		private IQueryable<Reconciliation> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Reconciliation> iQueryable,int tenant)
        {
            var reconciliationLinesQueryOperations = GetReconciliationLinesQueryOperations(queryOperations);
            if (reconciliationLinesQueryOperations == null)
                return iQueryable;
            GenericFilter filter = new GenericFilter();
            var reconciliationLinesQuery = GetIQueryableReconciliation(tenant);
            reconciliationLinesQuery = filter.GetFilteredQuery(reconciliationLinesQueryOperations, reconciliationLinesQuery);
            iQueryable = iQueryable.Where(w => reconciliationLinesQuery.Where(e=>e.ReconciliationId == w.Id).Any());
            
            return iQueryable;
		}

        private IQueryable<ReconciliationLineList> GetIQueryableReconciliation(int tenant)
        {
            return context.ReconciliationLines.Include(e=>e.LedgerTransaction.Account).Select(a=>
            new ReconciliationLineList()
            {
                ReconciliationId = a.ReconciliationId,
                TransactionId = a.TransactionId,
                TransactionAmount =Math.Abs( 
                (a.LedgerTransaction != null ? 
                    a.LedgerTransaction.Account.ReconcileMethodCode == "0" ? a.LedgerTransaction.LocalAmountCredit : a.LedgerTransaction.ForeignAmountCredit
                    : 0
                ) == 0 ?
                    -1 * (a.LedgerTransaction != null ? a.LedgerTransaction.Account.ReconcileMethodCode == "0" ? a.LedgerTransaction.LocalAmountDebit : a.LedgerTransaction.ForeignAmountDebit : 0)
                    : (a.LedgerTransaction != null ? a.LedgerTransaction.Account.ReconcileMethodCode == "0" ? a.LedgerTransaction.LocalAmountCredit : a.LedgerTransaction.ForeignAmountCredit : 0)
                    )

                

            }
            );
        }

        private QueryOperations GetReconciliationLinesQueryOperations(QueryOperations queryOperations)
        {
            var filterFieldName = "TransactionAmount";
            var reconciliationAmountFieldOperations = queryOperations.QueryFilterItems.Where(e => e.FieldName == filterFieldName).FirstOrDefault();
            if (reconciliationAmountFieldOperations == null)
                return null;
            var reconciliationLinesQueryOperations = CreateReconciliationLinesQueryOperations(reconciliationAmountFieldOperations);
            return reconciliationLinesQueryOperations;
        }

        private QueryOperations CreateReconciliationLinesQueryOperations(QueryFilterItem reconciliationAmountFieldOperations)
        {
            return new QueryOperations()
            {
                GetAll = true,
                QueryFilterItems = new List<QueryFilterItem>()
                {
                    new QueryFilterItem()
                    {
                        FieldName = reconciliationAmountFieldOperations.FieldName,
                        FieldValue = decimal.Parse(reconciliationAmountFieldOperations.FieldValue.ToString()),
                        Operator = reconciliationAmountFieldOperations.Operator
                    }
                }
            };
        }

        private IQueryable<Reconciliation> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Reconciliation> iQueryable,int tenant)
        {
			return iQueryable;
		}


        public List<ReconciliationList> GetOpenByAccountId(string accountId, int tenant)
        {
            IQueryable<Reconciliation> reconciliationQuery = (from a in context.Reconciliations
                                                                    where a.Tenant == tenant && a.AccountId == accountId && String.IsNullOrEmpty(a.Number)
                                                                    select a);

            IQueryable<ReconciliationList> reconciliationListQuery = this.GetIqueryableList(reconciliationQuery);
            List<ReconciliationList> rvList = reconciliationListQuery.ToList();
            return rvList;
        }

        public List<ReconciliationList> GetClosedByAccountIdAndDate(string accountId, DateTime? fromDate, DateTime? toDate, int tenant)
        {
            DateTime? fromD = fromDate.HasValue ? fromDate.Value.Date : fromDate;
            DateTime? toD = toDate.HasValue ? toDate.Value.Date : toDate;

            IQueryable<Reconciliation> reconciliationQuery;
            if (fromDate.HasValue && !toDate.HasValue)
            {
                reconciliationQuery = (from a in context.Reconciliations
                                       where a.Tenant == tenant && a.AccountId == accountId && !String.IsNullOrEmpty(a.Number) && DbFunctions.TruncateTime(a.CreateDate) >= fromD
                                       select a).OrderByDescending(b => b.CreateDate);
            }
            else if (!fromDate.HasValue && toDate.HasValue)
            {
                reconciliationQuery = (from a in context.Reconciliations
                                       where a.Tenant == tenant && a.AccountId == accountId && !String.IsNullOrEmpty(a.Number) && DbFunctions.TruncateTime(a.CreateDate) <= toD
                                       select a).OrderByDescending(b => b.CreateDate);
            }
            else if (fromDate.HasValue && toDate.HasValue)
            {
                reconciliationQuery = (from a in context.Reconciliations
                                       where a.Tenant == tenant && a.AccountId == accountId && !String.IsNullOrEmpty(a.Number) && DbFunctions.TruncateTime(a.CreateDate) >= fromD && DbFunctions.TruncateTime(a.CreateDate) <= toD
                                       select a).OrderByDescending(b => b.CreateDate);
            }
            else
            {
                reconciliationQuery = (from a in context.Reconciliations
                                       where a.Tenant == tenant && a.AccountId == accountId && !String.IsNullOrEmpty(a.Number)
                                       select a).OrderByDescending(b => b.CreateDate);
            }
            IQueryable<ReconciliationList> reconciliationListQuery = this.GetIqueryableList(reconciliationQuery);
            List<ReconciliationList> rvList = reconciliationListQuery.ToList();
            return rvList;
        }


        public List<ReconciliationList> GetRecoByLTSearch(string gLAccountId, string searchString, int maxHits, int tenant) 
        {
            //const int maxResults = 100;
            //if (accountingContext == null)
            //{
            //    accountingContext = AccountingContext.GetContext(tenant);
            //}
            ReconciliationRepository reconciliationRepository = new ReconciliationRepository(context);
            ReconciliationLineRepository reconciliationLineRepository = new ReconciliationLineRepository(context);
            LedgerTransactionRepository ledgerTransactionRepository = new LedgerTransactionRepository(context);
            IQueryable<Reconciliation> reconciliations = reconciliationRepository.GetClosedReconciliationsByAccount(gLAccountId, tenant);
            IQueryable<ReconciliationLine> reconciliationLines = reconciliationLineRepository.GetReconciliationLines(tenant);
            IQueryable<LedgerTransaction> ledgerTransactions = ledgerTransactionRepository.GetLedgerTransactionsByAccount(gLAccountId, tenant);
            IQueryable<ReconciliationList> myQuery = (from reco in reconciliations
                                                      join recoLine in reconciliationLines on reco.Id equals recoLine.ReconciliationId //into recoRecoLineJoin
                                                      from recoLine1 in reconciliationLines
                                                      join lt in ledgerTransactions on recoLine1.TransactionId equals lt.Id
                                                      where (lt.SearchFields.Contains(searchString) || String.IsNullOrEmpty(searchString))
                                                      orderby reco.CreateDate
                                                      select new ReconciliationList()
                                                      {
                                                          Id = reco.Id,
                                                          AccountId = reco.AccountId,
                                                          CreateDate = reco.CreateDate,
                                                          CreatedByUserId = reco.CreatedByUserId,
                                                          CreatedByUserName = reco.CreatedByUser == null ? null : reco.CreatedByUser.Contact.EnglishName,
                                                          Number = reco.Number,
                                                          Tenant = reco.Tenant,
                                                          SearchFields = reco.SearchFields,
                                                          AccountName = reco.Account == null ? null : reco.Account.EnglishName,
                                                          AccountNumber = reco.Account == null ? null : reco.Account.DisplayNumber,
                                                      });
            List<ReconciliationList> tempList = myQuery.ToList<ReconciliationList>();

            HashSet<ReconciliationList> uniqHashSet = new HashSet<ReconciliationList>();
            foreach (ReconciliationList oneReco in tempList)
            {
                if (uniqHashSet.Where(d => d.Id == oneReco.Id).Any<ReconciliationList>() != true) uniqHashSet.Add(oneReco);
            }
            int count = uniqHashSet.Count<ReconciliationList>();
            List<ReconciliationList> recoList;
            if (count <= maxHits)
            {
                recoList = uniqHashSet.ToList<ReconciliationList>();
               // message = "";
            }
            else
            {
                recoList = uniqHashSet.Take<ReconciliationList>(maxHits).ToList<ReconciliationList>();
                // message = "Warning: only the first " + maxResults + " records are shown. Please refine your search to get less results";
                //message = TranslateTextsClass.Translate("Reconciliations.Q.OnlyFirstRecords", tenant) + maxResults + TranslateTextsClass.Translate("Reconciliations.Q.OnlyFirstRecords_Ending", tenant);
            }
            return recoList;
        }

       
    }


}
	