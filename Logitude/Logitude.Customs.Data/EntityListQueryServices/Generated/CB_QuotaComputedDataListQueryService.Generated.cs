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

    public partial class CB_QuotaComputedDataListQueryService
    {
         private ICustomContext context;
        public CB_QuotaComputedDataListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CB_QuotaComputedDataList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<CB_QuotaComputedDataList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CB_QuotaComputedData> iQueryable = (from a in context.CB_QuotaComputedDatas
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<CB_QuotaComputedData>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CB_QuotaComputedDataList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CB_QuotaComputedDataList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<CB_QuotaComputedDataList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CB_QuotaComputedDataList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CB_QuotaComputedDataObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CB_QuotaComputedData",tenant).ToList();

                ObjectField objectField = (from a in CB_QuotaComputedDataObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CB_QuotaComputedDataList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CB_QuotaComputedDataList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CB_QuotaComputedDataList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CB_QuotaComputedDataList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CB_QuotaComputedDataList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CB_QuotaComputedDataList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CB_QuotaComputedDataList, decimal>(queryOperations, query2);
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

         public List<CB_QuotaComputedDataList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CB_QuotaComputedDataList GetSingle(string cb_id)
        {
            IQueryable<CB_QuotaComputedData> CB_QuotaComputedDataQuery = (from a in context.CB_QuotaComputedDatas
                                                       where a.CB_ID == cb_id
                                                       select a);

             
            IQueryable<CB_QuotaComputedDataList> CB_QuotaComputedDataListQuery = GetIqueryableList( CB_QuotaComputedDataQuery);
            CB_QuotaComputedDataList CB_QuotaComputedDataList = CB_QuotaComputedDataListQuery.FirstOrDefault();
            return CB_QuotaComputedDataList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations ){
		 		  return GetListCount(queryOperations, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CB_QuotaComputedData> iQueryable = (from a in context.CB_QuotaComputedDatas  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<CB_QuotaComputedData>(nonListQueryOperation, iQueryable);



            IQueryable<CB_QuotaComputedDataList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CB_QuotaComputedDataList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<CB_QuotaComputedDataList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 