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

using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.EntityLists;

namespace Logitude.DashboardModule.Data.EntityListQueryServices
{ 

    public partial class DashboardGlobalFilterListQueryService
    {
         private IDashboardContext context;
        public DashboardGlobalFilterListQueryService(IDashboardContext context)
        {
            this.context = context;
        }

        public List<DashboardGlobalFilterList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<DashboardGlobalFilterList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DashboardGlobalFilter> iQueryable = (from a in context.DashboardGlobalFilters
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<DashboardGlobalFilter>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<DashboardGlobalFilterList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<DashboardGlobalFilterList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<DashboardGlobalFilterList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DashboardGlobalFilterList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> DashboardGlobalFilterObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("DashboardGlobalFilter",tenant).ToList();

                ObjectField objectField = (from a in DashboardGlobalFilterObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<DashboardGlobalFilterList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<DashboardGlobalFilterList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DashboardGlobalFilterList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DashboardGlobalFilterList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DashboardGlobalFilterList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DashboardGlobalFilterList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<DashboardGlobalFilterList, decimal>(queryOperations, query2);
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

         public List<DashboardGlobalFilterList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public DashboardGlobalFilterList GetSingle(string id)
        {
            IQueryable<DashboardGlobalFilter> DashboardGlobalFilterQuery = (from a in context.DashboardGlobalFilters
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<DashboardGlobalFilterList> DashboardGlobalFilterListQuery = GetIqueryableList( DashboardGlobalFilterQuery);
            DashboardGlobalFilterList DashboardGlobalFilterList = DashboardGlobalFilterListQuery.FirstOrDefault();
            return DashboardGlobalFilterList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DashboardGlobalFilter> iQueryable = (from a in context.DashboardGlobalFilters 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<DashboardGlobalFilter>(nonListQueryOperation, iQueryable);



            IQueryable<DashboardGlobalFilterList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<DashboardGlobalFilterList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<DashboardGlobalFilterList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 