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

    public partial class InterestTransactionListQueryService
    {
         private IAccountingContext context;
        public InterestTransactionListQueryService(IAccountingContext context)
        {
            this.context = context;
        }
        // generate disabled
        public List<InterestTransactionList> GetList(QueryOperations queryOperations, int tenant)
        {
            List<QueryFilterItem> dateFilters = GetDateFiltersFromQueryOperations(queryOperations);
            FixDateEdges(dateFilters);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<InterestTransaction> iQueryable = (from a in context.InterestTransactions

                                                          where a.Tenant == tenant
                                                          select a);
            iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable, tenant);
            iQueryable = ApplyCustomFilters(queryOperations, iQueryable, tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<InterestTransaction>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<InterestTransactionList> query2 = GetIqueryableList(iQueryable, tenant);

            query2 = filter.GetFilteredQuery<InterestTransactionList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(InterestTransactionList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> InterestTransactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("InterestTransaction", tenant).ToList();

                ObjectField objectField = (from a in InterestTransactionObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<InterestTransactionList, string>(queryOperations, query2);
                    }
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                                {
                                    query2 = sortClass.GetSorterQuery<InterestTransactionList, string>(queryOperations, query2);
                                    break;
                                }
                            case "sigdouble":
                            case "double":
                                {
                                    query2 = sortClass.GetSorterQuery<InterestTransactionList, double>(queryOperations, query2);
                                    break;
                                }
                            case "date":
                            case "datetime":
                                {
                                    query2 = sortClass.GetSorterQuery<InterestTransactionList, DateTime>(queryOperations, query2);
                                    break;
                                }
                            case "unsinteger":
                            case "integer":
                                {
                                    query2 = sortClass.GetSorterQuery<InterestTransactionList, int>(queryOperations, query2);
                                    break;
                                }
                            case "boolean":
                                {
                                    query2 = sortClass.GetSorterQuery<InterestTransactionList, bool>(queryOperations, query2);
                                    break;
                                }
                            case "unsdecimal":
                            case "decimal":
                                {
                                    query2 = sortClass.GetSorterQuery<InterestTransactionList, decimal>(queryOperations, query2);
                                    break;
                                }
                            default:
                                {
                                    query2 = query2.OrderByDescending(d => d.CreateDateTime);
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.CreateDateTime);
            }
            if (!queryOperations.GetAll)
            {
                query2 = query2.Skip(skippedPorts);
                query2 = query2.Take(queryOperations.PageSize);
            }
            return query2.ToList();


        }

        private void FixDateEdges(List<QueryFilterItem> dateFilters)
        {
            foreach (var dateFilter in dateFilters)
            {
                var fromDate = Convert.ToDateTime(dateFilter.FieldValue);
                var toDate = Convert.ToDateTime(dateFilter.FieldValue2);

                dateFilter.FieldValue = GetStartOfDate(fromDate);
                dateFilter.FieldValue2 = GetEndOfDate(toDate);
            }
        }

        private static List<QueryFilterItem> GetDateFiltersFromQueryOperations(QueryOperations queryOperations)
        {
            var dateFiltersNames = new List<string> { "AccountingDate", "InterestValueDate", "CreateDateTime" };
            var dateFilters = queryOperations.QueryFilterItems.Where(f => dateFiltersNames.Contains(f.FieldName)).ToList();
            return dateFilters;
        }

        public List<InterestTransactionList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public InterestTransactionList GetSingle(string id)
        {
            IQueryable<InterestTransaction> InterestTransactionQuery = (from a in context.InterestTransactions
                                                       where a.Id == id
                                                       select a);

            var transaction = InterestTransactionQuery.FirstOrDefault();

            IQueryable<InterestTransactionList> InterestTransactionListQuery = GetIqueryableList(InterestTransactionQuery, transaction.Tenant);
            InterestTransactionList InterestTransactionList = InterestTransactionListQuery.FirstOrDefault();
            return InterestTransactionList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<InterestTransaction> iQueryable = (from a in context.InterestTransactions 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<InterestTransaction>(nonListQueryOperation, iQueryable);

            IQueryable<InterestTransactionList> query2 = GetIqueryableList(iQueryable, tenant);

            query2 = filter.GetFilteredQuery<InterestTransactionList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public DateTime GetEndOfDate(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 59);
            return date;
        }
        public DateTime GetStartOfDate(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
            return date;
        }

    }
}
	 