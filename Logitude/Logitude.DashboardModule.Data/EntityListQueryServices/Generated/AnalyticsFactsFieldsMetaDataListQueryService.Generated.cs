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

    public partial class AnalyticsFactsFieldsMetaDataListQueryService
    {
         private IDashboardContext context;
        public AnalyticsFactsFieldsMetaDataListQueryService(IDashboardContext context)
        {
            this.context = context;
        }

        public List<AnalyticsFactsFieldsMetaDataList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<AnalyticsFactsFieldsMetaDataList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AnalyticsFactsFieldsMetaData> iQueryable = (from a in context.AnalyticsFactsFieldsMetaDatas
                                              
                   where a.Tenant == tenant || a.Tenant == 0 select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<AnalyticsFactsFieldsMetaData>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<AnalyticsFactsFieldsMetaDataList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<AnalyticsFactsFieldsMetaDataList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<AnalyticsFactsFieldsMetaDataList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AnalyticsFactsFieldsMetaDataList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> AnalyticsFactsFieldsMetaDataObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AnalyticsFactsFieldsMetaData",tenant).ToList();

                ObjectField objectField = (from a in AnalyticsFactsFieldsMetaDataObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<AnalyticsFactsFieldsMetaDataList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsFieldsMetaDataList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsFieldsMetaDataList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsFieldsMetaDataList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsFieldsMetaDataList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsFieldsMetaDataList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsFieldsMetaDataList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.FieldCode);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.FieldCode);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<AnalyticsFactsFieldsMetaDataList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public AnalyticsFactsFieldsMetaDataList GetSingle(string id)
        {
            IQueryable<AnalyticsFactsFieldsMetaData> AnalyticsFactsFieldsMetaDataQuery = (from a in context.AnalyticsFactsFieldsMetaDatas
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<AnalyticsFactsFieldsMetaDataList> AnalyticsFactsFieldsMetaDataListQuery = GetIqueryableList( AnalyticsFactsFieldsMetaDataQuery);
            AnalyticsFactsFieldsMetaDataList AnalyticsFactsFieldsMetaDataList = AnalyticsFactsFieldsMetaDataListQuery.FirstOrDefault();
            return AnalyticsFactsFieldsMetaDataList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AnalyticsFactsFieldsMetaData> iQueryable = (from a in context.AnalyticsFactsFieldsMetaDatas 
                   where a.Tenant == tenant || a.Tenant == 0 select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<AnalyticsFactsFieldsMetaData>(nonListQueryOperation, iQueryable);



            IQueryable<AnalyticsFactsFieldsMetaDataList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<AnalyticsFactsFieldsMetaDataList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<AnalyticsFactsFieldsMetaDataList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 