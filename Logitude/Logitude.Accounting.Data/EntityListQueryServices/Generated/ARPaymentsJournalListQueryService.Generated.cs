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

    public partial class ARPaymentsJournalListQueryService
    {
         private IAccountingContext context;
        public ARPaymentsJournalListQueryService(IAccountingContext context)
        {
            this.context = context;
        }

        public List<ARPaymentsJournalList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARPaymentsJournal> iQueryable = (from a in context.ARPaymentsJournals
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARPaymentsJournal>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ARPaymentsJournalList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ARPaymentsJournalList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ARPaymentsJournalList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ARPaymentsJournalObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ARPaymentsJournal",tenant).ToList();

                ObjectField objectField = (from a in ARPaymentsJournalObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ARPaymentsJournalList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentsJournalList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentsJournalList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentsJournalList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentsJournalList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentsJournalList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentsJournalList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.PaymentId);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.PaymentId);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<ARPaymentsJournalList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ARPaymentsJournalList GetSingle(int tenant, string paymentid, bool isvoided)
        {
            IQueryable<ARPaymentsJournal> ARPaymentsJournalQuery = (from a in context.ARPaymentsJournals
                                                       where a.Tenant == tenant && a.PaymentId == paymentid && a.IsVoided == isvoided
                                                       select a);

             
            IQueryable<ARPaymentsJournalList> ARPaymentsJournalListQuery = GetIqueryableList( ARPaymentsJournalQuery);
            ARPaymentsJournalList ARPaymentsJournalList = ARPaymentsJournalListQuery.FirstOrDefault();
            return ARPaymentsJournalList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARPaymentsJournal> iQueryable = (from a in context.ARPaymentsJournals 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<ARPaymentsJournal>(nonListQueryOperation, iQueryable);

            IQueryable<ARPaymentsJournalList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ARPaymentsJournalList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 