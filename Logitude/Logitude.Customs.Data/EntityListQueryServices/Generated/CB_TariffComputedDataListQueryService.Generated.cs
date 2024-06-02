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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CB_TariffComputedDataListQueryService
    {
         private ICustomContext context;
        public CB_TariffComputedDataListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CB_TariffComputedDataList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<CB_TariffComputedDataList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CB_TariffComputedData> iQueryable = (from a in context.CB_TariffComputedDatas
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<CB_TariffComputedData>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CB_TariffComputedDataList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CB_TariffComputedDataList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<CB_TariffComputedDataList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CB_TariffComputedDataList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CB_TariffComputedDataObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CB_TariffComputedData",tenant).ToList();

                ObjectField objectField = (from a in CB_TariffComputedDataObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CB_TariffComputedDataList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CB_TariffComputedDataList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CB_TariffComputedDataList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CB_TariffComputedDataList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CB_TariffComputedDataList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CB_TariffComputedDataList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CB_TariffComputedDataList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.CB_ID);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.CB_ID);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<CB_TariffComputedDataList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CB_TariffComputedDataList GetSingle(string cb_id)
        {
            IQueryable<CB_TariffComputedData> CB_TariffComputedDataQuery = (from a in context.CB_TariffComputedDatas
                                                       where a.CB_ID == cb_id
                                                       select a);

             
            IQueryable<CB_TariffComputedDataList> CB_TariffComputedDataListQuery = GetIqueryableList( CB_TariffComputedDataQuery);
            CB_TariffComputedDataList CB_TariffComputedDataList = CB_TariffComputedDataListQuery.FirstOrDefault();
            return CB_TariffComputedDataList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations ){
		 		  return GetListCount(queryOperations, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CB_TariffComputedData> iQueryable = (from a in context.CB_TariffComputedDatas  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<CB_TariffComputedData>(nonListQueryOperation, iQueryable);



            IQueryable<CB_TariffComputedDataList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CB_TariffComputedDataList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<CB_TariffComputedDataList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 