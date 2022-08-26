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

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class ActivityOwnerHistoryListQueryService
    {
         private ICRMContext context;
        public ActivityOwnerHistoryListQueryService(ICRMContext context)
        {
            this.context = context;
        }

        public List<ActivityOwnerHistoryList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<ActivityOwnerHistoryList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ActivityOwnerHistory> iQueryable = (from a in context.ActivityOwnerHistories
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<ActivityOwnerHistory>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ActivityOwnerHistoryList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ActivityOwnerHistoryList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<ActivityOwnerHistoryList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ActivityOwnerHistoryList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ActivityOwnerHistoryObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ActivityOwnerHistory",tenant).ToList();

                ObjectField objectField = (from a in ActivityOwnerHistoryObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ActivityOwnerHistoryList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ActivityOwnerHistoryList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ActivityOwnerHistoryList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ActivityOwnerHistoryList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ActivityOwnerHistoryList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ActivityOwnerHistoryList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ActivityOwnerHistoryList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Id);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.Id);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<ActivityOwnerHistoryList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ActivityOwnerHistoryList GetSingle(string id)
        {
            IQueryable<ActivityOwnerHistory> ActivityOwnerHistoryQuery = (from a in context.ActivityOwnerHistories
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<ActivityOwnerHistoryList> ActivityOwnerHistoryListQuery = GetIqueryableList( ActivityOwnerHistoryQuery);
            ActivityOwnerHistoryList ActivityOwnerHistoryList = ActivityOwnerHistoryListQuery.FirstOrDefault();
            return ActivityOwnerHistoryList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ActivityOwnerHistory> iQueryable = (from a in context.ActivityOwnerHistories 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<ActivityOwnerHistory>(nonListQueryOperation, iQueryable);



            IQueryable<ActivityOwnerHistoryList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ActivityOwnerHistoryList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<ActivityOwnerHistoryList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 