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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class ReconcileExternalPageListQueryService
    {
	    private IQueryable<ReconcileExternalPageList> GetIqueryableList(IQueryable<ReconcileExternalPage> iQueryable)
        {
		IQueryable<ReconcileExternalPageList> query = (from a in iQueryable.Include("BankPageEntryType")
                                            select new ReconcileExternalPageList()
											{
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                
                                                BankAccountId = a.BankAccountId,
					
					                            GLAccountId = a.GLAccountId,
					
					                            PageNo = a.PageNo,
					
					                            SearchFields = a.SearchFields,
					
					                            FromDate = a.FromDate,
					
					                            ToDate = a.ToDate,

                                                StartBalance = a.StartBalance,

                                                CloseBalance = a.CloseBalance,
					
					                            CreateDate = a.CreateDate,
					
					                            //IsApproved = a.IsApproved,
					
					                            ApprovedByUserId = a.ApprovedByUserId,
                                                CreatedByUserId = a.CreatedByUserId,

                                                CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.LocalName : null,

                                                StatusCode = a.StatusCode,
                                                StatusName = a.ReconcileExternalPageStatus.EnglishName,
                                                StatusLocalName = a.ReconcileExternalPageStatus.LocalName,
                                                EntryTypeCode = a.EntryTypeCode,
                                                EntryTypeEnglishName = a.BankPageEntryType.EnglishName,
                                                EntryTypeLocalName = a.BankPageEntryType.LocalName,


                                            });
            return query;
		}

		private IQueryable<ReconcileExternalPage> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ReconcileExternalPage> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<ReconcileExternalPage> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ReconcileExternalPage> iQueryable, int tenant)
        {
			return iQueryable;
		}

        public List<ReconcileExternalPageLineList> getExternalReoncilioationsByFilter(QueryOperations queryOperations, string bankAccountId, int tenant)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            ReconcileExternalPageLineListQueryService lineQuery = new ReconcileExternalPageLineListQueryService(accountingContext);

            IQueryable<ReconcileExternalPageLine> accountQuery = GetExternalReconciliationByBankAccount(bankAccountId, tenant);

            // Get list query with query filters 
            IQueryable<ReconcileExternalPageLineList>  listQuery = BasicListFilter(accountQuery, queryOperations, tenant);

            // Ordering
            listQuery = listQuery.OrderByDescending(a => a.ReferenceDate);

            // Skip & Take
            var skipped = (queryOperations.PageIndex - 1);
            listQuery = listQuery
                .Skip(skipped)
                .Take(queryOperations.PageSize);

            List<ReconcileExternalPageLineList> mylist = listQuery.ToList();
            return mylist;


        }

        public GenericCallBack GetOpenReconciliationFilterCallBack(QueryOperations queryOperations, string bankAccountId, int tenant)
        {

            IQueryable<ReconcileExternalPageLine> accountQuery = GetExternalReconciliationByBankAccount(bankAccountId, tenant);

            IQueryable<ReconcileExternalPageLineList> query2 = BasicListFilter(accountQuery, queryOperations, tenant);
            var callback11 =
                (from r in query2
                 group r by 1 into gb
                 select new
                 {
                     TotalRecord = gb.Count()
                 })
             .FirstOrDefault() ?? new
             {
                 TotalRecord = 0,
             }
             ;
            var myGenericCallBack = new GenericCallBack();
            myGenericCallBack.MaxFieldName = "CreateDate";
            myGenericCallBack.TotalRecord = callback11.TotalRecord;

            return myGenericCallBack;
        }
        private IQueryable<ReconcileExternalPageLine> GetExternalReconciliationByBankAccount(string bankAccountId, int tenant)
        {

            IQueryable<ReconcileExternalPageLine> accountQuery = (from page in context.ReconcileExternalPages
                                                                  join line in context.ReconcileExternalPageLines on page.Id equals line.ReconcileExternalPageId
                                                                  where page.Tenant == tenant
                                                                        && page.BankAccountId == bankAccountId
                                                                        && page.StatusCode == "2"  // 2- Approved
                                                                        && line.IsReconciled == false
                                                                  select line);
            return accountQuery;
        }
        private IQueryable<ReconcileExternalPageLineList> BasicListFilter(IQueryable<ReconcileExternalPageLine> iQueryable, QueryOperations queryOperations, int tenant)
        {

            var accountingContext = AccountingContext.GetContext(tenant);
            ReconcileExternalPageLineListQueryService lineQuery = new ReconcileExternalPageLineListQueryService(accountingContext);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            iQueryable = lineQuery.ApplyBusinessUnitFilters(queryOperations, iQueryable, tenant);
            iQueryable = lineQuery.ApplyCustomFilters(queryOperations, iQueryable, tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ReconcileExternalPageLine>(nonListQueryOperation, iQueryable);

            IQueryable<ReconcileExternalPageLineList> query2 = lineQuery.GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ReconcileExternalPageLineList>(listQueryOperation, query2);
            return query2;
        }



        //
        // External Reconcile
        //
        public IQueryable<ReconcileExternalPageLineList> getPageLinesByFilter(QueryOperations queryOperations, string bankAccountId, int tenant)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            ReconcileExternalPageLineListQueryService lineQuery = new ReconcileExternalPageLineListQueryService(accountingContext);

            IQueryable<ReconcileExternalPageLine> accountQuery = GetExternalReconciliationByBankAccount(bankAccountId, tenant);

            // Get list query with query filters 
            IQueryable<ReconcileExternalPageLineList> listQuery = BasicListFilter(accountQuery, queryOperations, tenant);
            
            return listQuery;

        }

    }


}
	