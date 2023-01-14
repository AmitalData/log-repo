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

    public partial class DashboardGlobalPresetFilterListQueryService
    {
         private IDashboardContext context;
        public DashboardGlobalPresetFilterListQueryService(IDashboardContext context)
        {
            this.context = context;
        }

        public List<DashboardGlobalPresetFilterList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<DashboardGlobalPresetFilterList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DashboardGlobalPresetFilter> iQueryable = (from a in context.DashboardGlobalPresetFilters
                                               select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<DashboardGlobalPresetFilter>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<DashboardGlobalPresetFilterList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<DashboardGlobalPresetFilterList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<DashboardGlobalPresetFilterList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DashboardGlobalPresetFilterList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> DashboardGlobalPresetFilterObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("DashboardGlobalPresetFilter",tenant).ToList();

                ObjectField objectField = (from a in DashboardGlobalPresetFilterObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<DashboardGlobalPresetFilterList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<DashboardGlobalPresetFilterList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DashboardGlobalPresetFilterList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DashboardGlobalPresetFilterList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DashboardGlobalPresetFilterList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DashboardGlobalPresetFilterList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<DashboardGlobalPresetFilterList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Sort);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.Sort);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<DashboardGlobalPresetFilterList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public DashboardGlobalPresetFilterList GetSingle(string code)
        {
            IQueryable<DashboardGlobalPresetFilter> DashboardGlobalPresetFilterQuery = (from a in context.DashboardGlobalPresetFilters
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<DashboardGlobalPresetFilterList> DashboardGlobalPresetFilterListQuery = GetIqueryableList( DashboardGlobalPresetFilterQuery);
            DashboardGlobalPresetFilterList DashboardGlobalPresetFilterList = DashboardGlobalPresetFilterListQuery.FirstOrDefault();
            return DashboardGlobalPresetFilterList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations ){
		 		  return GetListCount(queryOperations, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DashboardGlobalPresetFilter> iQueryable = (from a in context.DashboardGlobalPresetFilters  select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<DashboardGlobalPresetFilter>(nonListQueryOperation, iQueryable);



            IQueryable<DashboardGlobalPresetFilterList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<DashboardGlobalPresetFilterList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<DashboardGlobalPresetFilterList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 